using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category { NULL, Party, Neutral, Friendly, EnemyAI1, EnemyAI2, BossAI1, BossAI2 };
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };

[System.Serializable]
public class Position
{
    public Vector2Int LowerLeft, UpperRight;
}

public class Unit : MonoBehaviour
{
    [Header("Normal Status")]
    public string unitName = "No Name";

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
        Debug.Log(unitName + " level Up!");
    }

    [Header("Position & Size Status")]
    public Position position;
    public Vector2Int size;

    [Header("Basic Status")]
    public int vitality;
    public int strength;
    public int defense;

    [Header("Special Status")]
    public int agility;
    public int move;
    public int critical;

    [Header("Hidden Status")]
    public int actionRate;
    public int currentAct;
    public int defenseRate;

    [Header("Count Status")]
    public int moveCount;
    public int skillCount;
    public int itemCount;

    [Header("Having")]
    public List<GameObject> skills;
    public List<GameObject> antiques;
    public List<GameObject> items;
    public List<GameObject> stateEffects;

}
