using UnityEngine;
using Model.Managers;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Stair : Tile
    {
        public Stair()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/stair_up");
            category = TileCategory.Stair;
            Initials = "ST";
        }
    }
}