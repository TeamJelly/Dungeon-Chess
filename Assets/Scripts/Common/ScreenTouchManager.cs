using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using View;
using Model;
using View.UI;
using DG.Tweening;

public class ScreenTouchManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static ScreenTouchManager instance;

    private void Awake() => instance = this;

    public Vector2 LeftDownLimit = Vector2.zero;
    public Vector2 RightUpLimit;
    public Transform cameraTransform;
    public float dragSpeed = 15f;
    Vector3 currentPos = Vector3.zero;
    Vector2 longClickPos = Vector2.zero;

    public bool isDraging = false;

    public void Move(Vector2Int position)
    {
        cameraTransform.DOMove(new Vector3(position.x, position.y, cameraTransform.position.z), 0.1f);
    }


    public void OnDrag(PointerEventData eventData)
    {
        // 롱클릭 갱신용 위치
        longClickPos = eventData.position;

        Vector3 currentPos_bef = currentPos;
        currentPos = Camera.main.ScreenToViewportPoint(eventData.position);
        Vector3 delta = currentPos_bef - currentPos;
        Vector3 nextPos = cameraTransform.position + delta * dragSpeed;
        nextPos.x = Mathf.Clamp(nextPos.x, LeftDownLimit.x, RightUpLimit.x);
        nextPos.y = Mathf.Clamp(nextPos.y, LeftDownLimit.y, RightUpLimit.y);
        cameraTransform.position = nextPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = Camera.main.ScreenToViewportPoint(eventData.position);
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
    }

    IEnumerator coroutine = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDraging) return;

        Vector3 mousepos = Camera.main.ScreenToWorldPoint(eventData.position) + Vector3.one * 0.5f;

        if (mousepos.x < LeftDownLimit.x || mousepos.x >= RightUpLimit.x || mousepos.y < LeftDownLimit.y || mousepos.y >= RightUpLimit.y) return;

        //Vector2Int selectedTileIdx = new Vector2Int(Mathf.Clamp((int)mousepos.x, 0, 15), Mathf.Clamp((int)mousepos.y, 0, 15));
        Vector2Int tileIdx = new Vector2Int((int)mousepos.x, (int)mousepos.y);

        // Debug.Log(tileIdx);

        if (IndicatorView.TileIndicatorParent.activeSelf)
        {
            IndicatorView.TileIndicators[tileIdx.y, tileIdx.x].GetComponent<EventTrigger>().OnPointerClick(null);
        }
        else if (!GameManager.InBattle)
        {
            if (coroutine != null) return;

            if (FieldManager.GetTile(tileIdx).HasUnit())
            {
                //리더 유닛 이동 코루틴 실행. 기존 실행되던 코루틴은 정지.
                //if (coroutine != null) StopCoroutine(coroutine);
                //coroutine = move(GameManager.LeaderUnit, tileIdx);
                //StartCoroutine(coroutine);
                BattleManager.instance.thisTurnUnit = FieldManager.GetTile(tileIdx).GetUnit();
            }
            else if (BattleManager.instance.thisTurnUnit != null)
            {
                coroutine = move(BattleManager.instance.thisTurnUnit, tileIdx);
                StartCoroutine(coroutine);
            }
        }
    }

    IEnumerator move(Unit user, Vector2Int target)
    {
        Vector2Int startPosition = user.Position;
        List<Vector2Int> path = Common.PathFind.PathFindAlgorithm(user, user.Position, target);

        if (path != null)
        {
            user.animationState = Unit.AnimationState.Move;

            float moveTime = 0.5f / path.Count;

            for (int i = 1; i < path.Count; i++)
            {
                View.VisualEffectView.MakeVisualEffect(user.Position, "Dust");
                user.Position = path[i];
                yield return new WaitForSeconds(moveTime);
            }
            user.animationState = Unit.AnimationState.Idle;

            // 실제 타일에 상속되는건 한번이다.
            Common.Command.Move(user, startPosition, target);
        }
        yield return null;
        coroutine = null;
    }

    IEnumerator longclick = null;
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("pointer down");

        if (longclick == null)
        {
            // 롱클릭 시작 위치
            Vector2 longClickStart = eventData.position;
            longClickPos = longClickStart;
            longclick = LongClick(longClickStart);
            StartCoroutine(longclick);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (longclick != null)
        {
            StopCoroutine(longclick);
            longclick = null;
        }
    }

    IEnumerator LongClick(Vector2 target)
    {
        yield return new WaitForSeconds(1f);

        if ((target - longClickPos).magnitude < Screen.width / 10)
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(target) + Vector3.one * 0.5f;
            Vector2Int tileIdx = new Vector2Int((int)mousepos.x, (int)mousepos.y);
            if (FieldManager.IsInField(tileIdx))
            {
                InfoView.instance.infoPanel.SetActive(true);
                Tile tile = FieldManager.GetTile(tileIdx);
                InfoView.Show(tile);

                Unit unit = tile.GetUnit();
                if (unit != null)
                    InfoView.Show(unit);

                Obtainable obtainable = tile.GetObtainable();
                if (obtainable != null)
                    InfoView.Show(obtainable);
            }
        }
    }

}
