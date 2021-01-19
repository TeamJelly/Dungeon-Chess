using System.Collections;
using UnityEngine;

public class Skill_009 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    private Skill_009()
    {
        number = 9;
        name = "절명";
        unitClass = UnitClass.Warrior;
        grade = Grade.Rare;
        description = "범위 안에 자신을 제외한 유닛에게 데미지를 준다, 만약 그 유닛이 죽었을 경우, 이 스킬을 다시 사용한다.";
        criticalRate = 10;
        reuseTime = 5;
        domain = Domain.Fixed;
        target = Target.AnyUnit;
        APSchema = "5;11111;11111;11111;11111;11111";
        RPSchema = "1;1";
        strengthToDamageRatio = 2;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage((int)(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel * 0.2));
        base.UseSkillToUnit(unit);
    }
}