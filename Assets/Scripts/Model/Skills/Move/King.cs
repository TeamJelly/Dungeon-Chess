using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class King : MoveSkill
    {
        public King() : base()
        {
            Name = "King's Move";

            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_1053");
            Color = Color.white;
            Description = "킹의 움직임으로 이동한다.";

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);
        }

        public override List<Vector2Int> GetUsePositions(Unit user, Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>() { userPosition };        // 이동가능한 모든 위치를 저장
            List<Vector2Int> new_frontier = new List<Vector2Int>();     // 새로 추가한 외곽 위치를 저장
            List<Vector2Int> old_frontier = new List<Vector2Int>();     // 이전번에 추가한 외곽 위치를 저장
            Vector2Int[] directions = { 
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.up + Vector2Int.right,
                Vector2Int.up + Vector2Int.left, Vector2Int.down + Vector2Int.right, Vector2Int.down + Vector2Int.left
            };

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
                            !positions.Contains(temp)
                            // 맵 범위 안이고
                            && FieldManager.IsInField(temp)
                            // 타일에 이 유닛이 위치할수 있으면
                            && FieldManager.GetTile(temp).IsPositionable(user)
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
    }
}