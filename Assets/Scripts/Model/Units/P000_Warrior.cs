using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class P000_Warrior : Unit
    {
        public P000_Warrior()
        {
            Name = "루시퍼";
            Alliance = UnitAlliance.Party;
            Class = UnitClass.Warrior;

            Level = 1;
            NextEXP = 10;
            MaxHP = 60;
            CurHP = 60;

            Strength = 10;
            Agility = 11;
            Move = 2;
            CriticalRate = 10;

            portraitPath = "2179";
            animatorPath = "Helltaker/Lucifer/Lucifer_animator";

            MoveSkill = new S100_Walk();

            Skills[0] = new S000_Cut();
            Skills[1] = new S005_SpinSlash();
        }
    }
}