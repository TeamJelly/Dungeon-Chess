using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category { NULL, Party, Neutral, Friendly, EnemyAI1, EnemyAI2, BossAI1, BossAI2 };
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };

[System.Serializable]
public class UnitPosition
{
    public Vector2Int lowerLeft, upperRight;
}

public class Unit : MonoBehaviour
{
    [Header("Normal Status")]
    public new string name = "NoName";

    public Category category = Category.NULL;
    public UnitClass unitClass = UnitClass.NULL;

    [Range(1,99)]
    public int level;

    [Min(0)]
    public int money;

    [Min(0)]
    public int currentHP, maxHP;

    [Min(0)]
    public int currentEXP, maxEXP;

    public void GetEXP(int getEXP)
    {
        while (getEXP != 0)
        {
            int needEXP = maxEXP - currentEXP;
            if (needEXP < getEXP)
            {
                getEXP -= needEXP;
                LevelUp();
            }
            else
            {
                currentEXP += getEXP;
                getEXP = 0;
            }
        }
    }

    public void LevelUp()
    {
        level += 1;
        currentEXP = 0;
        maxEXP += 10;
        Debug.Log(name + " level Up!");
    }

    [Header("Position & Size Status")]
    public UnitPosition unitPosition;
    public Vector2Int size;

    public bool isContainPostion(Vector2Int position)
    {
        if (unitPosition.lowerLeft.x <= position.x && unitPosition.lowerLeft.y <= position.y && unitPosition.upperRight.x >= position.x && unitPosition.upperRight.y >= position.y)
            return true;
        else
            return false;
    }

    [Header("Basic Status")]
    public int vitality;
    public int strength;
    public int defense;

    [Header("Special Status")]
    public int agility;
    public int move;
    public int critical;

    [Header("Hidden Status")]
    public float actionRate; // 행동 퍼센테이지
    public int currentAct;
    public int defenseRate;

    [Header("Count Status")]
    public int moveCount;
    public int skillCount;
    public int itemCount;

    [Header("Having")]
    public List<Skill> skills;
    public List<Antique> antiques;
    public List<Item> items;
    public List<Effect> stateEffects;

    public void BeforeMove()
    {
        foreach (var stateEffect in stateEffects)
        {
            stateEffect.BeforeMove();
        }
    }
    public void AfterMove()
    {
        foreach (var stateEffect in stateEffects)
        {
            stateEffect.AfterMove();
        }

        for (int i = unitPosition.lowerLeft.x; i < unitPosition.upperRight.x; i++)
            for (int j = unitPosition.lowerLeft.y; j < unitPosition.upperRight.y; j++)
                BattleManager.instance.GetTile(i, j).tileEffect.AfterMove();
    }

    public List<UnitPosition> GetMovablePosition(){

        List<Vector2Int> movableVector2s = new List<Vector2Int>();


        for (int i = -move; i < move; i++)
        {
            for (int j = -(move-Math.Abs(i)); j <= (move - Math.Abs(i)); j++)
            {
                if (i == 0 && j == 0)
                    continue;
                movableVector2s.Add(new Vector2Int(i, j));
            }
        }

        List<UnitPosition> movableUnitPositions = new List<UnitPosition>();
        foreach (var movableVector2 in movableVector2s)
        {
            UnitPosition tempPosition = new UnitPosition();
            tempPosition.lowerLeft.x = unitPosition.lowerLeft.x + movableVector2.x;
            tempPosition.lowerLeft.y = unitPosition.lowerLeft.y + movableVector2.y;
            tempPosition.upperRight.x = unitPosition.upperRight.x + movableVector2.x;
            tempPosition.upperRight.y = unitPosition.upperRight.y + movableVector2.y;
            movableUnitPositions.Add(tempPosition);
        }

        return movableUnitPositions;
    }

    public void Move(UnitPosition movePosition)
    {
        BeforeMove();

        unitPosition = movePosition;

        //화면상 위치 갱신.
        Vector2 screenPosition = movePosition.lowerLeft + (movePosition.upperRight - movePosition.lowerLeft) / 2;
        transform.position = screenPosition;

        AfterMove();
    }

    public void GetDamage()
    {
        throw new NotImplementedException();
    }

    public void GetStateEffects(int StateEffect)
    {
        throw new NotImplementedException();
    }

    public void HelloUnit()
    {
        Debug.LogError("i'm");
    }

}

