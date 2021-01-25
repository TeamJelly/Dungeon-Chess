using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 강타(Colored_555)
/// </summary>
public class Skill_004 : Skill
{
    Skill_004()
    {
        number = 4;
        name = "강타";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/004_강타";
        description = "한칸 안에 있는 단일 적에게 데미지를 입힌고, 기절 상태이상을 건다.";
        criticalRate = 0;
        reuseTime = 3;
        domain = Domain.SelectOne;
        target = Target.EnemyUnit;
        APSchema = "3;010;111;010";
        RPSchema = "1;1";
    }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(10 + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
