using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [System.Serializable]
    public class Artifact
    {
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

        public virtual void OnAddThisArtifact()
        {


        }

        public virtual void OnRemoveThisArtifact()
        {

        }
    }

}