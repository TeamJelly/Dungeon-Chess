using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace View
{
    public class SummonTouchPanel : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position) + Vector3.one * 0.5f;
            Vector2Int tileIdx = new Vector2Int((int)pos.x, (int)pos.y);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Debug.Log("vec3" + pos);
                // Debug.Log("vec2" + tileIdx);

                if (Model.Managers.FieldManager.IsInField(tileIdx) && DungeonEditor.instance.selectedIndex >= 0)
                {
                    // Debug.Log(DungeonEditor.instance.selectedObject);

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

                    else if (DungeonEditor.instance.selectedObject is Tile)
                    {
                        Vector3Int tilePosition = new Vector3Int(tileIdx.x, tileIdx.y, 0);
                        Tile newtile = ((Tile)DungeonEditor.instance.selectedObject).Clone();
                        Tile oldtile = Model.Managers.FieldManager.GetTile(tileIdx.x, tileIdx.y);

                        // 이전 타일 연결 관계 유지.
                        newtile.SetUnit(oldtile.GetUnit());
                        newtile.SetObtainable(oldtile.GetObtainable());

                        Model.Managers.FieldManager.instance.field[tileIdx.y, tileIdx.x] = newtile;
                        Model.Managers.FieldManager.instance.field[tileIdx.y, tileIdx.x].position = tileIdx;
                        Model.Managers.FieldManager.instance.tileMap.SetTile(tilePosition, newtile.TileBase);
                    }

                    //Common.Command.Summon((Unit) DungeonEditor.instance.selectedObject, tileIdx);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (Model.Managers.FieldManager.IsInField(tileIdx) /*&& DungeonEditor.instance.selectedIndex >= 0*/)
                {
                    Common.Command.UnSummon(tileIdx);
                }
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            // Debug.Log(eventData.delta);

            if (eventData.button == PointerEventData.InputButton.Middle)
                Camera.main.transform.position -= (new Vector3(eventData.delta.x, eventData.delta.y, 0)) * 0.01f;
            else
                OnPointerClick(eventData);

            // Camera.main.transform.DOLocalMove(position, 0.1f);
        }

        // public void OnPointerDown(PointerEventData eventData)
        // {
        //     Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position) + Vector3.one * 0.5f;
        //     Vector2Int tileIdx = new Vector2Int((int)pos.x, (int)pos.y);
        //     if (eventData.button == PointerEventData.InputButton.Left)
        //     {
        //         // Debug.Log("vec3" + pos);
        //         // Debug.Log("vec2" + tileIdx);

        //         if (Model.Managers.FieldManager.IsInField(tileIdx) && DungeonEditor.instance.selectedIndex >= 0)
        //         {
        //             Debug.Log(DungeonEditor.instance.selectedObject);

        //             //유닛 소환
        //             if (DungeonEditor.instance.selectedObject is string)
        //             {
        //                 Unit_Serializable u = Common.Data.Load_Unit_Serializable_Data((string)DungeonEditor.instance.selectedObject);
        //                 Unit unit = new Unit(u);
        //                 Common.Command.Summon(unit, tileIdx);
        //             }

        //             else if (DungeonEditor.instance.selectedObject is Item)
        //                 Common.Command.Summon(((Item)DungeonEditor.instance.selectedObject).Clone(), tileIdx);

        //             else if (DungeonEditor.instance.selectedObject is Artifact)
        //                 Common.Command.Summon(((Artifact)DungeonEditor.instance.selectedObject).Clone(), tileIdx);

        //             else if (DungeonEditor.instance.selectedObject is Tile)
        //             {
        //                 Vector3Int tilePosition = new Vector3Int(tileIdx.x, tileIdx.y, 0);
        //                 Tile newtile = ((Tile)DungeonEditor.instance.selectedObject).Clone();
        //                 Model.Managers.FieldManager.instance.field[tileIdx.y, tileIdx.x] = newtile;
        //                 Model.Managers.FieldManager.instance.field[tileIdx.y, tileIdx.x].position = tileIdx;
        //                 Model.Managers.FieldManager.instance.tileMap.SetTile(tilePosition, newtile.TileBase);
        //             }

        //             //Common.Command.Summon((Unit) DungeonEditor.instance.selectedObject, tileIdx);
        //         }
        //     }

        //     else if (eventData.button == PointerEventData.InputButton.Right)
        //     {
        //         if (Model.Managers.FieldManager.IsInField(tileIdx) && DungeonEditor.instance.selectedIndex >= 0)
        //         {
        //             Common.Command.UnSummon(tileIdx);
        //         }
        //     }

        // }
    }
}