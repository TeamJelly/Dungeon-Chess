using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;

namespace Model.Tiles
{
    public class DamageTile : Tile
    {
        public int damage = 3;
        public override void OnTile(Unit unit)
        {
            base.OnTile(unit);

            Common.Command.Damage(unit, 3);
        }
    }
}