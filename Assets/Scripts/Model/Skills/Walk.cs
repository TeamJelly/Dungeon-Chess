using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    using Managers;
    public class Walk : Skill
    {
        public Walk() : base(999) {}

        public override bool IsUsable(Unit user)
        {
            if (GetAvailablePositions(user).Count == 0)
                return false;

            if (user.MoveCount > 0 && currentReuseTime == 0)
                return true;
            else
                return false;
        }

    public override List<Vector2Int> GetRelatePositions(Unit user, Vector2Int target)
    {


        if (GetAvailablePositions(user, user.Position).Contains(target))
            return Common.PathFind.PathFindAlgorithm(user.Position, target);
        else
            return null;
    }

    /// <summary>
    /// 걷기의 이동 가능한 범위를 계산합니다.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public override List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
    {
        List<Vector2Int> positions = new List<Vector2Int>();        // 이동가능한 모든 위치를 저장
        List<Vector2Int> new_frontier = new List<Vector2Int>();     // 새로 추가한 외곽 위치를 저장
        List<Vector2Int> old_frontier = new List<Vector2Int>();     // 이전번에 추가한 외곽 위치를 저장
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        old_frontier.Add(userPosition);

            for (int i = 0; i < user.Move; i++)
            {
                foreach (var position in old_frontier)
                {
                    // 4방위를 탐색
                    foreach (var direction in directions)
                    {
                        Vector2Int temp = position + direction;

                        if (!positions.Contains(temp) &&                // 전에 추가한 위치가 아니고
                            BattleManager.IsAvilablePosition(temp) &&   // 맵 범위 안이고
                            BattleManager.GetTile(temp).IsUsable())     // 타일에 유닛이 존재하지 않는다면
                        {
                            new_frontier.Add(temp);                     // 이동가능한 위치로 추가한다.
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
            for (int i = user.StateEffects.Count - 1; i >= 0; i--)
                user.StateEffects[i].BeforeUseSkill(this);

            // 0 단계 : 로그 출력, 스킬 소모 기록
            Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            user.MoveCount--;
            currentReuseTime = reuseTime;

            // 1 단계 : 위치 이동
            {
                List<Vector2Int> path = Common.PathFind.PathFindAlgorithm(user.Position, target);

                user.animationState = Unit.AnimationState.Move;
                float moveTime = 0.5f / path.Count;

                for (int i = 1; i < path.Count; i++)
                {
                    Common.UnitAction.Move(user, path[i]);
                    yield return new WaitForSeconds(moveTime);
                }

                user.animationState = Unit.AnimationState.Idle;
            }

            for (int i = user.StateEffects.Count - 1; i >= 0; i--)
                user.StateEffects[i].BeforeUseSkill(this);
        }
    }
    [System.Serializable]
    public class Extension_Walk : Common.Extensionable
    {
    }
}

