using UnityEngine;
using Model.Managers;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Wall : Tile
    {
        public Wall()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/wall");
            category = TileCategory.Wall;
            Initials = "WL";
        }
    }
}