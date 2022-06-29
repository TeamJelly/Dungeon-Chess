using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;

namespace Model.Skills.Move
{
    public class Bishop : MoveSkill
    {
        public Bishop() : this(0) { }

        public Bishop(int level) : base()
        {
            Name = "Bishop's Move";

            SpriteNumber = 1051;
            InColor = Color.white;
            OutColor = Color.clear;

            Description = "비숍의 움직임으로 이동한다.";

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);

            OnUpgrade(level);
        }

        public override List<Vector2Int> GetUseRange(Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>() { userPosition };
            Vector2Int[] directions = { 
                Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, 
                Vector2Int.down + Vector2Int.right, Vector2Int.down + Vector2Int.left
            };

            bool[] canGo = new bool[directions.Length];
            for (int i = 0; i < canGo.Length; i++)
                canGo[i] = true;

            for (int i = 1; i <= User.Mobility; i++)
            {
                for (int b = 0; b < directions.GetLength(0); b++)
                {
                    Vector2Int temp;
                    temp = userPosition + directions[b] * (2 * i - 1);
                    if (canGo[b] && FieldManager.IsInField(temp) /*&& FieldManager.GetTile(temp).IsPositionable(user)*/)
                        positions.Add(temp);
                    else
                        canGo[b] = false;

                    temp = userPosition + directions[b] * 2 * i;
                    if (canGo[b] && FieldManager.IsInField(temp) /*&& FieldManager.GetTile(temp).IsPositionable(user)*/)
                        positions.Add(temp);
                    else
                        canGo[b] = false;
                }
            }

            foreach(var i in new List<Vector2Int>{ Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                if (FieldManager.IsInField(userPosition + i) && FieldManager.GetTile(userPosition + i).IsPositionable(User))
                    positions.Add(userPosition + i);

            return positions;
        }

        public override List<Vector2Int> GetAvlPositions(Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>() { userPosition };
            Vector2Int[] directions = { 
                Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, 
                Vector2Int.down + Vector2Int.right, Vector2Int.down + Vector2Int.left
            };

            bool[] canGo = new bool[directions.Length];
            for (int i = 0; i < canGo.Length; i++)
                canGo[i] = true;

            for (int i = 1; i <= User.Mobility; i++)
            {
                for (int b = 0; b < directions.GetLength(0); b++)
                {
                    Vector2Int temp;
                    temp = userPosition + directions[b] * (2 * i - 1);
                    if (canGo[b] && FieldManager.IsInField(temp) && FieldManager.GetTile(temp).IsPositionable(User))
                        positions.Add(temp);
                    else
                        canGo[b] = false;

                    temp = userPosition + directions[b] * 2 * i;
                    if (canGo[b] && FieldManager.IsInField(temp) && FieldManager.GetTile(temp).IsPositionable(User))
                        positions.Add(temp);
                    else
                        canGo[b] = false;
                }
            }

            foreach(var i in new List<Vector2Int>{ Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                if (FieldManager.IsInField(userPosition + i) && FieldManager.GetTile(userPosition + i).IsPositionable(User))
                    positions.Add(userPosition + i);

            return positions;
        }
    }
}