using UnityEngine;
using Model.Managers;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace Model.Tiles
{
    public class Floor : Tile
    {
        private static TileBase[] tileBases;

        public static TileBase[] TileBases
        {
            get
            {
                if (tileBases == null)
                    tileBases = Resources.LoadAll<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Floors");
                return tileBases;
            }
        }

        public Floor()
        {
            TileBase = TileBases[Random.Range(0, 5)];

            category = TileCategory.Floor;
        }
    }
}