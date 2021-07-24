using Model;
using UnityEngine;

public interface Obtainable : Spriteable
{
    string Name{get; set;}
    Vector2Int Position{get; set;}
    void ToBag();
}