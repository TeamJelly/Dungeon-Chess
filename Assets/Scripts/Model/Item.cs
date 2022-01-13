using Model.Managers;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Model
{
    [System.Serializable]
    public class Item : Skill, Obtainable
    {
        public Vector2Int Position { get; set; }
        public int Price { get; set; }

        //public void BelongTo(Unit unit)
        //{
        //    if (unit.Alliance == UnitAlliance.Party && GameManager.Instance.itemBag.Count == 3)
        //        return;

        //    GameManager.Instance.itemBag.Add(this);
        //    Common.Command.UnSummon(this);
        //    View.FadeOutTextView.MakeText(unit, $"+{Name}", Color.yellow);
        //    UnitControlView.instance.UpdateItemButtons();                
        //}

        public virtual List<Vector2Int> GetRelatePositions(Vector2Int position)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            return positions;
        }

        public virtual void Use(Tile tile) { }

        public new Item Clone()
        {
            return System.Activator.CreateInstance(GetType()) as Item;
        }
    }
}