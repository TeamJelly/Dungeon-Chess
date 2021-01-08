using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public Room currentRoom;

    public Room[,] AllRooms; // 10층, 각층에는 5개
    public Room finalRoom;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InitStage(1, 0);

        //        showStageUI();
    }

    void InitStage(int stageNumber, int difficulty)
    {
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 1단계 : 방을 생성한다.

        AllRooms = new Room[13, 5]; // 보스제외하고 13개의 층
        finalRoom = new Room(13, 0); // 14층 첫번째 방

        for (int i = 0; i < AllRooms.GetLength(0); i++) // 0
            for (int j = 0; j < AllRooms.GetLength(1); j++)
                AllRooms[i, j] = new Room(new Vector2Int(i, j));

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 2단계 : 방을 연결한다.

        for (int i = 0; i < AllRooms.GetLength(1); i++) // 5개의 길
            AllRooms[0, i].isActivate = true;

        for (int i = 0; i < AllRooms.GetLength(1); i++) // 5개의 길
        {
            int rand;
            Room currentRoom = AllRooms[0, i];

            while (currentRoom.position.x != AllRooms.GetLength(0)) // 층수가 13층이 될때까지 반복
            {
                Debug.LogError(currentRoom.position);

                if (currentRoom.position.y == 0)
                    rand = Random.Range(0, 1);
                else if (currentRoom.position.y == AllRooms.GetLength(1) - 1) // 맨 오른쪽 방이라면
                    rand = Random.Range(-1, 0);
                else // 그 사이라면
                    rand = Random.Range(-1, 1);

                if (rand == 0 && currentRoom.center != null)
                {
                    currentRoom.center = AllRooms[currentRoom.position.x + 1, currentRoom.position.y];
                    currentRoom = currentRoom.center;
                    currentRoom.isActivate = true;
                }
                else if (rand == 1 && currentRoom.right != null)
                {
                    currentRoom.right = AllRooms[currentRoom.position.x + 1, currentRoom.position.y + 1];
                    currentRoom = currentRoom.right;
                    currentRoom.isActivate = true;
                }
                else if (rand == -1 && currentRoom.left != null)
                {
                    currentRoom.left = AllRooms[currentRoom.position.x + 1, currentRoom.position.y - 1];
                    currentRoom = currentRoom.left;
                    currentRoom.isActivate = true;
                }
            }
        }

        for (int i = 0; i < AllRooms.GetLength(1); i++) // 13층 - 보스룸 연결
        {
            if (AllRooms[AllRooms.GetLength(0) - 1, i].isActivate)
                AllRooms[AllRooms.GetLength(0)-1, i].center = finalRoom;
        }

        finalRoom.isActivate = true;

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 3단계 : 방을 배정한다.

        int numberOfRooms = 0; // 방 개수 세기

        for (int i = 0; i < AllRooms.GetLength(0); i++)
            for (int j = 0; j < AllRooms.GetLength(1); j++)
                if (AllRooms[i, j].isActivate)
                    numberOfRooms++;

        for (int i = 0; i < AllRooms.GetLength(0); i++)
        {
            if (AllRooms[0, i].isActivate) // 1층 방은 전투방이다.
                AllRooms[0, i].SetRoomCategory(Room.Category.Monster);
            if (AllRooms[(AllRooms.GetLength(0) + 1) / 2, i].isActivate) // 중간층은 보물방이다.
                AllRooms[(AllRooms.GetLength(0) + 1) / 2, i].SetRoomCategory(Room.Category.Treasure);
            if (AllRooms[AllRooms.GetLength(0) - 1, i].isActivate) // 보스 전층은 휴식방이다.
                AllRooms[AllRooms.GetLength(0) - 1, i].SetRoomCategory(Room.Category.Tavern);
        }

        finalRoom.SetRoomCategory(Room.Category.Boss);

        Debug.LogError(numberOfRooms);

    }



}
