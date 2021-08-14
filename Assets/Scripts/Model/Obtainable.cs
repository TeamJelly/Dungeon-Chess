using Model;
using UnityEngine;

public interface Obtainable : Infoable, Buyable
{
    Vector2Int Position{get; set;}
    void BelongTo(Unit unit);
}