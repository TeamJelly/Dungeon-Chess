using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<Unit> AllUnits;
    Tile[,] AllTiles; //10x10일때 0,0 ~ 9,9으로
    Unit thisTurnUnit;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BattleUI.instance.Init();
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
        float minTime = 100;
        Unit nextUnit = null; // 다음 턴에 행동할 유닛

        foreach (var unit in AllUnits)
        {
            float velocity = unit.agility * 10 + 100;
            float time = (max - unit.actionRate) / velocity; // 거리 = 시간 * 속력 > 시간 = 거리 / 속력
            if (minTime >= time) // 시간이 가장 적게 걸리는애가 먼저된다.
            {
                minTime = time;
                nextUnit = unit;
            }
        }

        foreach (var unit in AllUnits)
        {
            float velocity = unit.agility * 10 + 100;
            unit.actionRate += velocity * minTime;
        }

        TurnStart(nextUnit);
    }

    public void TurnStart(Unit unit)
    {
        thisTurnUnit = unit;
        thisTurnUnit.actionRate = 0;

        //턴 상태 갱신(이동 가능한 타일 보여주기)
        BattleUI.instance.UpdateTurnStatus(unit);
//        thisTurnUnit.getMovableT
  //      BattleUI
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
