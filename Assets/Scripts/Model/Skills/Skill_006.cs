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
        spritePath = "HandMade/SkillImage/006_철인";
        description = "자신에게 보호막을 부여한다.";
        criticalRate = 0;
        reuseTime = 2;        
        APSchema = "1;1";
        RPSchema = "1;1";
    }
    /*public override void Use(Unit user, Tile target)
    {
        Unit unit = target.GetUnit();
        //Common.UnitAction.GetEffect(unit, new Effect());
        //자신에게 보호막 효과 부여 필요
        //강화시 보호막 수치 + 1
        //기본수치 = 15
        base.Use(user, target);
    }*/

}
