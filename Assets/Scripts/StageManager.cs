using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public Room currentRoom;

    public Room[,] AllRooms; // 10층, 각층에는 5개
    public Room finalRoom;

    public int numberOfShop;
    public int numberOfTavern;
    public int numberOfEvent;
    public int numberOfElite;
    public int numberOfMonster;

    public List<Vector2Int>[] pathList;

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

        pathList = new List<Vector2Int>[AllRooms.GetLength(1)];

        for (int i = 0; i < AllRooms.GetLength(1); i++) // 5개의 길
            AllRooms[0, i].isActivate = true;

        for (int i = 0; i < AllRooms.GetLength(1); i++) // 5개의 길
        {
            int rand;
            Room currentRoom = AllRooms[0, i];
            pathList[i] = new List<Vector2Int>();

            while (currentRoom.position.x != AllRooms.GetLength(0) - 1) // 층수가 13층이 될때까지 반복
            {
                //                Debug.LogError(currentRoom.position);
                pathList[i].Add(currentRoom.position);

                if (currentRoom.position.y == 0)
                    rand = Random.Range(0, 2);
                else if (currentRoom.position.y == AllRooms.GetLength(1) - 1) // 맨 오른쪽 방이라면
                    rand = Random.Range(-1, 1);
                else // 그 사이라면
                    rand = Random.Range(-1, 2);

                if (rand == 0)
                {
                    if (currentRoom.center == null)
                    {
                        currentRoom.center = AllRooms[currentRoom.position.x + 1, currentRoom.position.y];
                        currentRoom.center.isActivate = true;
                    }

                    currentRoom = currentRoom.center;
                }
                else if (rand == 1)
                {
                    if (currentRoom.right == null)
                    {
                        currentRoom.right = AllRooms[currentRoom.position.x + 1, currentRoom.position.y + 1];
                        currentRoom.right.isActivate = true;
                    }

                    currentRoom = currentRoom.right;
                }
                else //if (rand == -1)
                {
                    if (currentRoom.left == null)
                    {
                        currentRoom.left = AllRooms[currentRoom.position.x + 1, currentRoom.position.y - 1];
                        currentRoom.left.isActivate = true;
                    }

                    currentRoom = currentRoom.left;
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

        for (int i = 0; i < AllRooms.GetLength(1); i++)
        {
            if (AllRooms[0, i].isActivate) // 1층 방은 전투방이다.
            {
                AllRooms[0, i].SetRoomCategory(Room.Category.Monster);
                numberOfRooms--;
            }
            else if (AllRooms[(AllRooms.GetLength(0) + 1) / 2, i].isActivate) // 중간층은 보물방이다.
            {
                AllRooms[(AllRooms.GetLength(0) + 1) / 2, i].SetRoomCategory(Room.Category.Treasure);
                numberOfRooms--;
            }
            else if (AllRooms[AllRooms.GetLength(0) - 1, i].isActivate) // 보스 전층은 휴식방이다.
            {
                AllRooms[AllRooms.GetLength(0) - 1, i].SetRoomCategory(Room.Category.Tavern);
                numberOfRooms--;
            }
        }

        finalRoom.SetRoomCategory(Room.Category.Boss);

        numberOfShop = (numberOfRooms * 5 + 99) / 100; // 올림 계산
        numberOfTavern = (numberOfRooms * 10 + 99) / 100;
        numberOfEvent = (numberOfRooms * 20 + 99) / 100;
        numberOfElite = (numberOfRooms * 10 + 99) / 100;
        numberOfMonster = numberOfRooms - numberOfShop - numberOfTavern - numberOfEvent - numberOfElite;

        Debug.LogError("numberOfShop : " + numberOfShop);
        Debug.LogError("numberOfTavern : " + numberOfTavern);
        Debug.LogError("numberOfEvent : " + numberOfEvent);
        Debug.LogError("numberOfElite : " + numberOfElite);
        Debug.LogError("numberOfMonster : " + numberOfMonster);

        StageUI.instance.InitStageUI();
    }



}
