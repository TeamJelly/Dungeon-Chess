using System.Collections;
using UnityEngine;
using Model;
public class Skill_013 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    public Skill_013()
    {
        number = 13;
        name = "독화살";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        description = "특정 범위에 있는 단일 적에게 데미지를 입히고, 독 상태이상을 부여한다.";
        criticalRate = 5;
        reuseTime = 4;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "7;0001000;0001000;0011100;1111111;0011100;0001000;0001000";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    public override void UseSkillToUnit(Unit owner, Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(owner.strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(owner,unit);
    }
}