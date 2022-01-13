using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class MoveSkill : Skill
    {
        public MoveSkill() : base()
        {
            Category = SkillCategory.Move;
            Target = TargetType.Posable;
            AITarget = TargetType.Posable;
            Priority = Common.AI.Priority.NearFromPartys;

            Range = RangeType.Fixed;
            ReuseTime = 0;
        }

        public override bool IsUsable()
        {
            if (!User.IsSkilled && !User.IsMoved && WaitingTime == 0)
                return true;
            else
                return false;
        }

        public override List<Vector2Int> GetRelatePositions(Vector2Int target)
        {
            if (GetAvlUsePositions().Contains(target))
                return Common.PathFind.PathFindAlgorithm(User, User.Position, target);
            else
                return new List<Vector2Int>();
        }

        public override IEnumerator Use(Vector2Int target)
        {
            // 스킬 소모 기록
            User.IsMoved = true;
            WaitingTime = ReuseTime;

            Debug.Log($"{User.Name}가 {Name}스킬을 {Priority}우선으로 {target}에 사용!");

            if (target == User.Position)
                yield break;

            Vector2Int startPosition = User.Position;
            // 1 단계 : 위치 이동
            List<Vector2Int> path = Common.PathFind.PathFindAlgorithm(User, User.Position, target);

            User.animationState = Unit.AnimationState.Move;
            float moveTime = 0.5f / path.Count;

            for (int i = 1; i < path.Count; i++)
            {
                View.VisualEffectView.MakeVisualEffect(User.Position, "Dust");
                // 유닛 포지션의 변경은 여러번 일어난다.
                User.Position = path[i];
                ScreenTouchManager.instance.Move(path[i]);
                yield return new WaitForSeconds(moveTime);
            }
            User.animationState = Unit.AnimationState.Idle;

            // 실제 타일에 상속되는건 한번이다.
            Common.Command.Move(User, startPosition, target);
        }
    }
}

