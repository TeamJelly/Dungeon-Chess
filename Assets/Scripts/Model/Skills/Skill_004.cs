using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 강타(Colored_555)
/// </summary>
public class Skill_004 : Skill
{
    Skill_004()
    {
        number = 4;
        name = "강타";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/004_강타";
        description = "한칸 안에 있는 단일 적에게 데미지를 입힌고, 기절 상태이상을 건다.";
        criticalRate = 0;
        reuseTime = 3;
        APSchema = "3;010;111;010";
        RPSchema = "1;1";
    }

    public override void Use(Unit user, Tile target)
    {
        Unit unit = target.GetUnit();
        int damage = 10 + enhancedLevel;
        Common.UnitAction.Damage(unit, damage);
        base.Use(user, target);
    }
}
