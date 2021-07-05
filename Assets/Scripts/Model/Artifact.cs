using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Artifact : Obtainable
    {
        public Effect antiqueEffect;
        public ObjectInfo info;

        public void AssignTo(Unit unit)
        {
            unit.Antiques.Add(this);
        }
        public TileBase GetTileBase()
        {
            return info.tilebase;
        }
    }
}