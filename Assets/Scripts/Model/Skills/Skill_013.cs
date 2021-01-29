using System.Collections;
using UnityEngine;
using Model;
public class Skill_013 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    private Skill_013()
    {
        number = 13;
        name = "독화살";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        description = "특정 범위에 있는 단일 적에게 데미지를 입히고, 독 상태이상을 부여한다.";
        criticalRate = 5;
        reuseTime = 4;
        APSchema = "7;0001000;0001000;0011100;1111111;0011100;0001000;0001000";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    public override void Use(Unit user, Tile target)
    {
        Unit targetUnit = target.GetUnit();
        int damage = user.strength * strengthToDamageRatio + enhancedLevel;

        Common.UnitAction.Damage(targetUnit, damage);
        // 독 상태이상 부여
        // Common.UnitAction.GetEffect(targetUnit, );

        base.Use(user, target);
    }
}