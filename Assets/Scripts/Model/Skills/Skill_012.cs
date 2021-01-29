using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_012 : Skill
{
     private int strengthToDamageRatio;
     Skill_012()
     {
        number = 12;
        name = "광전사의 힘";
        unitClass = UnitClass.Warrior;
        grade = Grade.Legend;
        skillImagePath = "HandMade/SkillImage/012_광전사의 힘";
        description = "자신에게 광폭화 상태를 2턴 부여하고, 단일 적에게 준 데미지 만큼, 체력을 회복한다.";
        criticalRate = 50;
        reuseTime = 5;
         APSchema = "5;00100;01110;11111;01110;00100";
         RPSchema = "1;1";
        strengthToDamageRatio = 1;
     }

    public override void Use(Unit user, Tile target)
    {
        Unit targetUnit = target.GetUnit();
        int damage = user.strength * strengthToDamageRatio + enhancedLevel;

        //적에게 광폭화 상태효과 부여 필요

        int damaged = Common.UnitAction.Damage(targetUnit, damage);
        Common.UnitAction.Heal(user, damaged);
        base.Use(user, target);
    }
}
