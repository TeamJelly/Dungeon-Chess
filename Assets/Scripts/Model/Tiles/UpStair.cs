using UnityEngine;
using Model.Managers;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class UpStair : Tile
    {
        public UpStair()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/stair_up");
            category = TileCategory.UpStair;
        }
    }
}