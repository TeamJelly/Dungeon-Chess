using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_016 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
    Skill_016()
    {
        number = 16;
        name = "화살비";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        spritePath = "HandMade/SkillImage/016_화살비";
        description = "범위 안에 있는 모든 유닛에게 데미지를 준다.";
        criticalRate = 5;
        reuseTime = 1;
        APSchema = "5;00100;01110;11111;01110;00100";
        RPSchema = "3;111;101;111";
        strengthToDamageRatio = 1;
    }
    /*public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel * 2);
        base.UseSkillToUnit(unit);
    }*/
}
