using System.Collections;
using UnityEngine;
using Model;
public class Skill_007 : Skill
{

    // Use this for initialization
    private Skill_007()
    {
        number = 7;
        name = "수호";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        spritePath = "HandMade/SkillImage/007_수호";
        description = "두칸 안에 있는 아군에게 보호 효과를 3턴간 건다,";
        criticalRate = 5;
        reuseTime = 3;
        APSchema = "5;00100;01110;11111;01110;00100";
        RPSchema = "1;0";
    }

    //public override void Use(Unit user, Tile target)
    //{
    //    Unit unit = target.GetUnit();
    //    //Common.UnitAction.GetEffect(unit, new Effect("수호, ))
    //    base.Use(user, target);
    //}
}