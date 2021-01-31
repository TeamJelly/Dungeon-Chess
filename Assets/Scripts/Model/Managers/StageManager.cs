using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;
using Model;
using UI;

namespace Model.Managers
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager instance;

        public Room currentRoom
        {
            get => GameManager.Instance.currentRoom;
            set => GameManager.Instance.currentRoom = value;
        }

        public Room[,] AllRooms
        {
            get => GameManager.Instance.AllRooms;
            set => GameManager.Instance.AllRooms = value;
        }// 10층, 각층에는 5개

        public int numberOfFloors = 14;
        public int numberOfRoomsPerOneFloor = 5;

        public List<Vector2Int>[] pathList
        {
            get => GameManager.Instance.pathList;
            set => GameManager.Instance.pathList = value;
        }


        public List<Vector2Int> roomHistory
        {
            get => GameManager.Instance.roomHistory;
            set => GameManager.Instance.roomHistory = value;
        }

        private void Awake()
        {
            instance = this;
            if (currentRoom == null)
            {
                InitStage(1, 0);
                foreach (var item in AllRooms)
                    if (item.isActivate == true && item.category == Room.Category.NULL)
                        Debug.LogError("error"); // 없는 방 존재
                                                 //        }
            }
        }

        public void Restart()
        {
            GameManager.Reset();
            SceneManager.LoadScene("#003_Stage");
        }

        void InitStage(int stageNumber, int difficulty)
        {
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 1단계 : 방을 생성한다.

            AllRooms = new Room[numberOfFloors, numberOfRoomsPerOneFloor]; // 보스제외하고 13개의 층

            for (int i = 0; i < numberOfFloors; i++) // 0
                for (int j = 0; j < numberOfRoomsPerOneFloor; j++)
                    AllRooms[i, j] = new Room(new Vector2Int(i, j));

            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 2단계 : 방을 연결한다.

            pathList = new List<Vector2Int>[numberOfRoomsPerOneFloor];

            for (int i = 0; i < numberOfRoomsPerOneFloor; i++) // 5개의 길
                AllRooms[0, i].isActivate = true;

            for (int i = 0; i < numberOfRoomsPerOneFloor; i++) // 5개의 길
            {
                int rand;
                Room currentRoom = AllRooms[0, i];
                pathList[i] = new List<Vector2Int>();
                pathList[i].Add(currentRoom.position);

                while (currentRoom.position.x != numberOfFloors - 2) // 층수가 12층이 될때까지 반복
                {

                    if (currentRoom.position.y == 0)
                        rand = Random.Range(0, 2);
                    else if (currentRoom.position.y == numberOfRoomsPerOneFloor - 1) // 맨 오른쪽 방이라면
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

                    pathList[i].Add(currentRoom.position);

                    if (currentRoom.position.x == numberOfFloors - 2) // 13층 - 보스룸 연결
                    {
                        currentRoom.center = AllRooms[numberOfFloors - 1, 0];
                        pathList[i].Add(AllRooms[numberOfFloors - 1, 0].position);
                    }
                }
            }


            AllRooms[numberOfFloors - 1, 0].isActivate = true;

            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 3단계 : 방을 배정한다.

            int numberOfRooms = 0; // 방 개수 세기

            for (int i = 0; i < numberOfFloors; i++)
                for (int j = 0; j < numberOfRoomsPerOneFloor; j++)
                    if (AllRooms[i, j].isActivate)
                        numberOfRooms++;

            for (int i = 0; i < numberOfRoomsPerOneFloor; i++)
            {
                if (AllRooms[0, i].isActivate) // 1층 방은 전투방이다.
                {
                    AllRooms[0, i].SetRoomCategory(Room.Category.Monster);
                    numberOfRooms--;
                }
                if (AllRooms[(numberOfFloors - 1) / 2, i].isActivate) // 중간층은 보물방이다.
                {
                    AllRooms[(numberOfFloors - 1) / 2, i].SetRoomCategory(Room.Category.Treasure);
                    numberOfRooms--;
                }
                if (AllRooms[numberOfFloors - 2, i].isActivate) // 보스 전층은 휴식방이다.
                {
                    AllRooms[numberOfFloors - 2, i].SetRoomCategory(Room.Category.Tavern);
                    numberOfRooms--;
                }
            }

            AllRooms[numberOfFloors - 1, 0].SetRoomCategory(Room.Category.Boss);
            numberOfRooms--;

            int numberOfShop = (numberOfRooms * 5 + 99) / 100; // 올림 계산
            int numberOfTavern = (numberOfRooms * 10 + 99) / 100;
            int numberOfElite = (numberOfRooms * 10 + 99) / 100;
            int numberOfEvent = (numberOfRooms * 20 + 99) / 100;
            int numberOfMonster = numberOfRooms - numberOfShop - numberOfTavern - numberOfEvent - numberOfElite;

            for (int i = 0; i < numberOfFloors; i++)
            {
                for (int j = 0; j < numberOfRoomsPerOneFloor; j++)
                {

                    List<Room.Category> possibleRooms = new List<Room.Category>();
                    List<int> numberOfPossibleRooms = new List<int>();
                    int maxRandNumber = 0;
                    Room currentRoom = AllRooms[i, j];

                    if (currentRoom.isActivate == false || currentRoom.category == Room.Category.NULL)
                        continue;

                    if (currentRoom.category != Room.Category.Elite)
                    {
                        possibleRooms.Add(Room.Category.Elite);
                        numberOfPossibleRooms.Add(numberOfElite);
                        maxRandNumber += numberOfElite;
                    }
                    if (currentRoom.category != Room.Category.Shop)
                    {
                        possibleRooms.Add(Room.Category.Shop);
                        numberOfPossibleRooms.Add(numberOfShop);
                        maxRandNumber += numberOfShop;
                    }
                    if (currentRoom.category != Room.Category.Tavern)
                    {
                        possibleRooms.Add(Room.Category.Tavern);
                        numberOfPossibleRooms.Add(numberOfTavern);
                        maxRandNumber += numberOfTavern;
                    }

                    possibleRooms.Add(Room.Category.Event);
                    numberOfPossibleRooms.Add(numberOfEvent);
                    maxRandNumber += numberOfEvent;

                    possibleRooms.Add(Room.Category.Monster);
                    numberOfPossibleRooms.Add(numberOfMonster);
                    maxRandNumber += numberOfMonster;

                    List<Room> nextRooms = new List<Room>();

                    if (currentRoom.left != null)
                        nextRooms.Add(currentRoom.left);
                    if (currentRoom.center != null)
                        nextRooms.Add(currentRoom.center);
                    if (currentRoom.right != null)
                        nextRooms.Add(currentRoom.right);

                    foreach (var nextRoom in nextRooms)
                    {
                        if (nextRoom.category == Room.Category.NULL)
                        {
                            int rand = Random.Range(0, maxRandNumber);
                            int temp = 0;

                            for (int k = 0; k < possibleRooms.Count; k++)
                            {
                                temp += numberOfPossibleRooms[k];

                                if (rand < temp)
                                {
                                    nextRoom.category = possibleRooms[k];
                                    //numberOfPossibleRooms[k]--;

                                    maxRandNumber -= numberOfPossibleRooms[k];

                                    if (nextRoom.category == Room.Category.Monster)
                                        numberOfMonster--;
                                    else if (nextRoom.category == Room.Category.Event)
                                        numberOfEvent--;
                                    else if (nextRoom.category == Room.Category.Elite)
                                        numberOfElite--;
                                    else if (nextRoom.category == Room.Category.Tavern)
                                        numberOfTavern--;
                                    else if (nextRoom.category == Room.Category.Shop)
                                        numberOfShop--;

                                    possibleRooms.RemoveAt(k);
                                    numberOfPossibleRooms.RemoveAt(k);

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //        Debug.LogError("numberOfRooms : " + numberOfRooms);
            //        Debug.LogError("numberOfShop : " + numberOfShop);
            //        Debug.LogError("numberOfTavern : " + numberOfTavern);
            //        Debug.LogError("numberOfEvent : " + numberOfEvent);
            //        Debug.LogError("numberOfElite : " + numberOfElite);
            //        Debug.LogError("numberOfMonster : " + numberOfMonster);

            foreach (var item in AllRooms)
                if (item.isActivate == true && item.category == Room.Category.NULL)
                    if (numberOfShop != 0)
                    {
                        item.category = Room.Category.Shop;
                        numberOfShop--;
                    }
                    else if (numberOfTavern != 0)
                    {
                        item.category = Room.Category.Tavern;
                        numberOfTavern--;
                    }
                    else if (numberOfElite != 0)
                    {
                        item.category = Room.Category.Elite;
                        numberOfElite--;
                    }
                    else if (numberOfEvent != 0)
                    {
                        item.category = Room.Category.Event;
                        numberOfEvent--;
                    }
                    else if (numberOfMonster != 0)
                    {
                        item.category = Room.Category.Monster;
                        numberOfMonster--;
                    }
        }

        //UI에 의해 선택된 방 방문
        public void VisitRoom(Room room)
        {
            //현재 방 갱신 및 기록 추가
            currentRoom = room;
            roomHistory.Add(new Vector2Int(room.position.x, room.position.y));
            SceneLoader.LoadRoom(room.category);
        }
    }
}