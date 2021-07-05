using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface Obtainable
{
    void AssignTo(Unit unit);

    TileBase GetTileBase();

}