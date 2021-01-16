using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Grade { NULL, Normal, Rare, Legend, Boss }

[System.Serializable]
public class Skill : MonoBehaviour
{
    public int number;                                              // 스킬 번호
    public new string name = "No Skill Name";                       // 스킬 이름
    public UnitClass unitClass = UnitClass.NULL;                    // 스킬 클래스
    public Grade grade = Grade.NULL;                                // 스킬 등급
    public Sprite skillImage; 

    [TextArea(1, 10)]
    public string description;                                      // 스킬 설명
    public int enhancedLevel;                                       // 강화도
    public int reuseTime;                                           // 재사용 대기시간
    public int currentReuseTime;

    [Range(0, 100)]
    public float criticalRate;                                      // 크리티컬율

    public enum Domain                                              // 스킬 사용가능 범위의 종류
    {
        NULL,
        Me,             // 나에게 사용, Unit
        RandomOne,      // 범위 내 대상에서 랜덤한 하나를 뽑는다.
        SelectOne,      // 범위 내 대상중에서 하나를 선택한다.
        Fixed,          // 복수 대상일 경우 범위가 고정된다.
        Rotate,         // 복수 대상일 경우 범위가 회전한다.
    }

    public enum Target                                              // 스킬의 대상
    {
        NULL,
        AnyTile,            // 타일을 대상으로 사용가능
        NoUnitTile,         // 유닛이 없는 곳에만 사용가능, 소환류 스킬에 사용
        AnyUnit,            // 적, 아군 구별 없이 사용가능
        PartyUnit,          // 내 파티에게만 사용가능
        FriendlyUnit,       // 아군에게만 사용가능
        EnemyUnit,          // 적에게만 사용가능
    }

    public Domain domain = Domain.NULL;
    public Target target = Target.NULL;

    private List<Vector2Int> AvailablePositions = new List<Vector2Int>(); // 사용가능한 위치
    private List<Vector2Int> RangePositions = new List<Vector2Int>(); // 다중 범위
    public string APSchema;
    public string RPSchema;

    private void Awake()
    {
        // APSchema 파싱
        Common.Range.ParseRangeSchema(APSchema, AvailablePositions);

        // PRSchema 파싱
        Common.Range.ParseRangeSchema(APSchema, RangePositions);
    }

    public virtual List<Vector2Int> GetAvailablePositions()
    {
        return AvailablePositions;
    }

    public virtual List<Vector2Int> GetRangePositions()
    {
        // 스킬 레벨에 따라 달라지는 다중 범위면 변경 필요.
        return RangePositions;
    }

    public virtual void EnhanceSkill(int level) // 강화할때마다 호출해서 스팩을 업데이트한다.
    {
        enhancedLevel = level;
    }

    public virtual void UseSkillToTile(Vector2Int position)
    {
        currentReuseTime = reuseTime;
    }

    public virtual void UseSkillToTile(List<Vector2Int> positions)
    {
        foreach (var position in positions)
            UseSkillToTile(position);
    }

    public virtual void UseSkillToUnit(Unit unit)
    {
        // 스킬당 구현 필요
        currentReuseTime = reuseTime;
    }

    public virtual void UseSkillToUnit(List<Unit> units)
    {
        foreach (var unit in units)
            UseSkillToUnit(unit);
    }

    public List<Unit> GetUnitsInDomain(Unit skillUser) //
    {
        List<Unit> units = new List<Unit>();

        if (domain == Domain.Me)
            units.Add(skillUser);
        else if (domain == Domain.SelectOne || domain == Domain.RandomOne)
            units = GetUnitsInSelectOne(skillUser);

        return units;
    }

    public List<Unit> GetUnitsInSelectOne(Unit skillUser) 
    {
        List<Unit> units = new List<Unit>();
        List<Unit> tempUnits = BattleManager.GetUnitsInPositions(GetPositionsInDomain(skillUser)); // 스킬 도메인을 받고 그위의 유닛들을 반환

        foreach (var unit in tempUnits)
        {
            if (target == Target.AnyUnit)
                units.Add(unit);
            else if (target == Target.EnemyUnit && unit.category == Category.Enemy)
                units.Add(unit);
            else if (target == Target.FriendlyUnit && (unit.category == Category.Friendly || unit.category == Category.Party))
                units.Add(unit);
            else if (target == Target.PartyUnit && unit.category == Category.Party)
                units.Add(unit);
        }

        return units;
    }

    public List<Vector2Int> GetPositionsInDomain(Unit skillUser) // 스킬 범위를 절대 위치로 바꾼다.
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (var item in AvailablePositions)
        {
            List<Vector2Int> temp = UnitPosition.VectoredPosition(skillUser.unitPosition, item).UnitPositionToVector2IntList();

            foreach (var position in temp)
            {
                if (position.x < 0 || position.y < 0) // 범위 제한
                    continue;
                if (position.x > BattleManager.instance.AllTiles.GetLength(1) || position.y > BattleManager.instance.AllTiles.GetLength(0)) // 범위 제한
                    continue;
                if (target == Target.NoUnitTile && !BattleManager.instance.AllTiles[position.x, position.y].IsUsable())
                    continue;
                positions.Add(position);
            }

        }
        return positions;
    }

    public bool IsUsable()
    {
        if (currentReuseTime == 0)
            return true;
        else
            return false;
    }

}