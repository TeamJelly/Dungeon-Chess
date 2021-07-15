using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Artifact : Obtainable
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
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

        public Vector2Int Position { get; set; }

        public virtual void OnAddThisArtifact()
        {


        }

        public virtual void OnRemoveThisArtifact()
        {

        }

        public Sprite GetImage()
        {
            return Sprite;
        }

        public void ToBag()
        {
            Managers.GameManager.Instance.artifactBag.Add(this);
        }
    }
}