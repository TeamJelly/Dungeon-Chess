using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;

namespace Model.Tiles
{
    public class Locked : Tile
    {
        public object TileManager { get; private set; }

        public void Unlock()
        {
            category = Category.UnLocked;
            FieldManager.instance.UpdateTile(this);
        }
    }
}