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
            Name = "Key";
            UserTarget = TargetType.Any;

            SpriteNumber = 559;
            InColor = Color.white;
            OutColor = Color.clear;
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