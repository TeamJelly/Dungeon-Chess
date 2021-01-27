using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 철인
/// </summary>
public class Skill_006 : Skill
{
    Skill_006()
    {
        number = 6;
        name = "철인";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/006_철인";
        description = "자신에게 보호막을 부여한다.";
        criticalRate = 0;
        reuseTime = 2;
        domain = Domain.Me;
        target = Target.PartyUnit;
        APSchema = "1;1";
        RPSchema = "1;1";
    }
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        
       //자신에게 보호막 효과 부여 필요
       //강화시 보호막 수치 + 1
       //기본수치 = 15
        base.UseSkillToUnit(unit);
    }
}
