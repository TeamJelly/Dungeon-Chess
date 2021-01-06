using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category { NULL, Party, Neutral, Friendly, Enemy, Boss};
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };
public enum UnitAI {NULL, AI1, AI2, AI3, AI4 };

public class Unit : MonoBehaviour
{
    [Header("Normal Status")]
    public new string name = "NoName";

    public Category category = Category.NULL;
    public UnitClass unitClass = UnitClass.NULL;
    public UnitAI unitAI = UnitAI.NULL;

    [Range(1,99)]
    public int level;

    [Min(0)]
    public int currentHP, maxHP;

    [Min(0)]
    public int currentEXP, nextEXP;

    [Header("Position & Size Status")]
    public UnitPosition unitPosition;
    public Vector2Int size;

    [Header("Basic Status")]
    public int strength;
    public int defense;

    [Header("Special Status")]
    public int agility;
    public int move;
    public int critical;

    [Header("Hidden Status")]
    public float actionRate; // 행동 퍼센테이지
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

    public void GetEXP(int getEXP)
    {
        while (getEXP != 0)
        {
            int needEXP = nextEXP - currentEXP;
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
        nextEXP += 10;
        Debug.Log(name + " level Up!");
    }

    public void GetMaxHP(int number)
    {
        maxHP += number;
        currentHP += number;
    }

    public void LoseMaxHP(int number)
    {
        maxHP -= number;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public bool IsContainPostion(Vector2Int position)
    {
        if (unitPosition.lowerLeft.x <= position.x && unitPosition.lowerLeft.y <= position.y && unitPosition.upperRight.x >= position.x && unitPosition.upperRight.y >= position.y)
            return true;
        else
            return false;
    }

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

    public List<UnitPosition> GetMovablePosition(){ // 현재 유닛의 이동가능한 위치들을 리턴해준다.

        List<UnitPosition> explored = new List<UnitPosition>();
        List<UnitPosition> frontier = new List<UnitPosition>();

        frontier.Add(unitPosition);

        for (int i = 0; i < move; i++)
        {
            List<UnitPosition> nextFrontier = new List<UnitPosition>();
            foreach (var f in frontier)
            {
                foreach (var neighborPosition in UnitPosition.GetNeighborPosition(f))
                {
                    bool isExplored = false;

                    foreach (var e in explored)
                        if (neighborPosition.Equals(e))
                            isExplored = true;

                    if (isExplored.Equals(false)) // 아이템이 이미 탐색한곳이 아니라면
                        nextFrontier.Add(neighborPosition); // 프론티어에 추가
                }
                explored.Add(f);
            }
            frontier = nextFrontier;
        }

        return explored;
    }

    public void Move(Vector2Int position)
    {
/*        foreach (var item in BattleManager.PathFindAlgorithm(unitPosition, new UnitPosition(position, position)))
        {
            item.ShowUnitPosition();
        }*/

        BeforeMove();

        //기존 타일 유닛 초기화
        for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
        {
            for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
            {
                BattleManager.instance.AllTiles[i, j].unit = null;
            }
        }

        //일단 유닛 크기 1x1만 이동 가능하도록 설정
        unitPosition.lowerLeft.x = position.x;
        unitPosition.upperRight.x = position.x;

        unitPosition.lowerLeft.y = position.y;
        unitPosition.upperRight.y = position.y;


        //새로 이동한 타일에 유닛 할당
        for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
        {
            for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
            {
                BattleManager.instance.AllTiles[i, j].unit = this;
            }
        }
        //화면상 위치 갱신.
        Vector2 screenPosition = unitPosition.lowerLeft + (unitPosition.upperRight - unitPosition.lowerLeft) / 2;
        transform.position = screenPosition;

        AfterMove();
    }

    public void Move(List<UnitPosition> position) // 거인 & 1x1 통합본
    {
/*        BeforeMove();

        //기존 타일 유닛 초기화
        for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
        {
            for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
            {
                BattleManager.instance.AllTiles[i, j].unit = null;
            }
        }

        //일단 유닛 크기 1x1만 이동 가능하도록 설정
        unitPosition.lowerLeft = position.lowerLeft;
        unitPosition.upperRight.x = position.x;

        unitPosition.lowerLeft.y = position.y;
        unitPosition.upperRight.y = position.y;


        //새로 이동한 타일에 유닛 할당
        for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
        {
            for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
            {
                BattleManager.instance.AllTiles[i, j].unit = this;
            }
        }
        //화면상 위치 갱신.
        Vector2 screenPosition = unitPosition.lowerLeft + (unitPosition.upperRight - unitPosition.lowerLeft) / 2;
        transform.position = screenPosition;

        AfterMove();*/
    }

    public void GetDamage(int number)
    {
        currentHP -= number * (100 - defenseRate);

//        if (currentHP < 0)
//            Die();
    }

    public void GetHeal(int number)
    {
        if (currentHP + number < maxHP)
            currentHP += number;
        else
            currentHP = maxHP;
    }

    public void GetStateEffects(int StateEffect)
    {

    }

    public void DebugTestUnit()
    {
        Debug.LogError("i'm" + name);
    }

}

