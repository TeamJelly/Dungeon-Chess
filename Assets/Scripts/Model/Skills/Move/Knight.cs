using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class Knight : MoveSkill
    {
        public Knight() : this(0) { }
        public Knight(int level) : base()
        {
            Name = "Knight's Move";

            SpriteNumber = 1054;
            InColor = Color.white;
            OutColor = Color.clear;

            Description = "나이트의 움직임으로 이동한다.";

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);

            OnUpgrade(level);
        }

        public override List<Vector2Int> GetRelatePositions(Vector2Int target)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            list.Add(target);

            return list;
        }

        public override List<Vector2Int> GetUseRange(Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>() { userPosition };
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            for (int i = 1; i <= User.Mobility; i++)
                foreach (var direction1 in directions)
                    foreach (var direction2 in directions)
                    {
                        if (direction1 == direction2 || direction1 == -direction2)
                            continue;

                        Vector2Int temp = userPosition + direction1 * (i+1) + direction2 * i;
                        if (FieldManager.IsInField(temp) 
                            //&& FieldManager.GetTile(temp).IsPositionable(user)
                            )
                            positions.Add(temp);
                    }

            return positions;
        }
        public override List<Vector2Int> GetAvlPositions(Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>() { userPosition };
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            for (int i = 1; i <= User.Mobility; i++)
                foreach (var direction1 in directions)
                    foreach (var direction2 in directions)
                    {
                        if (direction1 == direction2 || direction1 == -direction2)
                            continue;

                        Vector2Int temp = userPosition + direction1 * (i+1) + direction2 * i;
                        if (FieldManager.IsInField(temp) 
                            && FieldManager.GetTile(temp).IsPositionable(User))
                            positions.Add(temp);
                    }

            return positions;
        }

        public override IEnumerator Use(Vector2Int target)
        {
            // 스킬 소모 기록
            User.IsMoved = true;
            WaitingTime = ReuseTime;

            Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 {Priority}우선으로 {target}에 사용!");

            Vector2Int startPosition = User.Position;
            View.VisualEffectView.MakeVisualEffect(User.Position, "Dust");
            Common.Command.Move(User,startPosition, target);
            ScreenTouchManager.instance.CameraMove(User.Position);

            yield return null;
        }
    }
}