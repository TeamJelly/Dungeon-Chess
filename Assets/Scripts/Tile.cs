// using UnityEngine;
// using UnityEngine.Tilemaps;
// using Common;
// using Model.Managers;

// namespace Model
// {
//     public enum TileCategory
//     {
//         Floor,
//         Wall,
//         Stair,
//         Thorn,
//         Hole,
//         Sell,
//         Heal,
//         Power,
//         Locked,
//         UnLocked,
//         Water,
//     };


//     [System.Serializable]
//     public class Tile : Infoable
//     {
//         public string Name { get => category.ToString() + " tile"; }

//         public TileBase TileBase { get; set; }

//         public TileCategory category;

//         public string Initials { get; set; }

//         public Color Color { get; set; }

//         public string Description { get; set; }

//         public string Type => "Tile";

//         public Sprite Sprite { get => Model.Managers.FieldManager.instance.tileMap.GetSprite(new Vector3Int(position.x, position.y, 0)); }

//         public int SpriteNumber => throw new System.NotImplementedException();

//         public Color InColor => throw new System.NotImplementedException();

//         public Color OutColor => throw new System.NotImplementedException();

//         public string SpriteName => throw new System.NotImplementedException();

//         public Vector2Int position;

//         protected Unit unit = null;

//         protected Obtainable obtainable = null;

//         // 인자로 넘긴 해당 유닛이 위치할수 있는지를 검사한다.
//         public bool IsPositionable(Unit unit)
//         {
//             if (HasUnit())
//                 return false;
//             else if (category == TileCategory.Wall)
//                 return false;
//             else if (
//                     unit.IsFlying == false && // 유닛이 날고있지 않다면, 벽 구멍 잠금타일은 갈수 없다.
//                     (
//                         //                        category == TileCategory.Wall ||
//                         category == TileCategory.Hole ||
//                         category == TileCategory.Locked
//                     )
//                 )
//                 return false;
//             else
//                 return true;
//         }

//         //obtainable -> item
//         //Droptem
//         // 아이템이 위치할수 있는지 검사한다.
//         public bool IsItemPositionable()
//         {
//             // 유닛이나 획득물품이 존재하면 false
//             if (HasUnit() || HasObtainable())
//                 return false;
//             // 벽이거나 구멍이면 false
//             else if (category == TileCategory.Wall || category == TileCategory.Hole || category == TileCategory.Locked)
//                 return false;
//             // 이외의 경우에 가능하다.
//             else
//                 return true;
//         }

//         public bool HasUnit()
//         {
//             return unit != null;
//         }

//         public bool HasObtainable()
//         {
//             return obtainable != null;
//         }

//         // public virtual void SetUnit(Unit newUnit)
//         // {
//         //     unit = newUnit;
//         // }

//         public Unit GetUnit()
//         {
//             return unit;
//         }

//         // 타일에 Obtainable Model을 등록한다.
//         public void SetObtainable(Obtainable obt)
//         {
//             obtainable = obt;
//         }

//         // 타일에서 Obtainable Model을 반환한다.
//         public Obtainable GetObtainable()
//         {
//             return obtainable;
//         }

//         public virtual void OnTile(Unit unit)
//         {
//             this.unit = unit;

//             if (obtainable != null && BattleManager.GetUnit(UnitAlliance.Party).Contains(unit))
//             {
//                 unit.AddObtainable(obtainable);
//                 //obtainable.BelongTo(unit);Í
//                 //Bag.instance.UpdateUI();
//             }
//         }

//         public virtual void OffTile()
//         {
//             this.unit = null;
//         }

//         public Tile Clone() => (Tile)System.Activator.CreateInstance(GetType());
//     }
// }