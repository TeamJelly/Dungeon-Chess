using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;

namespace Model.Tiles
{
    public class ShopTile : Tile
    {
        public override void OnTile(Unit unit)
        {
            if(obtainable != null)
            {
                 OXView.Enable("Buy", BuyFunction, CancelFunction);
            }
        }

        void BuyFunction()
        {
            int price = obtainable.price;
            if (GameManager.Instance.Gold < price)
            {
                Debug.Log("�� ����.");
                //���� �����մϴ� â Ȱ��ȭ
            }
            else
            {
                GameManager.Instance.Gold -= price;
                obtainable.ToBag();
                View.FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"{obtainable.Name} ȹ��!", Color.yellow);
                Common.Command.UnSummon(obtainable);

                Debug.Log("Gold: " + GameManager.Instance.Gold);
                Debug.Log("Price: " + price);
                Debug.Log("Buy");
                OXView.Disable();
            }
        }

        void CancelFunction()
        {
            OXView.Disable();
        }
    }
}