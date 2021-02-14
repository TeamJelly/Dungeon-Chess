using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Unit_000 : Unit
    {
        public Unit_000()
        {
            Name = "기본 전사";
            UnitClass = UnitClass.Warrior;
            UnitAI = UnitAI.NULL;
            spritePath = "HandMade/women";
            Level = 1;
            CurrentHP = 2;
            MaximumHP = 20;
            CurrentEXP = 0;
            NextEXP = 10;
            Strength = 3;
            Agility = 10;
            Move = 2;
            MoveSkill = new Walk();
            Skills[0] = new Skill_000();
            Skills[0].enhancedLevel = 1;
        }
    }
}