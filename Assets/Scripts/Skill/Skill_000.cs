using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 베기
/// </summary>
public class Skill_000 : Skill
{
    private int strengthToDamageRatio;

    private Skill_000()
    {
        number = 0;
        name = "베기";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        description = "한칸 안에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 5;
        reuseTime = 0;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "3;010;111;010";
        RPSchema = "1;0";
        strengthToDamageRatio = 1;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel * 2);
        base.UseSkillToUnit(unit);
    }
}
