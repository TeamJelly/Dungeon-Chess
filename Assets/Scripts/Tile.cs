using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public int number;
    public new string name;

    public Vector2Int position;

    public Effect tileEffect;

    public Unit unit = null;

    public bool IsUsable()
    {
        return unit == null;
    }
}
