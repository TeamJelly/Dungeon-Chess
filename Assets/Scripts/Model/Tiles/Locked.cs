using UnityEngine;
using Model.Managers;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Locked : Tile
    {
        public Locked()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Locked");
            category = TileCategory.Locked;
            Initials = "LK";
        }

        public void Unlock()
        {
            category = TileCategory.UnLocked;
            FieldManager.instance.UpdateTile(this);
        }
    }
}