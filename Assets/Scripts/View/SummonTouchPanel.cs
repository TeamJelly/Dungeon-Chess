using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class SummonTouchPanel : MonoBehaviour, IPointerDownHandler
    {


        // public void OnDrag(PointerEventData eventData)
        // {
        //     Vector3 pos = Camera.main.ScreenToViewportPoint(eventData.position);
        //     Vector2Int tileIdx = new Vector2Int((int)pos.x, (int)pos.y);

        //     if (Model.Managers.FieldManager.IsInField(tileIdx))
        //     {
        //         Debug.Log("hello" + tileIdx);
        //     }
        // }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position) + Vector3.one * 0.5f;
            Vector2Int tileIdx = new Vector2Int((int)pos.x, (int)pos.y);

            // Debug.Log("vec3" + pos);
            // Debug.Log("vec2" + tileIdx);

            if (Model.Managers.FieldManager.IsInField(tileIdx) && DungeonEditor.instance.selectedIndex > 0)
            {
                // if (DungeonEditor.instance.selectedObject.GetType().Name.Contains("Unit"))
                //     Common.Command.Summon()
            }
        }
    }

}
