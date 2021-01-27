using System.Collections;
using UnityEngine;
using Model.Models;
public class Skill_001 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    private Skill_001()
    {
        number = 1;
        name = "속사";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/001_속사";
        description = "세칸 안에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 10;
        reuseTime = 0;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "7;0001000;0011100;0111110;1111111;0111110;0011100;0001000";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}