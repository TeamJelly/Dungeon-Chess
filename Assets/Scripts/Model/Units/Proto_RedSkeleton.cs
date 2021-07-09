using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_RedSkeleton : Unit
    {
        public Proto_RedSkeleton()
        {
              Name = "빨간 스켈레톤";
            Alliance = UnitAlliance.Enemy;
            Class = UnitClass.Monster;

            Level = 1;
            MaxHP = 10;
            CurHP = MaxHP;

            Strength = 30;
            Agility = 10;
            Move = 4;
            CriticalRate = 5;

            portraitPath = "Helltaker/RedSkeleton/RedSkeleton_portrait";
            animatorPath = "Helltaker/RedSkeleton/RedSkeleton_animator";

            MoveSkill = new S100_Walk()
            {
                Priority = Common.AI.Priority.NearFromClosestParty,
            };

            Skills[0] = new S000_Cut()
            {
                Target = Skill.TargetType.Party,
            };

            Skills[1] = new S004_Bang()
            {
                Target = Skill.TargetType.Party,
            };
        }
    }
}