using UnityEngine;

namespace Model.Skills
{
    public class DummySkill : Skill
    {
        public DummySkill()
        {
            InitializeSkillFromDB<Skill>();
        }

        public override void UseSkillToUnit(Unit owner, Unit unit)
        {
            Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
            //unit.GetDamage(owner.strength * extension.strengthToDamageRatio + enhancedLevel * extension.upgradePerEnhancedLevel);
            base.UseSkillToUnit(owner, unit);
        }
    }
}