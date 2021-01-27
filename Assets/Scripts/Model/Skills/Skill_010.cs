using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_010 : Skill
{
     Skill_010()
     {
        number = 10;
        name = "광역 도발";
        unitClass = UnitClass.Warrior;
        grade = Grade.Rare;
        description = "방 안에 있는 모든 적군에게 도발 효과를 주고 보호막을 2턴 얻는다.";
        criticalRate = 0;
        reuseTime = 6;
        domain = Domain.Fixed;
        target = Target.EnemyUnit;
        APSchema = "1;1";
        RPSchema = "1;1";
     }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");

        //도발 효과, 보호막 2턱
        // unit.GetDamage(GetComponent<Unit>().strength * damage + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
