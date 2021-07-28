using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class P001_Priest : Unit
    {
        public P001_Priest()
        {
            Name = "아자젤";
            Alliance = UnitAlliance.Party;
            // Class = UnitClass.Priest;

            Level = 1;
            NextEXP = 10;
            MaxHP = 35;
            CurHP = MaxHP;

            Strength = 13;
            Agility = 10;
            Move = 3;
            CriticalRate = 5;

            // spritePath = "Helltaker/Azazel/Azazel_portrait";
            animatorPath = "Helltaker/Azazel/Azazel_animator";

            MoveSkill = new S100_Walk();

            Skills[0] = new S001_Snapshot();
            Skills[1] = new S002_MagicArrow();
        }
    }
}
