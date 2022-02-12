// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Model;
// using Model.Managers;
// using View;
// using UI.Battle;
// using UnityEngine.Tilemaps;

// namespace Model.Tiles
// {
//     public class DownStair : Tile
//     {
//         public DownStair()
//         {
//             TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/stair_down");
//             category = TileCategory.DownStair;
//         }

//         public override void OnTile(Unit unit)
//         {
//             // 턴 시작후에 
//             base.OnTile(unit);
//             CheckPartyDownStair();
//         }

//         // 턴시작, 유닛소환, 유닛소환해제, OnTile 시점에서 DownStairButton 활성화 검사코드를 호출한다.
//         public static bool CheckPartyDownStair()
//         {
//             int downStairUnitCount = 0;
//             foreach (Unit partyUnit in GameManager.PartyUnits)
//                 if (FieldManager.GetTile(partyUnit.Position).category == TileCategory.DownStair)
//                     downStairUnitCount++;

//             // 배틀중이 아니거나, 파티 유닛이 전부 계단에 올라가 있으면 계단 내려가기가 활성화된다.
//             if (downStairUnitCount == GameManager.PartyUnits.Count || GameManager.InBattle == false)
//             {
//                 BattleView.DownStairButton.gameObject.SetActive(true);
//                 return true;
//             }
//             // 조건이 맞지 않다면, 파티 DownStair 리스너를 삭제해준다.
//             else
//             {
//                 BattleView.DownStairButton.gameObject.SetActive(false);
//                 return false;
//             }
//         }


//         // public override void OnTile(Unit unit)
//         // {
//         //     base.OnTile(unit);

//         //     if (!GameManager.PartyUnits.Contains(unit)) return;

//         //     int partyObjCnt = 0;

//         //     foreach (Unit party in GameManager.PartyUnits)
//         //     {
//         //         if (BattleView.UnitObjects.ContainsKey(party))
//         //             partyObjCnt++;
//         //     }

//         //     Debug.Log(partyObjCnt);

//         //     if (partyObjCnt == 1)
//         //     {
//         //         MapView.instance.IsClickBlock = false;
//         //         MapView.instance.Enable();
//         //     }
//         //     else
//         //     {
//         //         Common.Command.UnSummon(unit);
//         //         BattleManager.instance.thisTurnUnit = null;
//         //         if (GameManager.InBattle) BattleController.instance.NextTurnStart();
//         //     }
//         // }
//     }
// }