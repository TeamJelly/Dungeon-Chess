using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Heal : Tile
    {
        public Heal()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Heal");
            category = TileCategory.Heal;
        }
        
        public int heal = 3;

        public override void OnTile(Unit newUnit)
        {
            base.OnTile(newUnit);

            newUnit.OnTurnEnd.before.AddListener(HealFunctionCallback);
        }

        bool HealFunctionCallback(bool b)
        {
            Common.Command.Heal(unit, heal);
            unit.OnTurnEnd.before.RemoveListener(HealFunctionCallback);
            return b;
        }
    }
}