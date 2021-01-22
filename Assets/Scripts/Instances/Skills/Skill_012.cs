using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_012 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
     Skill_012()
     {
        number = 12;
        name = "광전사의 힘";
        unitClass = UnitClass.Warrior;
        grade = Grade.Legend;
        description = "자신에게 광폭화 상태를 2턴 부여하고, 단일 적에게 준 데미지 만큼, 체력을 회복한다.";
        criticalRate = 50;
        reuseTime = 5;
        domain = Domain.SelectOne;
        target = Target.EnemyUnit;
         APSchema = "5;00100;01110;11111;01110;00100";
         RPSchema = "1;1";
        strengthToDamageRatio = 1;
     }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        Unit owner = GetComponent<Unit>();
        int damage = owner.strength * strengthToDamageRatio + enhancedLevel;

        //적에게 광폭화 상태효과 부여 필요
        unit.GetDamage(damage);
        owner.GetHeal(damage);
        base.UseSkillToUnit(unit);
    }
}
