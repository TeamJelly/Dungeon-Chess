using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public int number;
    public string name;

    public Vector2Int position;

    public Effect tileEffect;

    public Unit unit = null;

    public bool IsUsable()
    {
        return unit == null;
    }
}
