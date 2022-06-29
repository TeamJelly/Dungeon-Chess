using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [System.Serializable]
    public class Room
    {
        public enum Category
        {
            NULL = 0, Boss = 1, Monster = 2, Elite = 3, Treasure = 4, Shop = 5, Tavern = 6, Event = 7
        }

        public Category category = Category.NULL;

        public bool isActivate = false;
        // public bool isSelectable = false;

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
}