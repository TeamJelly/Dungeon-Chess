using System.Collections;
using UnityEngine;
using Common.DB;

namespace Model.Skills
{
    public class DummySkill : Skill
    {
        private int strengthToDamageRatio;
        private int upgradePerEnhancedLevel;

        public DummySkill()
        {
            Debug.Log(Query.Instance.SelectFrom<DummySkill>("skill_table"));
            //number = 0;
            //name = "베기";
            //unitClass = UnitClass.Warrior;
            //grade = Grade.Normal;
            //skillImagePath = "HandMade/SkillImage/000_베기";
            //description = "한칸 안에 있는 단일 적에게 데미지를 입힌다.";
            //criticalRate = 5;
            //reuseTime = 0;
            //domain = Domain.SelectOne;
            //target = Target.AnyUnit;
            //APSchema = "3;010;101;010";
            //RPSchema = "1;1";
            //strengthToDamageRatio = 1;
        }

        public override void UseSkillToUnit(Unit owner, Unit unit)
        {
            Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
            unit.GetDamage(owner.strength * strengthToDamageRatio + enhancedLevel * 2);
            base.UseSkillToUnit(owner, unit);
        }
    }
}