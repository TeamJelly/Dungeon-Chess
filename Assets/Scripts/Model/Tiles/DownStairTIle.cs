using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;
public class DownStairTIle : Tile
{
    public override void OnTile(Unit unit)
    {
        base.OnTile(unit);

        Debug.Log("!");
        if(!GameManager.PartyUnits.Contains(unit)) return;

        int partyObjCnt = 0;
        
        foreach(Unit party in GameManager.PartyUnits)
        {
            if(BattleView.UnitObjects.ContainsKey(party))
                partyObjCnt++;
        }

        Debug.Log(partyObjCnt);

        if(partyObjCnt == 1)
        {
            StageView.instance.Enable();
        }
        else
        {
            Common.Command.UnSummon(unit);
        }
    }
    
}
