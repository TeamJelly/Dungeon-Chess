using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_001 : Skill
{
    Skill_001()
    {
        number = 1;
        name = "베기";
        unitClass = UnitClass.Warrior;

        grade = Grade.NULL;

        description = "한칸 안에 있는 단일 적에게 데미지를 입힌다.";
        enhancedLevel = 0;
        reuseTime = 0;

        criticalRate = 5;

/*        AvailablePositions = new List<Vector2Int>();
        AvailablePositions.Add(new Vector2Int(1, 0));
        AvailablePositions.Add(new Vector2Int(0, 0));
        AvailablePositions.Add(new Vector2Int(2, 0));
        AvailablePositions.Add(new Vector2Int(3, 0));
        AvailablePositions.Add(new Vector2Int(4, 0));
        AvailablePositions.Add(new Vector2Int(5, 0));
        RangePositions = new List<Vector2Int>();
        RangePositions.Add(new Vector2Int(1, 1));
        RangePositions.Add(new Vector2Int(2, 1));*/
    }

    public new void UseSkillToUnit(Unit unit)
    {
        unit.currentHP -= 10;
        base.UseSkillToUnit(unit);
    }
}
