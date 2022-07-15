// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Model.Managers;

// namespace Model.Skills.Move
// {
//     public class MoveSkill : Skill
//     {
//         // public MoveSkill() : base()
//         // {
//         //     Category = SkillCategory.Move;
//         //     UserTarget = TargetType.Posable;
//         //     AITarget = TargetType.Posable;
//         //     // Priority = Common.Intelligence.Priority.NearFromPartys;

//         //     Range = RangeType.Fixed;
//         //     ReuseTime = 0;
//         // }

//         public override bool IsUsable()
//         {
//             if (!User.IsSkilled && !User.IsMoved && WaitingTime == 0)
//                 return true;
//             else
//                 return false;
//         }

//         public override List<Vector2Int> GetRelatePositions(Vector2Int target)
//         {
//             if (GetAvlUsePositions().Contains(target))
//                 return Common.PathFind.PathFindAlgorithm(User, User.Position, target);
//             else
//                 return new List<Vector2Int>();
//         }

//         public override IEnumerator Use(Vector2Int target)
//         {
//             // 스킬 소모 기록
//             User.IsMoved = true;
//             WaitingTime = ReuseTime;

//             if (target == User.Position)
//                 yield break;

//             Vector2Int startPosition = User.Position;
//             // 1 단계 : 위치 이동
//             List<Vector2Int> path = Common.PathFind.PathFindAlgorithm(User, User.Position, target);

//             // 갈수 있는 길이 없다면??
//             if (path == null)
//             {
//                 Debug.LogError($"{User.Position}에서 {target}으로 길이 존재하지 않습니다.");
//                 yield break;
//             }
//             float moveTime = GameManager.AnimationDelaySpeed / 5;

//             for (int i = 1; i < path.Count; i++)
//             {
//                 // 유닛 포지션의 변경은 여러번 일어난다.
//                 if (User.Alliance != UnitAlliance.Party)
//                     ScreenTouchManager.instance.CameraMove(path[i]);
//                 User.Position = path[i];

//                 AnimationManager.ReserveAnimationClips("Dust", path[i - 1]);
//                 yield return new WaitForSeconds(moveTime);
//             }

//             Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 우선으로 {target}에 사용!");
//             // 실제 타일에 상속되는건 한번이다.
//             Common.Command.Move(User, startPosition, target);
//         }
//     }
// }

