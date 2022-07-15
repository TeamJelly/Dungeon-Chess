// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Model.Skills.Basic
// {
//     public class Scratch : Skill
//     {
//         private float strToDam;
//         private int fixedDam;

//         // public Scratch() : this(0) { }

//         // public Scratch(int level) : base()
//         // {
//         //     Name = "Scratch";
//         //     // Priority = Common.Intelligence.Priority.NULL;

//         //     UserTarget = TargetType.Any;
//         //     AITarget = TargetType.Hostile;
//         //     Range = RangeType.Fixed;

//         //     SpriteNumber = 553;
//         //     InColor = Color.red;
//         //     OutColor = Color.clear;

//         //     // species.Add(UnitSpecies.SmallBeast);
//         //     // species.Add(UnitSpecies.MediumBeast);
//         //     // species.Add(UnitSpecies.LargeBeast);

//         //     OnUpgrade(level);
//         // }

//         public override IEnumerator Use(Vector2Int target)
//         {
//             // 필요 변수 계산
//             bool isCri = Random.Range(0, 100) < User.CriRate;
//             int damage = (int)(User.Strength * strToDam);
//             if (isCri) damage *= 2;

//             // 스킬 소모 기록
//             User.IsSkilled = true;
//             WaitingTime = ReuseTime;

//             // 스킬 실행
//             Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
//             if (targetUnit != null)
//             {
//                 Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 우선으로 {targetUnit.Name}에 사용!");
//                 Common.Command.Damage(targetUnit, damage);
//             }
//             else
//                 Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 우선으로 {target}에 사용!");

//             AnimationManager.ReserveAnimationClips("Scratch", target);

//             yield return null;
//         }

//         public override string GetDescription()
//         {
//             int damage = (int)(fixedDam + User.Strength * strToDam);

//             return $"지정한 대상에게 {damage}만큼 데미지를 준다.";
//         }

//         public override void OnUpgrade(int level)
//         {
//             Level = level;

//             if (Level == 0)
//             {
//                 ReuseTime = 1;
//                 AvilablePosition = Common.Data.MakeRangeData(1, 1);
//                 RangePosition = Common.Data.MakeRangeData(1, 0);
//                 strToDam = 1.0f;
//                 fixedDam = 0;
//             }
//             else if (Level == 1)
//             {
//                 ReuseTime = 1;
//                 AvilablePosition = Common.Data.MakeRangeData(1, 1);
//                 RangePosition = Common.Data.MakeRangeData(1, 0);
//                 strToDam = 1.5f;
//                 fixedDam = 0;
//             }
//             else if (Level == 2)
//             {
//                 ReuseTime = 1;
//                 AvilablePosition = Common.Data.MakeRangeData(1, 1);
//                 RangePosition = Common.Data.MakeRangeData(1, 0);
//                 strToDam = 1.5f;
//                 fixedDam = 0;
//             }
//             else if (Level == 3)
//             {
//                 ReuseTime = 1;
//                 AvilablePosition = Common.Data.MakeRangeData(1, 1);
//                 RangePosition = Common.Data.MakeRangeData(1, 0);
//                 strToDam = 2.0f;
//                 fixedDam = 0;
//             }
//         }
//     }
// }