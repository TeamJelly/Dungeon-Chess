using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public enum Category
    {
        NULL, Boss, Monster, Elite, Treasure, Shop, Tavern, Event
    }

    public bool isCleared = false;
    public bool isSelectable = false;

    Category category = Category.NULL;

    Room right, left, center;
}
