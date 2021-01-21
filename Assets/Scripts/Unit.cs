using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [Header("Basic Status")]
    public int strength;
    // public int defense;

    [Header("Special Status")]
    public int agility;
    public int move;
    public int critical;

    [Header("Hidden Status")]
    public float actionRate; // 행동 퍼센테이지
    // public int defenseRate;

    [Header("Count Status")]
    public int moveCount;
    public int skillCount;
    public int itemCount;

    private List<Skill> _skills = new List<Skill>();
    public List<Skill> skills { get => _skills; }
    [Header("Having")]
    public List<string> havingSkills;
    public List<Antique> antiques;
    public List<Item> items;
    public List<Effect> stateEffects;

    // 초기화 함수
    private void Awake()
    {
        AwakeSkills();
    }

    /// <summary>
    /// 처음에 스킬 가지고 있는 것을 초기화
    /// </summary>
    private void AwakeSkills()
    {
        for (int i = 0; i < havingSkills.Count; i++)
        {
            if (havingSkills[i] != null)
            {
                addSkillComponent(havingSkills[i]);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillName">스킬 이름</param>
    private void addSkillComponent(string skillName)
    {
        var skill = (Skill)gameObject.AddComponent(Type.GetType(skillName));
        skills.Add(skill);
    }

    /// <summary>
    /// (public) 스킬 등록
    /// </summary>
    /// <param name="newSkill">등록하려는 스킬 이름</param>
    /// <param name="index">슬롯 위치</param>
    public void setSkill(string newSkill, int index)
    {
        if (index >= skills.Count)
        {
            //TODO UI Error Message
            return;
        }
        havingSkills[index] = newSkill;
        addSkillComponent(newSkill);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">slot 위치</param>
    public void removeSkill(int index)
    {
        if (index >= skills.Count || havingSkills[index] == null || skills[index] == null)
        {
            //TODO UI Error Message
            return;
        }
        havingSkills[index] = "";
        Destroy(skills[index]);
    }

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
                BattleManager.instance.GetTile(i, j).tileEffect?.AfterMove();
    }

    public List<UnitPosition> GetMovableUnitPositions(){ // 현재 유닛의 이동가능한 위치들을 리턴해준다.

        List<UnitPosition> explored = new List<UnitPosition>();
        List<UnitPosition> frontier = new List<UnitPosition>();

        frontier.Add(unitPosition);

        for (int i = 0; i < move; i++)
        {
            List<UnitPosition> nextFrontier = new List<UnitPosition>();
            foreach (var f in frontier)
            {
                foreach (var neighborPosition in UnitPosition.GetNeighborPosition(f, this))
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

    public List<Vector2Int> GetMovablePositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (UnitPosition unitPosition in GetMovableUnitPositions())
        {
            foreach (var position in unitPosition.GetPositions())
            {
                bool isContain = false;

                foreach (var item in positions)
                {
                    if (item.x == position.x && item.y == position.y)
                        isContain = true;
                }

                if (isContain)
                    continue;
                else
                    positions.Add(position);
            }
        }

        return positions;
    }

    public void Move(UnitPosition destination) // 거인 & 1x1 통합본
    {
        BeforeMove();

        //기존 타일 유닛 초기화 Tile.cs로 옮기면 좋을듯
        BattleManager.instance.AllocateUnitTiles(null,unitPosition);

        //unitPosition = destination;
        unitPosition.Set(destination); // 얕은복사가 아닌 깊은복사로 오류 해결

        //새로 이동한 타일에 유닛 할당
        BattleManager.instance.AllocateUnitTiles(this, unitPosition);

        //화면상 위치 갱신.
        Vector2 screenPosition = unitPosition.lowerLeft + (Vector2)(unitPosition.upperRight - unitPosition.lowerLeft) / 2;
        transform.localPosition = screenPosition;

        //HP바 위치 갱신.
        BattleUI.instance.SetHPBarPosition(this);

        AfterMove();
    }

    public void GetDamage(int number)
    {
        currentHP -= number;

        // if (currentHP < 0)
        // Die();
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

