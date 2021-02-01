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
            //Category = Category.NULL;
            UnitClass = UnitClass.Warrior;
            UnitAI = UnitAI.NULL;
            spritePath = "HandMade/slime";
            Level = 1;
            CurrentHP = 10;
            MaximumHP = 20;
            CurrentEXP = 0;
            NextEXP = 10;
            Agility = 10;
            Move = 2;
        }
    }
}