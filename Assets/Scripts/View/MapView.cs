using Model.Managers;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace View
{
    public class MapView : MonoBehaviour
    {
        public static MapView instance;

        public GameObject panel;
        public GameObject contentPanel;
        public GameObject floorPrefab;
        public GameObject roomPrefab;
        public GameObject clearSignPrefab;
        public RectTransform linePrefab;

        public Button[,] AllRoomButtons;

        public Transform selector;

        public List<GameObject> floors;
        public List<GameObject> lines;

        public List<Sprite> roomImages;

        

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
           InitStageUI(MapManager.instance.AllRooms);
        }
        public void Enable()
        {
            UIEffect.FadeInPanel(panel);
        }
        public void Disable()
        {
            UIEffect.FadeOutPanel(panel);
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


            RectTransform rt = (RectTransform)contentPanel.transform;
            Debug.Log(rt.rect.width);
            rt.offsetMax = new Vector2(((RectTransform)floorPrefab.transform).rect.width * AllRooms.GetLength(0) - rt.rect.width, rt.offsetMax.y);
            
            for (int i = 0; i < AllRooms.GetLength(0); i++)
            {
                GameObject floor = Instantiate(floorPrefab, contentPanel.transform);

                ((RectTransform)floor.transform).anchoredPosition = Vector2.right * ((RectTransform)floor.transform).rect.width * i;
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
                        AllRoomButtons[i, j].onClick.AddListener(() => MapManager.instance.VisitRoom(room));
                        AllRoomButtons[i, j].interactable = false; //생성된 모든 버튼은 처음에 비활성화.
                    }
                }

                float gap = ((RectTransform)floor.transform).rect.height / floor.transform.childCount;
                float centerHeight = ((RectTransform)floor.transform).rect.height * 0.5f;
                for(int j = 0; j < floor.transform.childCount; j++)
                {
                    RectTransform child = (RectTransform)floor.transform.GetChild(j);
                    child.anchorMin =  Vector2.one * 0.5f;
                    child.anchorMax =  Vector2.one * 0.5f;
                    child.pivot = Vector2.one * 0.5f;
                    child.sizeDelta = Vector2.one * 120; //
                    child.anchoredPosition = new Vector2(0, -gap * j + centerHeight  - gap * 0.5f);
                }
            }

            //스테이지 첫 진입시 스테이지 진입지점만 활성화
            if (MapManager.instance.currentRoom == null)
            {
                for (int i = 0; i < AllRooms.GetLength(1); i++)
                {
                    AllRoomButtons[0, i].interactable = true;
                }
            }
            else // 이전 기록 데이터로 UI 초기화
            {
                foreach (Vector2Int roomPos in MapManager.instance.roomHistory)
                {
                    SetVisitedRoom(roomPos);
                }
                ShowAvailableRoom(MapManager.instance.currentRoom);
            }

            //방 잇는 라인UI 생성
            /*for (int i = 0; i < AllRooms.GetLength(1); i++)
            {
                lines.Add(Instantiate(linePrefab));
            }*/
            Canvas.ForceUpdateCanvases();
            GenerateLines2();
            //Invoke("GenerateLines", 0.05f);
        }

        /// <summary>
        /// 방을 잇는 라인을 그려줌.
        /// </summary>
        void GenerateLines()
        {
            for (int j = 0; j < MapManager.instance.pathList.Length; j++)
            {
                List<Vector2Int> path = MapManager.instance.pathList[j];
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

        void GenerateLines2()
        {
            Room[,] AllRooms = MapManager.instance.AllRooms;
            for (int i = 0; i < AllRooms.GetLength(0); i++)
            {
                for (int j = 0; j < AllRooms.GetLength(1); j++)
                {
                    Room room = AllRooms[i, j];
                    if (room.isActivate)
                    {
                        Room roomChecker = room.left;
                        if (roomChecker != null)
                        {
                            DrawLine(room, roomChecker);
                        }
                        roomChecker = room.center;
                        if (roomChecker != null)
                        {
                            DrawLine(room, roomChecker);
                        }

                        roomChecker = room.right;
                        if (roomChecker != null)
                        {
                            DrawLine(room, roomChecker);
                        }
                    }
                }
            }
        }

        void DrawLine(Room A, Room B)
        {
            float angle = GetAngle(((RectTransform)AllRoomButtons[A.position.x, A.position.y].transform).position,
                                   ((RectTransform)AllRoomButtons[B.position.x, B.position.y].transform).position);
            RectTransform line = Instantiate(linePrefab,contentPanel.transform);
            line.position = AllRoomButtons[A.position.x,A.position.y].transform.position;


            Vector2 posA = ((RectTransform)AllRoomButtons[A.position.x, A.position.y].transform).anchoredPosition;
            Vector2 posB = ((RectTransform)AllRoomButtons[B.position.x, B.position.y].transform).anchoredPosition;

            posA.x += A.position.x * 300;
            posB.x += B.position.x * 300;

            float distance = Vector2.Distance(posA, posB);
            line.sizeDelta = new Vector2(distance , 6);
    
            line.eulerAngles = new Vector3(0, 0, angle);
            line.SetParent(contentPanel.transform);
            line.SetAsFirstSibling();
        }
        float GetAngle(Vector3 vStart, Vector3 vEnd)
        {
            Vector3 v = vEnd - vStart;
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        }

    }
}