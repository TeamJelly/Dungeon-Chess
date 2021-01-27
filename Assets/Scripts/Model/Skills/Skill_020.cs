using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_020 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
    Skill_020()
    {
        number = 20;
        name = "스프레드 샷";
        unitClass = UnitClass.Archer;
        grade = Grade.Rare;
        description = "범위 안에 있는 모든 유닛에게 데미지를 나누어 입힌다.";
        criticalRate = 10;
        reuseTime = 3;
        domain = Domain.Fixed;
         target = Target.AnyUnit;
         APSchema = "3;010;111;010";
         RPSchema = "1;1";
        strengthToDamageRatio = 3;
    }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
