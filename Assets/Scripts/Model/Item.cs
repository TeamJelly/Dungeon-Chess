using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Item : Skill, Obtainable, Spriteable
    {
        public Vector2Int Position { get; set; }
        public int price { get; set; }
        public Color Color { get; set; }

        public Sprite GetImage()
        {
            return Sprite;
        }

        public void ToBag()
        {
            Managers.GameManager.Instance.itemBag.Add(this);
        }

    }
}