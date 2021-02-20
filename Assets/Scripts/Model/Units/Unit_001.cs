using Model.Skills;
using UnityEditor;
using UnityEngine;

namespace Model.Units
{
    public class Unit_001 : Unit
    {
        public Unit_001()
        {
            Name = "슬라임";
            UnitClass = UnitClass.Monster;
            Level = 1;
            CurrentHP = 20;
            MaximumHP = 20;
            CurrentEXP = 0;
            NextEXP = 10;
            Strength = 1;
            Agility = 10;
            Move = 2;

            portraitPath = "Helltaker/Skeleton/skeleton_portrait";
            animatorPath = "Helltaker/Skeleton/Skeleton_animator";

            MoveSkill = new Walk();
            MoveSkill.priority = Common.AI.Priority.NearFromClosestParty;
            Skills[0] = new Skill_000();
            Skills[0].target = Skill.Target.Party;
        }
    }
}