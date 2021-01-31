using Model;
using System.Collections;
using UnityEngine;

public class Skill_015 : Skill
{
    // bless 수치가 없음.

    // Use this for initialization
    public Skill_015()
    {
        number = 15;
        name = "철수";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        description = "지정한 위치로 이동하고 축복 버프를 얻는다.";
        criticalRate = 20;
        reuseTime = 4;
        domain = Domain.Me;
        target = Target.NoUnitTile;
        APSchema = "5;00100;01110;11111;01110;00100";
        RPSchema = "1;1";
    }

    public override void UseSkillToTile(Tile tile)
    {
        Debug.LogError(name + " 스킬을 " + tile.name + "에 사용!");
        // 유닛 이동
        base.UseSkillToTile(tile);
    }

    public override void UseSkillToUnit(Unit owner, Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        // 유닛 축복
        base.UseSkillToUnit(owner,unit);
    }
}