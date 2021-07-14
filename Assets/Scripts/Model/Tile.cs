using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Tile
    {
        public int number;
        public string name;

        public Vector2Int position;

        Obtainable obtainableObject = null;

        public enum Category
        {
            Floor   = 'F',
            Wall    = 'W',
            UpStair   = 'U',
            DownStair   = 'D',
            Thorn   = 'T',
            Hole    = 'H',
        };

        public Category category;

        public Effect tileEffect;

        Unit unit = null;

        public bool IsUnitPositionable(Unit unit)
        {
            if (HasUnit())
               return false;
            if (
                    unit.IsFlying == false && // 유닛이 날고있다면 위치 가능하다.
                    (
                        category == Category.Wall ||
                        category == Category.Hole
                    )
                )
                return false;
            else
                return true;
        }


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

        public void SetObtainableObj(Obtainable obt)
        {
            obtainableObject = obt;
        }

        public Obtainable GetObtainableObj()
        {
            Obtainable obt = obtainableObject;
            if(obt != null)
            {
                obtainableObject = null;
            }
            return obt;
        }
    }
}