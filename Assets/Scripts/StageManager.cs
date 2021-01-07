using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public Room currentRoom;

    Room[,] AllRooms; // 10층, 각층에는 5개

    Room finalRoom;

    void InitStage(int stageNumber, int difficulty)
    {
        AllRooms = new Room[9, 5];
        finalRoom = new Room();

        for (int i = 0; i < AllRooms.GetLength(0)-1; i++) // 0 ~ 10층
        {
            for (int j = 0; j < AllRooms.GetLength(1); j++)
            {
                int rand;

                if (j.Equals(0)) // 만약 층의 맨 왼쪽 방이거나 맨 오른쪽 방이라면
                    rand = Random.Range(0, 1);
                else if (j.Equals(AllRooms.GetLength(1) - 1))
                    rand = Random.Range(-1, 0);
                else
                    rand = Random.Range(-1, 1);

                Random.Range(0, 2);
                if (rand.Equals(0))
                {

                } else if (rand.Equals(1))
                {

                } else
                {

                }
            }
        }


    }

    private void Start()
    {
        instance = this;
        InitStage(1, 0);

//        showStageUI();
    }

}
