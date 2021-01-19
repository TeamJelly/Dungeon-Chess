using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_008 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
    Skill_008()
    {
        number = 8;
        name = "힘의 파동";
        unitClass = UnitClass.Warrior;
        grade = Grade.Rare;
        description = "범위 안에 있는 모든 적군에게 데미지를 입히고 기절 시킨다.";
        criticalRate = 0;
        reuseTime = 5;
        domain = Domain.Fixed;
        target = Target.EnemyUnit;
        APSchema = "3;010;111;010";
        RPSchema = "1:0";//회전 들어감. 추후 수정.
        strengthToDamageRatio = 2;
     }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
