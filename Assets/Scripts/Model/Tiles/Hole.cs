using UnityEngine;
using Model.Managers;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Hole : Tile
    {
        public Hole()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Tile_Outline");
            category = TileCategory.Hole;
        }
    }
}