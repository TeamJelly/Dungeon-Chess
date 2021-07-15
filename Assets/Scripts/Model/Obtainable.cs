using Model;
using UnityEngine;

public interface Obtainable
{
    string Name{get; set;}
    Vector2Int Position{get; set;}
    void ToBag();
    Sprite GetImage();
}