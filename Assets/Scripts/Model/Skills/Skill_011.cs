using System.Collections;
using UnityEngine;
using Model;
using System.Collections.Generic;

public class Skill_011 : Skill
{
    private int fixedDamge;

    private Skill_011()
    {
        number = 11;
        name = "혼신의 일격";
        unitClass = UnitClass.Warrior;
        grade = Grade.Legend;
        description = "단일 적에게 체력 만큼의 데미지를 준다.";
        criticalRate = 0;
        reuseTime = 3;
        APSchema = "5;00100;00100;11111;00100;00100";
        fixedDamge = 20;
    }

    public override List<Vector2Int> GetRangePositions(Unit user)
    {
        if (enhancedLevel == 0)
            RPSchema = "1;1";
        else if (enhancedLevel >= 1)
            RPSchema = "3;111;111;111";

        return base.GetRangePositions(user);
    }

    public override void Use(Unit user, Tile target)
    {
        Unit unit = target.GetUnit();
        int damage = user.maxHP + fixedDamge;
        Common.UnitAction.Damage(unit, damage);
        base.Use(user, target);
    }
}