using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using View.UI;

namespace Model
{
    [System.Serializable]
    public class Tile : Infoable
    {
        public string Name { get => category.ToString() + " tile"; }

        public Sprite Sprite { get => Model.Managers.FieldManager.instance.tileMap.GetSprite(new Vector3Int(position.x, position.y, 0)); }

        public Color Color { get; set; }

        public string Description { get; set; }

        public Vector2Int position;

        public enum Category
        {
            Floor       = 'F',
            Wall        = 'W',
            Back        = 'B',
            Next        = 'N',
            Thorn       = 'T',
            Void        = 'V',
            Sell        = 'S',
            Heal        = 'H',
            Power       = 'P',
            Locked      = 'L',
            UnLocked    = 'U',
        };

        public Category category;

        protected Unit unit = null;

        protected Obtainable obtainable = null;


        public Tile() { }

        // 인자로 넘긴 해당 유닛이 위치할수 있는지를 검사한다.
        public bool IsPositionable(Unit unit)
        {
            if (HasUnit())
               return false;
            else if (
                    unit.IsFlying == false && // 유닛이 날고있다면 위치 가능하다.
                    (
                        category == Category.Wall ||
                        category == Category.Void ||
                        category == Category.Locked
                    )
                )
                return false;
            else
                return true;
        }


        //obtainable -> item
        //Droptem
        // 아이템이 위치할수 있는지 검사한다.
        public bool IsPositionable()
        {
            // 유닛이나 획득물품이 존재하면 false
            if (HasUnit() || HasObtainable())
                return false;
            // 벽이거나 구멍이면 false
            else if (category == Category.Wall || category == Category.Void || category == Category.Locked)
                return false;
            // 이외의 경우에 가능하다.
            else
                return true;
        }

        public bool HasUnit()
        {
            return unit != null;
        }

        public bool HasObtainable()
        {
            return obtainable != null;
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
                obtainable.BelongTo(unit);
                //Bag.instance.UpdateUI();
            }
        }
    }
}