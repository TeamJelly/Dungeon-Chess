// namespace Model.Artifacts.Normal
// {
//     /// <summary>
//     /// 공격력 증가 소 + 1
//     /// </summary>
//     public class WoodClub : Artifact
//     {
//         protected int increasingValue = 1;
//         public WoodClub()
//         {
//             Name = "Wood Club";
//             Grade = GradeEnum.Normal;
//             Description = "공격력 +1";

//             SpriteNumber = 127;
//             InColor = UnityEngine.Color.gray;
//             OutColor = UnityEngine.Color.clear;
//         }

//         public override void OnAdd()
//         {
//             Owner.Strength += increasingValue;
//         }

//         public override void OnRemove()
//         {
//             Owner.Strength -= increasingValue;
//         }
//     }
// }