using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Item : Obtainable
    {
        GameObject gameObject;

        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }

        protected string spritePath;

        private Sprite sprite;

        public Sprite Sprite
        {
            set => sprite = value;
            get
            {
                if (sprite == null)
                {
                    sprite = Common.Data.LoadSprite(spritePath);
                }
                return sprite;
            }
        }

        public virtual void OnAddThisItem()
        {


        }

        public virtual void OnRemoveThisItem()
        {

        }
        public void AssignTo(Unit unit)
        {
            
        }
        public void DropImage(Vector2Int position)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteImage()
        {
            throw new System.NotImplementedException();
        }
    }
}