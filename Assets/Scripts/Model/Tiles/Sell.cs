using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View.UI;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Sell : Tile
    {
        public Sell()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Sell");
            category = TileCategory.Sell;
            Initials = "SL";
        }

        public override void OnTile(Unit unit)
        {
            if (obtainable != null && unit.Alliance == UnitAlliance.Party)
            {
                Confirm.Enable("Buy?", BuyFunction, CancelFunction);
            }
        }

        void BuyFunction()
        {
            int price = obtainable.Price;
            if (GameManager.Instance.Gold < price)
            {
                View.FadeOutTextView.MakeText(unit, "Not Enough Money", Color.red);
            }
            else
            {
                GameManager.Instance.Gold -= price;
                base.OnTile(unit);

                Debug.Log("Gold: " + GameManager.Instance.Gold);
                Debug.Log("Price: " + price);
                Debug.Log("Buy");
                Confirm.Disable();
            }
        }

        void CancelFunction()
        {
            Confirm.Disable();
        }
    }
}