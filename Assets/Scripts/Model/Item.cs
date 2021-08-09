using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Item : Skill, Obtainable
    {
        public Vector2Int Position { get; set; }
        public int Price { get; set; }

        public void ToBag()
        {
            Managers.GameManager.Instance.itemBag.Add(this);
        }

        public virtual List<Vector2Int> GetRelatePositions(Vector2Int position)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            return positions;
        }

        public virtual void Use(Tile tile) { }
    }
}