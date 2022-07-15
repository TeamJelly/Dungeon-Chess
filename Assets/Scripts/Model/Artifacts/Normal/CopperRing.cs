// namespace Model.Artifacts.Normal
// {
//     /// <summary>
//     /// 치명률 증가 소 + 1
//     /// </summary>
//     public class CopperRing : Artifact
//     {
//         protected int increasingValue = 1;
//         public CopperRing()
//         {
//             Name = "Copper Ring";
//             Grade = GradeEnum.Normal;
//             Description = "치명률 +5%";

//             SpriteNumber = 330;
//             InColor = UnityEngine.Color.gray;
//             OutColor = UnityEngine.Color.clear;
//         }

//         public override void OnAdd()
//         {
//             Owner.CriRate += increasingValue;
//         }

//         public override void OnRemove()
//         {
//             Owner.CriRate -= increasingValue;
//         }
//     }
// }