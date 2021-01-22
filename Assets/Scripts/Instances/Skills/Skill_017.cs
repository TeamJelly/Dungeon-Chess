using System.Collections;
using UnityEngine;

public class Skill_017 : Skill
{
    private float maxDamageUp;
    private float minDamageUp;
    private float maxDamage;
    private float minDamage;

    // Use this for initialization
    private Skill_017()
    {
        number = 17;
        name = "럭키샷";
        unitClass = UnitClass.Archer;
        grade = Grade.Rare;
        description = "지정한 대상에게 데미지를 준다";
        criticalRate = 20;
        reuseTime = 1;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "5;00100;01110;11111;01110;00100";
        RPSchema = "1;1";
        maxDamageUp = 10;
        minDamageUp = 5;
        maxDamage = 50;
        minDamage = 10;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        float damage = Random.Range(minDamage + minDamageUp * enhancedLevel, maxDamage + maxDamageUp * enhancedLevel);
        unit.GetDamage((int)damage);
        base.UseSkillToUnit(unit);
    }
}