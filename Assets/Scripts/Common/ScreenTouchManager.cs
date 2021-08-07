using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using View;
using Model;

public class ScreenTouchManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 LeftDownLimit = Vector2.zero;
    public Vector2 RightUpLimit = new Vector2(16, 16);
    public Transform cameraTransform;
    public float dragSpeed = 15f;
    Vector3 currentPos = Vector3.zero;
    Vector2 longClickPos = Vector2.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = Camera.main.ScreenToViewportPoint(eventData.position);
    }
    private void OnMouseOver()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        longClickPos = eventData.position;

        Vector3 currentPos_bef = currentPos;
        currentPos = Camera.main.ScreenToViewportPoint(eventData.position);
        Vector3 delta = currentPos_bef - currentPos;
        Vector3 nextPos = cameraTransform.position + delta * dragSpeed;
        nextPos.x = Mathf.Clamp(nextPos.x, LeftDownLimit.x, RightUpLimit.x);
        nextPos.y = Mathf.Clamp(nextPos.y, LeftDownLimit.y, RightUpLimit.y);
        cameraTransform.position = nextPos;
    }

    IEnumerator coroutine;
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(eventData.position) + Vector3.one * 0.5f;

        if (mousepos.x < LeftDownLimit.x || mousepos.x >= RightUpLimit.x || mousepos.y < LeftDownLimit.y || mousepos.y >= RightUpLimit.y) return;

        //Vector2Int selectedTileIdx = new Vector2Int(Mathf.Clamp((int)mousepos.x, 0, 15), Mathf.Clamp((int)mousepos.y, 0, 15));
        Vector2Int tileIdx = new Vector2Int((int)mousepos.x,(int)mousepos.y);

        if(!GameManager.InBattle)
        {
            if (FieldManager.GetTile(tileIdx).IsPositionable(GameManager.LeaderUnit))
            {
                //리더 유닛 이동 코루틴 실행. 기존 실행되던 코루틴은 정지.
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = move(GameManager.LeaderUnit, tileIdx);
                StartCoroutine(coroutine);
            }
        }
        else
        {
            IndicatorView.TileIndicators[tileIdx.x, tileIdx.y].GetComponent<EventTrigger>().OnPointerClick(null);
        }
    }

    IEnumerator move(Unit user, Vector2Int target)
    {
        Vector2Int startPosition = user.Position;
        List<Vector2Int> path = Common.PathFind.PathFindAlgorithm(user, user.Position, target);

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
        Common.Command.Move(user,startPosition, target);
    }

    IEnumerator longclick = null;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointer down");
        if (longclick == null)
        {
            Vector2 mousepos = eventData.position;
            longClickPos = mousepos;
            longclick = LongClick(mousepos);
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
        Debug.Log("롱클릭 시작" + longClickPos);

        yield return new WaitForSeconds(1f);

        Debug.Log(target +", "+ longClickPos + ", "+ (target - longClickPos).magnitude);

        if ((target - longClickPos).magnitude < Screen.width / 10)
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(target) + Vector3.one * 0.5f;
            Vector2Int tileIdx = new Vector2Int((int)mousepos.x,(int)mousepos.y);
            Unit unit = BattleManager.GetUnit(tileIdx);
            if (unit != null)
                Debug.Log(unit.Name);
        }
    }
}
