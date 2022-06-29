// using Model.Skills;

// namespace Model.Units
// {
//     public class M000_Judgement : Unit
//     {
//         public M000_Judgement()
//         {
//             Name = "저지먼트";
//             Alliance = UnitAlliance.Enemy;

//             Level = 1;
//             MaxHP = 150;
//             CurHP = MaxHP;

//             Strength = 10;
//             Agility = 10;
//             Move = 5;
//             CriticalRate = 15;

//             // spritePath = "Helltaker/Judgement/Judgement_portrait";
//             animatorPath = "Helltaker/Judgement/Judgement_animator";

//             MoveSkill = new S100_Walk()
//             {
//                 Priority = Common.AI.Priority.NearFromClosestParty,
//             };

//             Skills[0] = new S000_Cut()
//             {
//                 Target = Skill.TargetType.Party,
//             };

//             Skills[1] = new S004_Bang()
//             {
//                 Target = Skill.TargetType.Party,
//             };
//         }
//     }
// }
