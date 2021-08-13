using Model;
using UnityEngine;

public interface Obtainable : Spriteable, Buyable
{
    string Name{get; set;}
    Vector2Int Position{get; set;}
    void BelongTo(Unit unit);
}