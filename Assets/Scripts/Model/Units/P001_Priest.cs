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
            Name = "Azazel";
            Alliance = UnitAlliance.Party;
            Species = UnitSpecies.Human;
            Modifier = UnitModifier.Deadly;

            Level = 1;
            NextEXP = 10;
            MaxHP = 35;
            CurHP = MaxHP;

            Strength = 13;
            Agility = 10;
            Mobility = 3;
            CriticalRate = 5;

            Sprite = Common.Data.GetRandomSprite(Species, Random.Range(0,10000));

            Skills[SkillCategory.Move] = new Skills.Move.Pawn();
            Skills[SkillCategory.Basic] = new S001_Snapshot();
            Skills[SkillCategory.Intermediate] = new S002_MagicArrow();
        }
    }
}
