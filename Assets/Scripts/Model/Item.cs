using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Item : Obtainable
    {
        public ObjectInfo info;
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