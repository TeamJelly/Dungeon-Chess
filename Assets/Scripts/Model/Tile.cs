using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [System.Serializable]
    public class Tile
    {
        public int number;
        public string name;

        public Vector2Int position;

        public enum Category
        {
            Floor   = 'F',
            Wall    = 'W',
            Stair   = 'S',
            Thorn   = 'T',
            Hole    = 'H',
        };

        public Category category;

        public Effect tileEffect;

        Unit unit = null;

        public bool HasUnit()
        {
            return unit != null;
        }

        public bool HasUnit(Unit unit)
        {
            if (this.unit == unit)
                return true;
            else
                return false;
        }

        public void SetUnit(Unit newUnit)
        {
            unit = newUnit;
        }

        public Unit GetUnit()
        {
            return unit;
        }
    }
}