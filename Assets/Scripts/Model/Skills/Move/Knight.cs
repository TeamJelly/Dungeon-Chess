using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class Knight : MoveSkill
    {
        public Knight() : base()
        {
            Name = "Knight's Move";

            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_1054");
            Color = Color.white;
            Description = "나이트의 움직임으로 이동한다.";

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);
        }

        public override List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            for (int i = 1; i <= user.Mobility; i++)
                foreach (var direction1 in directions)
                    foreach (var direction2 in directions)
                    {
                        if (direction1 == direction2 || direction1 == -direction2)
                            continue;

                        Vector2Int temp = userPosition + direction1 * (i+1) + direction2 * i;
                        if (FieldManager.IsInField(temp) && FieldManager.GetTile(temp).IsPositionable(user))
                            positions.Add(temp);
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

            Vector2Int startPosition = user.Position;

            View.VisualEffectView.MakeVisualEffect(user.Position, "Dust");
            Common.Command.Move(user,startPosition, target);

            yield return null;
        }
    }
}