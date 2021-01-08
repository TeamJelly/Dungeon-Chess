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

    public Category category = Category.NULL;

    public bool isActivate = false;
//    public bool isCleared = false;
    public bool isSelectable = false;

    public Vector2Int position; // x = 층, y = 번째 방

    public Room right = null;
    public Room left = null;
    public Room center = null;

    public Room(int floor, int number)
    {
        position = new Vector2Int(floor, number);
    }

    public Room(Vector2Int position)
    {
        this.position = position;
    }

    public void SetRoomCategory(Category category)
    {
        this.category = category;
    }


}