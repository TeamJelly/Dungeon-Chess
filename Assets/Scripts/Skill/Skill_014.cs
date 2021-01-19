using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_014 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
    Skill_014()
    {
        number = 14;
        name = "저격";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        description = "특정 범위에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 20;
        reuseTime = 4;
        domain = Domain.SelectOne;
        target = Target.EnemyUnit;
        APSchema = "9;000010000;000010000;000000000;000000000;110010011;000000000;000000000;000010000;000010000";
        RPSchema = "1;1";
        strengthToDamageRatio = 2;
    }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel * 2);
        base.UseSkillToUnit(unit);
    }
}
