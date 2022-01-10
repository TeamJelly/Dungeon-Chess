using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class MoveSkill : Skill
    {
        public MoveSkill()
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

        public override List<Vector2Int> GetRelatePositions(Unit user, Vector2Int target)
        {
            if (GetAvlUsePositions(user).Contains(target))
                return Common.PathFind.PathFindAlgorithm(user, user.Position, target);
            else
                return new List<Vector2Int>();
        }

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 필요 변수 계산
            int SLV = GetSLV(user);

            // 스킬 소모 기록
            user.IsMoved = true;
            user.WaitingSkills.Add(this, ReuseTime[SLV]);

            Debug.Log($"{user.Name}가 {Name}스킬을 {Priority}우선으로 {target}에 사용!");

            if (target == user.Position)
                yield break;

            Vector2Int startPosition = user.Position;
            // 1 단계 : 위치 이동
            List<Vector2Int> path = Common.PathFind.PathFindAlgorithm(user, user.Position, target);

            user.animationState = Unit.AnimationState.Move;
            float moveTime = 0.5f / path.Count;

            for (int i = 1; i < path.Count; i++)
            {
                View.VisualEffectView.MakeVisualEffect(user.Position, "Dust");
                // 유닛 포지션의 변경은 여러번 일어난다.
                user.Position = path[i];
                ScreenTouchManager.instance.Move(path[i]);
                yield return new WaitForSeconds(moveTime);
            }
            user.animationState = Unit.AnimationState.Idle;

            // 실제 타일에 상속되는건 한번이다.
            Common.Command.Move(user, startPosition, target);
        }
    }
}

