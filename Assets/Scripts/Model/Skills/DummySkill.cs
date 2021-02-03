using UnityEngine;

namespace Model.Skills
{
    public class DummySkill : Skill
    {
        [System.Serializable]
        private class Extension_000
        {
            public int strengthToDamageRatio;
            public int upgradePerEnhancedLevel;
        }

        private Extension_000 ext_000;

        public DummySkill()
        {
            InitializeSkillFromDB(0);
            ext_000 = ParseExtension<Extension_000>(extension);
        }

        public override void UseSkillToUnit(Unit owner, Unit unit)
        {
            Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
            unit.GetDamage(owner.strength * ext_000.strengthToDamageRatio + enhancedLevel * ext_000.upgradePerEnhancedLevel);
            base.UseSkillToUnit(owner, unit);
        }
    }
}