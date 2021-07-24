using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
   // [System.Serializable]
    public class Artifact : Effect, Obtainable 
    {
        public Vector2Int Position { get; set; }

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