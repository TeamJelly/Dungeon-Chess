using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class Knight : Skill
    {
        public Knight()
        {
            Name = "Knight's Move";
            Category = SkillCategory.Move;
            Priority = Common.AI.Priority.NULL;
            Target = TargetType.NoUnit;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_1054");
            Color = Color.white;
            Description = "나이트의 움직임으로 이동한다.";

            ReuseTime = new int[1] { 0 };

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);
        }


        public override bool IsUsable(Unit user)
        {
            if (!user.IsSkilled && !user.IsMoved && !user.WaitingSkills.ContainsKey(this))
                return true;
            else
                return false;
        }

        public override List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>(){userPosition};
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            bool[] canGo = { true, true, true, true };

            for (int i = 1; i <= user.Mobility; i++)
            {
                for (int b = 0; b < directions.GetLength(0); b++)
                {
                    Vector2Int temp;
                    temp = userPosition + directions[b] * (2 * i - 1);
                    if (canGo[b] && FieldManager.IsInField(temp) && FieldManager.GetTile(temp).IsPositionable(user))
                        positions.Add(temp);
                    else
                        canGo[b] = false;

                    temp = userPosition + directions[b] * 2 * i;
                    if (canGo[b] && FieldManager.IsInField(temp) && FieldManager.GetTile(temp).IsPositionable(user))
                        positions.Add(temp);
                    else
                        canGo[b] = false;
                }               
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