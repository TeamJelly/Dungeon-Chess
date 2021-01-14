using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    static GameManager instance;

    public List<Vector2Int> roomHistory = new List<Vector2Int>();
    public Room currentRoom = null;
    public Room[,] AllRooms = null;
    public List<Vector2Int>[] pathList = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
}
