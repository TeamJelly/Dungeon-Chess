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
            UnitAI = UnitAI.NULL;
            spritePath = "HandMade/slime";
            Level = 1;
            CurrentHP = 20;
            MaximumHP = 20;
            CurrentEXP = 0;
            NextEXP = 10;
            Agility = 10;
            Move = 2;
            MoveSkill = new Walk();
        }
    }
}