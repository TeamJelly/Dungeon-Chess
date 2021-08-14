using Model.Effects;
using Model.Tiles;
using System;
using UnityEngine;

namespace Model.Items
{
    class Key : Item
    {
        public Key()
        {
            Name = "열쇠";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_559");
            Target = TargetType.Any;
            Color = Color.white;
            //Debug.Log("!");
        }

        public override void Use(Tile tile)
        {
            if (tile.GetType() == typeof(Tiles.Locked))
            {
                ((Locked)tile).Unlock();
            }
        }
    }
}