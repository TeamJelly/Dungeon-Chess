using UnityEditor;
using UnityEngine;
using Model;

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
            Agility = 10;
            Move = 2;
            MoveSkill = new Walk();
            Skills[0] = new Skill_000();
        }
    }
}