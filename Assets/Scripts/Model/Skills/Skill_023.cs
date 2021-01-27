using System.Collections;
using UnityEngine;
using Model;

public class Skill_023 : Skill
{
    private int strengthToDamageRatio;

    private Skill_023()
    {
        number = 23;
        name = "화염구";
        unitClass = UnitClass.Wizard;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/023_화염구";
        description = "범위 안에 있는 모든 유닛에게 데미지를 입힌다.";
        criticalRate = 0;
        reuseTime = 1;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "5;10101;00000;10101;00000;10101";
        RPSchema = "3;111;111;111";
        strengthToDamageRatio = 1;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel * 2);
        base.UseSkillToUnit(unit);
    }
}
