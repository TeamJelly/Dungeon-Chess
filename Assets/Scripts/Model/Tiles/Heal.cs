using System.Collections.Generic;
using System.Threading.Tasks;
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
            Initials = "HL";
        }

        public int heal = 3;

        public override void OnTile(Unit newUnit)
        {
            base.OnTile(newUnit);

            newUnit.OnTurnEnd.before.AddListener(HealFunctionCallback);
        }

        public override void OffTile()
        {
            Debug.Log(unit);
            Debug.Log(unit.Name);
            Debug.Log(unit.OnTurnEnd);
            Debug.Log(unit.OnTurnEnd.before);
            unit.OnTurnEnd.before.RemoveListener(HealFunctionCallback);

            // tile.unit = null;
            base.OffTile();
        }

        private async Task<bool> HealFunctionCallback(bool b)
        {
            await Common.Command.Heal(unit, heal);
            return b;
        }
    }
}