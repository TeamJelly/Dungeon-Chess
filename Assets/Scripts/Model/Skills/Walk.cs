using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;

public class Walk : Skill
{
    public Walk()
    {
        number = 999;
        name = "걷기";
        unitClass = UnitClass.NULL;
        spritePath = null;
        description = "이동한다.";
        reuseTime = 0;
    }

    /// <summary>
    /// 걷기의 이동 가능한 범위를 계산합니다.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public override List<Vector2Int> GetAvailablePositions(Unit user)
    {
        List<Vector2Int> positions = new List<Vector2Int>();        // 이동가능한 모든 위치를 저장
        List<Vector2Int> new_frontier = new List<Vector2Int>();     // 새로 추가한 외곽 위치를 저장
        List<Vector2Int> old_frontier = new List<Vector2Int>();     // 이전번에 추가한 외곽 위치를 저장
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

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
        base.Use(user, target);

        yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
        user.animationState = Unit.AnimationState.Move;

        yield return new WaitWhile(()=> user.animationState != Unit.AnimationState.Idle);
    }
}