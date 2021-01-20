using System.Collections;
using UnityEngine;


public class Skill_021 : Skill
{
    private int strengthToDamageRatio;

    private Skill_021()
    {
        number = 21;
        name = "비장의 한발";
        unitClass = UnitClass.Archer;
        grade = Grade.Legend;
        description = "지정 대상에게 큰 데미지를 입힌다.";
        criticalRate = 50;
        reuseTime = 5;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "5;00100;00100;11111;00100;00100";
        RPSchema = "1;1";
        strengthToDamageRatio = 4;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio);
        base.UseSkillToUnit(unit);
    }
}
