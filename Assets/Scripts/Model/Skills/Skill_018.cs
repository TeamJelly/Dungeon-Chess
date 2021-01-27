using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_018 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
    Skill_018()
    {
        number = 18;
        name = "사냥준비";
        unitClass = UnitClass.Archer;
        grade = Grade.Rare;
        description = "지정 대상에게 데미지를 주고 속박을 부여하고, 자신에게 심안 버프를 건다.";
        criticalRate = 0;
        reuseTime = 4;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
         APSchema = "5;00100;00100;11111;00100;00100";
         RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        //적에게 속박, 자신에게 심안버프 필요
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
