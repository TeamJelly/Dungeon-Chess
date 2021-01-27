using System.Collections;
using UnityEngine;
using Model;
public class Skill_011 : Skill
{
    private int fixedDamge;

    // Use this for initialization
    private Skill_011()
    {
        number = 11;
        name = "혼신의 일격";
        unitClass = UnitClass.Warrior;
        grade = Grade.Legend;
        description = "단일 적에게 체력 만큼의 데미지를 준다.";
        criticalRate = 0;
        reuseTime = 3;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "5;00100;00100;11111;00100;00100";
        RPSchema = "1;1"; // 강화시 -> "3;111;111;111"
        fixedDamge = 20;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().maxHP + fixedDamge);
        base.UseSkillToUnit(unit);
    }
}