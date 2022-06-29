// using UnityEditor;
// using UnityEngine;
// using Model;
// using Model.Skills;

// namespace Model.Units
// {
//     public class P000_Warrior : Unit
//     {
//         public P000_Warrior()
//         {
//             Name = "Lucifer";
//             Alliance = UnitAlliance.Party;
//             Species = UnitSpecies.Human;
//             Modifier = UnitModifier.Meticulous;

//             Level = 1;
//             NextEXP = 10;
//             MaxHP = 60;
//             CurHP = 60;

//             Strength = 10;
//             Agility = 11;
//             Mobility = 2;
//             CriticalRate = 10;

//             Sprite = Common.Data.GetRandomSprite(Species, Random.Range(0,10000));

//             Skills[SkillCategory.Move] = new Skills.Move.Pawn();
//             Skills[SkillCategory.Basic] = new S000_Cut();
//             Skills[SkillCategory.Intermediate] = new S005_SpinSlash();
//         }
//     }
// }