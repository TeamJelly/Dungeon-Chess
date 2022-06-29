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
        Vector2Int currentTileIndex = Vector2Int.one * -1;
        Vector2Int currentTileIndex_bef = Vector2Int.one * -1;
        bool flag_dragging = false;

        Vector2Int GetTileIdx(Vector2 position)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(position) + Vector3.one * 0.5f;
            return new Vector2Int((int)pos.x, (int)pos.y);
        }

        void Perform_Summon(PointerEventData.InputButton input, Vector2Int tileIdx)
        {
            if (!Model.Managers.FieldManager.IsInField(tileIdx)) return;

            if (input == PointerEventData.InputButton.Left)
            {
                // Debug.Log("vec3" + pos);
                // Debug.Log("vec2" + tileIdx);

                if (DungeonEditor.instance.selectedIndex >= 0)
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
                        newtile.OnTile(oldtile.GetUnit());
                        newtile.SetObtainable(oldtile.GetObtainable());

                        Model.Managers.FieldManager.instance.field[tileIdx.y, tileIdx.x] = newtile;
                        Model.Managers.FieldManager.instance.field[tileIdx.y, tileIdx.x].position = tileIdx;
                        Model.Managers.FieldManager.instance.tileMap.SetTile(tilePosition, newtile.TileBase);
                    }

                    //Common.Command.Summon((Unit) DungeonEditor.instance.selectedObject, tileIdx);
                }
            }
            else if (input == PointerEventData.InputButton.Right)
            {
                Common.Command.UnSummon(tileIdx);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            //드래그 후에 point click 호출 방지
            if (flag_dragging)
            {
                flag_dragging = false;
                currentTileIndex = Vector2Int.one * -1; // 다음 drag를 위해 초기화.
                return;
            }
            Perform_Summon(eventData.button, GetTileIdx(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            flag_dragging = true;
            // Debug.Log(eventData.delta);

            if (eventData.button == PointerEventData.InputButton.Middle)
                Camera.main.transform.position -= (new Vector3(eventData.delta.x, eventData.delta.y, 0)) * 0.01f;
            else
            {
                //같은 타일 내에서 연속 호출 방지
                currentTileIndex_bef = currentTileIndex;
                currentTileIndex = GetTileIdx(eventData.position);
                if (currentTileIndex != currentTileIndex_bef)
                    Perform_Summon(eventData.button, currentTileIndex);
            }
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