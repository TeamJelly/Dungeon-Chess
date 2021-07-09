using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_Skeleton : Unit
    {
        public Proto_Skeleton()
        {
            Name = "스켈레톤";
            Alliance = UnitAlliance.Enemy;
            Class = UnitClass.Monster;

            Level = 1;
            MaxHP = 15;
            CurHP = MaxHP;

            Strength = 3;
            Agility = 10;
            Move = 4;
            CriticalRate = 5;

            portraitPath = "Helltaker/Skeleton/Skeleton_portrait";
            animatorPath = "Helltaker/Skeleton/Skeleton_animator";

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