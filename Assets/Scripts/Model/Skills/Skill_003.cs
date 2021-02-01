using System.Collections;
using UnityEngine;
using Model;
public class Skill_003 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    public Skill_003()
    {
        number = 3;
        name = "천벌";
        unitClass = UnitClass.Priest;
        grade = Grade.Normal;
        spritePath = "HandMade/SkillImage/003_천벌";
        description = "두칸 안에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 0;
        reuseTime = 0;
        APSchema = "5;00100;01110;11111;01110;00100";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    //public override void Use(Unit user, Vector2Int target)
    //{
    //    Unit unit = Model.Managers.BattleManager.GetUnit(target);
    //    int damage = user.strength * strengthToDamageRatio + enhancedLevel;
    //    Common.UnitAction.Damage(unit, damage);
    //    base.Use(user, target);
    //}
}