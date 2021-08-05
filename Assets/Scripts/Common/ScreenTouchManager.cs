using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using View;

public class ScreenTouchManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public Vector2 LeftDownLimit = Vector2.zero;
    public Vector2 RightUpLimit = new Vector2(16, 16);
    public Transform cameraTransform;
    public float dragSpeed = 15f;
    Vector3 currentPos = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = Camera.main.ScreenToViewportPoint(eventData.position);
    }
    private void OnMouseOver() {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
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
                coroutine = GameManager.LeaderUnit.Skills[Model.SkillCategory.Move].Use(GameManager.LeaderUnit, tileIdx);
                StartCoroutine(coroutine);
            }
        }
        else
        {
            IndicatorView.TileIndicators[tileIdx.x, tileIdx.y].GetComponent<EventTrigger>().OnPointerClick(null);
        }
    }
}
