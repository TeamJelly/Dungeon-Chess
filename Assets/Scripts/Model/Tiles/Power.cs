using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Power : Tile
    {
        public Power()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Strength");
            category = TileCategory.Power;
        }

        public int strength = 3;

        public override void OnTile(Unit unit)
        {
            base.OnTile(unit);
            FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"STR +{strength}", Color.green);
            unit.Strength += strength;

            Vector2Int RemoveBuffCallback(Vector2Int v)
            {
                FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"STR -{strength}", Color.green);
                unit.Strength -= strength;
                unit.OnMove.after.RemoveListener(RemoveBuffCallback);
                return v;
            }
            unit.OnMove.after.AddListener(RemoveBuffCallback);
        }
        
    }
}