using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Model;

public class Skill_005 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    private Skill_005()
    {
        number = 5;
        name = "회전베기";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        spritePath = "HandMade/SkillImage/005_회전베기";
        description = "범위 안에 있는 모든 유닛에게 데미지를 입힌다.";
        criticalRate = 5;
        reuseTime = 1;
        APSchema = "1;1";
        RPSchema = "3:111;101;111"; // 이걸로 표현 불가
        strengthToDamageRatio = 1;
    }

    /*public override void Use(Unit user, Tile target)
    {
        Unit unit = target.GetUnit();
        int damage = user.strength * strengthToDamageRatio + enhancedLevel;
        Common.UnitAction.Damage(unit, damage);
        base.Use(user, target);
    }*/
}