using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Item : Obtainable, Spriteable
    {
        GameObject gameObject;

        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }

        protected string spritePath;
        public Sprite Sprite { get => Common.Data.LoadSprite(spritePath); }

        public Vector2Int Position { get; set; }

        public virtual void OnAddThisItem()
        {

        }

        public virtual void OnRemoveThisItem()
        {

        }

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