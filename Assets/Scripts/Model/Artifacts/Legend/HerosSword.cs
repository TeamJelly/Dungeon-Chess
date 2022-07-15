// namespace Model.Artifacts.Legend
// {
//     /// <summary>
//     /// 공격력 증가 대 + 3
//     /// </summary>

//     public class HerosSword : Artifact
//     {
//         protected int increasingValue = 2;
//         public HerosSword()
//         {
//             Name = "Heros Sword";
//             Grade = GradeEnum.Legend;
//             Description = "공격력 +3";

//             SpriteNumber = 464;
//             InColor = UnityEngine.Color.cyan;
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