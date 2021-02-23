using Model.Managers;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StageUI : MonoBehaviour
    {
        public static StageUI instance;

        public GameObject contentPanel;
        public GameObject floorPrefab;
        public GameObject roomPrefab;
        public GameObject linePrefab;
        public GameObject clearSignPrefab;

        public Button[,] AllRoomButtons;

        public Transform selector;

        public List<GameObject> floors;
        public List<GameObject> lines;

        public List<Sprite> roomImages;

        private void Start()
        {
           InitStageUI(StageManager.instance.AllRooms);
        }
        /// <summary>
        /// 이용 가능한 방 및 현재 방 표시
        /// </summary>
        /// <param name="room">현재 방 위치</param>
        void ShowAvailableRoom(Room room)
        {
            //현재 방 위치 표시기 갱신
            selector.SetParent(AllRoomButtons[room.position.x, room.position.y].transform);
            selector.position = AllRoomButtons[room.position.x, room.position.y].transform.position;

            //이동 가능한 방 활성화
            Room roomChecker = room.left;
            if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;

            roomChecker = room.center;
            if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;

            roomChecker = room.right;
            if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
        }

        /// <summary>
        /// 방문한 방에 대해 표시
        /// </summary>
        /// <param name="roomPos">방의 위치정보</param>
        void SetVisitedRoom(Vector2Int roomPos)
        {
            Transform roomButtonTransform = AllRoomButtons[roomPos.x, roomPos.y].transform;

            //방문한 방은 더욱 어둡게 보이게 함.
            Image image = roomButtonTransform.GetComponent<Image>();
            Color color = image.color / 4; color.a = 1;
            image.color = color;

            //방문 했음을 나타내는 마크 생성
            Transform t = Instantiate(clearSignPrefab, roomButtonTransform).transform;
            t.localPosition = Vector2.zero;
        }

        /// <summary>
        /// UI 생성
        /// </summary>
        /// <param name="AllRooms">생성할 모든 방에대한 정보를 가져온다.</param>
        public void InitStageUI(Room[,] AllRooms)
        {
            AllRoomButtons = new Button[AllRooms.GetLength(0), AllRooms.GetLength(1)];

            for (int i = 0; i < AllRooms.GetLength(0); i++)
            {
                GameObject floor = Instantiate(floorPrefab, contentPanel.transform);
                floor.name = "floor " + i;
                floors.Add(floor);

                for (int j = 0; j < AllRooms.GetLength(1); j++)
                {
                    Room room = AllRooms[i, j];
                    if (room.isActivate)
                    {
                        //버튼 생성 및 이미지 세팅
                        AllRoomButtons[i, j] = Instantiate(roomPrefab, floor.transform).GetComponent<Button>();
                        AllRoomButtons[i, j].GetComponent<Image>().sprite = roomImages[room.category.GetHashCode()];

                        //각 방의 버튼마다 이벤트 부여
                        AllRoomButtons[i, j].onClick.AddListener(() => StageManager.instance.VisitRoom(room));
                        AllRoomButtons[i, j].interactable = false; //생성된 모든 버튼은 처음에 비활성화.
                    }
                }
            }

            //스테이지 첫 진입시 스테이지 진입지점만 활성화
            if (StageManager.instance.currentRoom == null)
            {
                for (int i = 0; i < AllRooms.GetLength(1); i++)
                {
                    AllRoomButtons[0, i].interactable = true;
                }
            }
            else // 이전 기록 데이터로 UI 초기화
            {
                foreach (Vector2Int roomPos in StageManager.instance.roomHistory)
                {
                    SetVisitedRoom(roomPos);
                }
                ShowAvailableRoom(StageManager.instance.currentRoom);
            }

            //방 잇는 라인UI 생성
            for (int i = 0; i < AllRooms.GetLength(1); i++)
            {
                lines.Add(Instantiate(linePrefab));
            }
            Invoke("GenerateLines", 0.05f);

        }

        /// <summary>
        /// 방을 잇는 라인을 그려줌.
        /// </summary>
        void GenerateLines()
        {
            

            for (int j = 0; j < StageManager.instance.pathList.Length; j++)
            {
                List<Vector2Int> path = StageManager.instance.pathList[j];
                LineRenderer lineRenderer = lines[j].GetComponent<LineRenderer>();
                lineRenderer.positionCount = path.Count;

                for (int i = 0; i < path.Count; i++)
                {
                    Vector3 roomPosition = AllRoomButtons[path[i].x, path[i].y].transform.position;
                    lineRenderer.SetPosition(i, roomPosition);
                }
                lines[j].transform.SetParent(contentPanel.transform);
            }
        }

    }
}