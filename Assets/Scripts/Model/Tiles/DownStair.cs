﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;
using UI.Battle;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class DownStair : Tile
    {
        public DownStair()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/stair_down");
            category = TileCategory.DownStair;
        }

        public override void OnTile(Unit unit)
        {
            base.OnTile(unit);

            int downStairUnitCount = 0;

            foreach (Unit partyUnit in GameManager.PartyUnits)
                if (FieldManager.GetTile(partyUnit.Position).category == TileCategory.DownStair)
                    downStairUnitCount++;
            
            if (downStairUnitCount == GameManager.PartyUnits.Count)
            {
                foreach (Unit partyUnit in GameManager.PartyUnits)
                {
                    Common.Command.UnSummon(partyUnit);
                }                                    
                MapView.instance.Enable();
            }
        }

        // public override void OnTile(Unit unit)
        // {
        //     base.OnTile(unit);

        //     if (!GameManager.PartyUnits.Contains(unit)) return;

        //     int partyObjCnt = 0;

        //     foreach (Unit party in GameManager.PartyUnits)
        //     {
        //         if (BattleView.UnitObjects.ContainsKey(party))
        //             partyObjCnt++;
        //     }

        //     Debug.Log(partyObjCnt);

        //     if (partyObjCnt == 1)
        //     {
        //         MapView.instance.IsClickBlock = false;
        //         MapView.instance.Enable();
        //     }
        //     else
        //     {
        //         Common.Command.UnSummon(unit);
        //         BattleManager.instance.thisTurnUnit = null;
        //         if (GameManager.InBattle) BattleController.instance.NextTurnStart();
        //     }
        // }
    }
}