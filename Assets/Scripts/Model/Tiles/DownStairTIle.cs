using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;
public class DownStairTIle : Tile
{
    public override void OnTile()
    {
        Debug.Log("!");
        if(!GameManager.PartyUnits.Contains(unit)) return;
        int partyObjCnt = 0;
        foreach(Unit unit in GameManager.PartyUnits)
        {
            if(BattleView.UnitObjects.ContainsKey(unit))
                partyObjCnt++;
        }

        Debug.Log(partyObjCnt);

        if(partyObjCnt == 1)
        {
            StageView.instance.Enable();
        }
        else
        {
            Common.UnitAction.UnSummon(unit);
        }
    }
    
}
