using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;

namespace Model.Tiles
{
    public class StrengthBuffTile : Tile
    {
        public int strength = 3;
        public override void SetUnit(Unit newUnit)
        {
            if (newUnit == null)
            {
                FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"STR -{strength}", Color.green);
                unit.Strength -= strength;
            }
            else
            {
                FadeOutTextView.MakeText(newUnit.Position + Vector2Int.up, $"STR +{strength}", Color.green);
                newUnit.Strength += strength;
            }

            unit = newUnit;
        }
    }
}