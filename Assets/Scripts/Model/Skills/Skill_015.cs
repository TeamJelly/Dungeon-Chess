using Model;
using System.Collections;
using UnityEngine;
public class Skill_015 : Skill
{
    // bless 수치가 없음.

    // Use this for initialization
    private Skill_015()
    {
        number = 15;
        name = "철수";
        unitClass = UnitClass.Archer;
        grade = Grade.Normal;
        description = "지정한 위치로 이동하고 축복 버프를 얻는다.";
        criticalRate = 20;
        reuseTime = 4;
        APSchema = "5;00100;01110;11111;01110;00100";
        RPSchema = "1;1";
    }

    /*
        public override void Use(Unit user, UnitPosition target)
        {
            Common.UnitAction.Move(user, target);
            Common.UnitAction.GetEffect(user, )
            // 유닛 이동
            base.UseSkillToTile(tile);
        }*/
}