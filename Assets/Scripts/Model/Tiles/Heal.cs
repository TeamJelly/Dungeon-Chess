using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;

namespace Model.Tiles
{
    public class Heal : Tile
    {
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