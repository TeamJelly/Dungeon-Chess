using Model;
using UnityEngine;

public interface Obtainable
{
    Vector2Int Position{get; set;}
    void ToBag();
    Sprite GetImage();
}