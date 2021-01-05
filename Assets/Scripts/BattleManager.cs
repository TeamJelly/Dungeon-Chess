using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    List<Unit> AllUnits;
    Tile[,] AllTiles; //10x10일때 0,0 ~ 9,9으로
    Unit thisTurnUnit;

    void Start()
    {
        instance = this;
    }


    public Tile GetTile(Vector2Int position)
    {
        return AllTiles[position.x, position.y];
    }
    public Tile GetTile(int x, int y)
    {
        return GetTile(new Vector2Int(x, y));
    }


    public void SetNextTurn()
    {
        float max = 100; // 주기의 최댓값
        float minValue = 100;
        Unit nextUnit = null; // 다음 턴에 행동할 유닛

        foreach (var unit in AllUnits)
        {
            float temp = (max - unit.actionRate) / unit.agility;
            if (minValue >= temp)
            {
                minValue = temp;
                nextUnit = unit;
            }
        }

        foreach (var unit in AllUnits)
        {
            unit.actionRate += unit.agility * minValue;
        }

        TurnStart(nextUnit);
    }

    public void TurnStart(Unit unit)
    {
        thisTurnUnit = unit;
        // 현재 유닛 가운데로
        // UI 갱신 스킬, 아이템 갱신

        // unit.OnTurnStart();
        // 끝냄
    }

    public void OnBattleStart()
    {
        foreach (var unit in AllUnits)
        {
            foreach (var effect in unit.stateEffects)
                effect.OnBattleStart();
        }
    }

    public void OnBattleEnd()
    {
        foreach (var unit in AllUnits)
        {
            foreach (var effect in unit.stateEffects)
                effect.OnBattleEnd();
        }
    }

}
