using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;

namespace Model.Tiles
{
    public class Power : Tile
    {
        public Power()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Strength");
            category = TileCategory.Power;
            Initials = "PW";
        }

        public int strength = 3;

        public async override void OnTile(Unit unit)
        {
            base.OnTile(unit);

            await FadeOutTextView.MakeText(unit, $"STR +{strength}", Color.green);
            unit.Strength += strength;

            async Task<Vector2Int> RemoveBuffCallback(Vector2Int v)
            {
                await FadeOutTextView.MakeText(unit, $"STR -{strength}", Color.green);
                unit.Strength -= strength;
                unit.OnMove.after.RemoveListener(RemoveBuffCallback);
                return v;
            }

            unit.OnMove.after.AddListener(RemoveBuffCallback);
        }

    }
}