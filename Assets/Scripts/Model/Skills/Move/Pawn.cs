using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class Pawn : Skill
    {
        public Pawn()
        {
            Name = "Pawn's Move";
            Category = SkillCategory.Move;
            Priority = Common.AI.Priority.NULL;
            Target = TargetType.NoUnit;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_1049");
            Color = Color.white;
            Description = "폰의 움직임으로 이동한다.";

            ReuseTime = new int[4] { 0, 0, 0, 0 };

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);
        }

        public override bool IsUsable(Unit user)
        {
            if (GetAvailablePositions(user).Count == 0)
                return false;
            if (!user.IsSkilled && !user.IsMoved && !user.WaitingSkills.ContainsKey(this))
                return true;
            else
                return false;
        }

        public override List<Vector2Int> GetRelatePositions(Unit user, Vector2Int target)
        {
            if (GetAvailablePositions(user).Contains(target))
                return Common.PathFind.PathFindAlgorithm(user, user.Position, target);
            else
                return null;
        }

        public override List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();        // 이동가능한 모든 위치를 저장
            List<Vector2Int> new_frontier = new List<Vector2Int>();     // 새로 추가한 외곽 위치를 저장
            List<Vector2Int> old_frontier = new List<Vector2Int>();     // 이전번에 추가한 외곽 위치를 저장
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            old_frontier.Add(userPosition);

            for (int i = 0; i < user.Mobility; i++)
            {
                foreach (var position in old_frontier)
                {
                    // 4방위를 탐색
                    foreach (var direction in directions)
                    {
                        Vector2Int temp = position + direction;

                        if (
                            // 전에 추가한 위치가 아니고
                            !positions.Contains(temp) &&
                            // 맵 범위 안이고
                            FieldManager.IsInField(temp) &&
                            // 타일에 이 유닛이 위치할수 있으면
                            FieldManager.GetTile(temp).IsPositionable(user)
                            )
                        {
                            // 이동가능한 위치로 추가한다.
                            new_frontier.Add(temp);                
                            positions.Add(temp);
                        }
                    }
                }

                // old와 new를 스왑한다.
                old_frontier.Clear();
                old_frontier.AddRange(new_frontier);

                // new는 초기화 시킨다.
                new_frontier.Clear();
            }

            return positions;
        }

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 필요 변수 계산
            int SLV = GetSLV(user);

            // 스킬 소모 기록
            user.IsMoved = true;
            user.WaitingSkills.Add(this, ReuseTime[SLV]);

            Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");

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
                yield return new WaitForSeconds(moveTime);
            }
            user.animationState = Unit.AnimationState.Idle;

            // 실제 타일에 상속되는건 한번이다.
            Common.Command.Move(user,startPosition, target);
        }
    }
}

