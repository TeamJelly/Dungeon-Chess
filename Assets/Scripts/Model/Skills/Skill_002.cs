using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 마법 화살(Colored_280)
/// </summary>
public class Skill_002 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;

    Skill_002()
    {
        number = 2;
        name = "마법 화살";
        unitClass = UnitClass.Wizard;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/002_마법화살";
        description = "세칸 안에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 0;
        reuseTime = 0;
        APSchema = "7;0001000;0011100;0111110;1111111;0111110;0011100;0001000";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    public override void Use(Unit user, Tile target)
    {
        Unit unit = target.GetUnit();
        int damage = user.strength * strengthToDamageRatio + enhancedLevel;
        Common.UnitAction.Damage(unit, damage);
        base.Use(user, target);
    }
}
