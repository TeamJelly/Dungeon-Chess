using System.Collections;
using System.Collections.Generic;
using Model;
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
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Debug.Log("vec3" + pos);
                // Debug.Log("vec2" + tileIdx);

                if (Model.Managers.FieldManager.IsInField(tileIdx) && DungeonEditor.instance.selectedIndex >= 0)
                {
                    Debug.Log(DungeonEditor.instance.selectedObject);

                    //유닛 소환
                    if (DungeonEditor.instance.selectedObject is string)
                    {
                        Unit_Serializable u = Common.Data.Load_Unit_Serializable_Data((string)DungeonEditor.instance.selectedObject);
                        Unit unit = new Unit(u);
                        Common.Command.Summon(unit, tileIdx);
                    }

                    else if (DungeonEditor.instance.selectedObject is Item)
                        Common.Command.Summon(((Item)DungeonEditor.instance.selectedObject).Clone(), tileIdx);

                    else if (DungeonEditor.instance.selectedObject is Artifact)
                        Common.Command.Summon(((Artifact)DungeonEditor.instance.selectedObject).Clone(), tileIdx);
                    // if (DungeonEditor.instance.selectedObject is Tile)
                    //     Common.Command.Summon((Unit) DungeonEditor.instance.selectedObject, tileIdx);
                }
            }

            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                if (Model.Managers.FieldManager.IsInField(tileIdx) && DungeonEditor.instance.selectedIndex >= 0)
                {
                    Common.Command.UnSummon(tileIdx);
                }
            }
            
        }
    }
}