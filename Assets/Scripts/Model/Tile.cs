using UnityEngine;
using UnityEngine.Tilemaps;
using Common;
using Model.Managers;

namespace Model
{

    [System.Serializable]
    public class Tile : MonoBehaviour
    {
        public Vector2Int position;
        public Unit unit;
        public Sprite sprite;
        public TileBase tileBase;

        //     // 인자로 넘긴 해당 유닛이 위치할수 있는지를 검사한다.
        //     public bool IsPositionable(Unit unit)
        //     {
        //         if (HasUnit())
        //             return false;
        //         else if (category == TileCategory.Wall)
        //             return false;
        //         else if (
        //                 unit.IsFlying == false && // 유닛이 날고있지 않다면, 벽 구멍 잠금타일은 갈수 없다.
        //                 (
        //                     //                        category == TileCategory.Wall ||
        //                     category == TileCategory.Hole ||
        //                     category == TileCategory.Locked
        //                 )
        //             )
        //             return false;
        //         else
        //             return true;
        //     }

        //     //obtainable -> item
        //     //Droptem
        //     // 아이템이 위치할수 있는지 검사한다.
        //     public bool IsItemPositionable()
        //     {
        //         // 유닛이나 획득물품이 존재하면 false
        //         if (HasUnit() || HasObtainable())
        //             return false;
        //         // 벽이거나 구멍이면 false
        //         else if (category == TileCategory.Wall || category == TileCategory.Hole || category == TileCategory.Locked)
        //             return false;
        //         // 이외의 경우에 가능하다.
        //         else
        //             return true;
        //     }

        //     public bool HasUnit()
        //     {
        //         return unit != null;
        //     }

        //     public bool HasObtainable()
        //     {
        //         return obtainable != null;
        //     }

        //     // public virtual void SetUnit(Unit newUnit)
        //     // {
        //     //     unit = newUnit;
        //     // }

        //     public Unit GetUnit()
        //     {
        //         return unit;
        //     }

        //     // 타일에 Obtainable Model을 등록한다.
        //     public void SetObtainable(Obtainable obt)
        //     {
        //         obtainable = obt;
        //     }

        //     // 타일에서 Obtainable Model을 반환한다.
        //     public Obtainable GetObtainable()
        //     {
        //         return obtainable;
        //     }

        //     public virtual void OnTile(Unit unit)
        //     {
        //         this.unit = unit;

        //         if (obtainable != null && BattleManager.GetUnit(UnitAlliance.Party).Contains(unit))
        //         {
        //             unit.AddObtainable(obtainable);
        //             //obtainable.BelongTo(unit);Í
        //             //Bag.instance.UpdateUI();
        //         }
        //     }

        //     public virtual void OffTile()
        //     {
        //         this.unit = null;
        //     }

        //     public Tile Clone() => (Tile)System.Activator.CreateInstance(GetType());
    }
}