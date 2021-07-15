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

        protected Unit unit = null;

        protected Obtainable obtainable = null;

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

        public virtual void SetUnit(Unit newUnit)
        {        
            unit = newUnit;
        }

        public Unit GetUnit()
        {
            return unit;
        }

        // 타일에 Obtainable Model을 등록한다.
        public void SetObtainable(Obtainable obt)
        {
            obtainable = obt;
        }

        // 타일에서 Obtainable Model을 반환한다.
        public Obtainable GetObtainable()
        {
            return obtainable;
        }

        public virtual void OnTile(Unit unit)
        {
            if (obtainable != null)
            {
                obtainable.ToBag();
                View.FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"{obtainable.Name} 획득!", Color.yellow);
                Common.Command.UnSummon(obtainable);
            }
        }
    }
}