using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View.UI;

namespace Model.Tiles
{
    public class ShopTile : Tile
    {
        public override void OnTile(Unit unit)
        {
            if(obtainable != null)
            {
                Confirm.Enable("Buy?", BuyFunction, CancelFunction);
            }
        }

        void BuyFunction()
        {
            int price = obtainable.price;
            if (GameManager.Instance.Gold < price)
            {
                View.FadeOutTextView.MakeText(unit.Position + Vector2Int.up,"돈이 부족하다.", Color.red);
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