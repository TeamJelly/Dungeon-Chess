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
        public int price { get; set; }

        public void ToBag()
        {
            Managers.GameManager.Instance.itemBag.Add(this);
        }

        public virtual void Use(Unit unit)
        {

        }
    }
}