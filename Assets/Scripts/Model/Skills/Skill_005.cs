using System.Collections;
using UnityEngine;
using Model;
public class Skill_005 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    private Skill_005()
    {
        number = 5;
        name = "회전베기";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/005_회전베기";
        description = "범위 안에 있는 모든 유닛에게 데미지를 입힌다.";
        criticalRate = 5;
        reuseTime = 1;
        domain = Domain.Fixed;
        target = Target.AnyUnit;
        APSchema = "1;1";
        RPSchema = "3:111;101;111";
        strengthToDamageRatio = 1;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}