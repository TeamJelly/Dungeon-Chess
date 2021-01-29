using System.Collections;
using UnityEngine;
using Model;
public class Skill_009 : Skill
{
    private int strengthToDamageRatio;

    // Use this for initialization
    private Skill_009()
    {
        number = 9;
        name = "절명";
        unitClass = UnitClass.Warrior;
        grade = Grade.Rare;
        skillImagePath = "HandMade/SkillImage/009_절명";
        description = "범위 안에 자신을 제외한 유닛에게 데미지를 준다, 만약 그 유닛이 죽었을 경우, 이 스킬을 다시 사용한다.";
        criticalRate = 10;
        reuseTime = 5;
        APSchema = "5;11111;11111;11111;11111;11111";
        RPSchema = "1;1";
        strengthToDamageRatio = 2;
    }

    public override void Use(Unit user, Tile target)
    {
        Unit unit = target.GetUnit();
        int damage = Mathf.FloorToInt((float)(user.strength * strengthToDamageRatio + enhancedLevel * 0.2));
        Common.UnitAction.Damage(unit, damage);
        base.Use(user, target);
    }
}