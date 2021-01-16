using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        instance = this;
    }

    public void SelectRoom(Room room)
    {
        //이전에 방문했던 방이 있다면 방문했음을 표시.
        if (StageManager.instance.currentRoom != null)
        {
            Vector2Int pos = StageManager.instance.currentRoom.position;
            SetVisitedRoom(pos.x, pos.y);
        }
        DisableRoomButtons();//일단 모든 방 비활성화

        //현재 방 갱신 및 기록 추가
        StageManager.instance.currentRoom = room;
        StageManager.instance.roomHistory.Add(new Vector2Int(room.position.x, room.position.y));
       
        //현재 방 위치 표시기 갱신
        selector.SetParent(AllRoomButtons[room.position.x, room.position.y].transform);
        selector.position = AllRoomButtons[room.position.x, room.position.y].transform.position;

        AllRoomButtons[room.position.x, room.position.y].enabled = false; // 현재 방 버튼 비활성화

        //이동 가능한 방 활성화
        Room roomChecker = room.left;
        if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
        roomChecker = room.center;
        if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
        roomChecker = room.right;
        if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
    }
    void SetVisitedRoom(int x, int y)
    {
        Image image = AllRoomButtons[x, y].GetComponent<Image>();
        Color color = image.color / 4; color.a = 1;
        image.color = color;

        Transform t = Instantiate(clearSignPrefab, AllRoomButtons[x, y].transform).transform;
        t.position = AllRoomButtons[x, y].transform.position;
    }

    void DisableRoomButtons()
    {
        
    for(int i = 0; i < AllRoomButtons.GetLength(0); i++)
        for(int j = 0; j < AllRoomButtons.GetLength(1); j++)
            if(AllRoomButtons[i,j] != null)
            {
                AllRoomButtons[i, j].interactable = false;
            }
    }
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
                    Button roomButton = Instantiate(roomPrefab, floor.transform).GetComponent<Button>();
                    roomButton.GetComponent<Image>().sprite = roomImages[room.category.GetHashCode()];

                    //각 방의 버튼마다 이벤트 부여
                    roomButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if(room.category == Room.Category.Monster)
                             SceneLoader.LoadScene("SampleScene");
                        SelectRoom(room);
                    });
                   
                    if (i != 0 || StageManager.instance.currentRoom != null) roomButton.interactable = false; //첫 시작 방.
                    AllRoomButtons[i, j] = roomButton;
                }
            }
        }
        for (int i = 0; i < AllRooms.GetLength(1); i++)
        {
            lines.Add(Instantiate(linePrefab));
        }
        foreach(Vector2Int roomPos in StageManager.instance.roomHistory)
        {
            SetVisitedRoom(roomPos.x, roomPos.y);
        }
        if (StageManager.instance.currentRoom != null)
            SelectRoom(StageManager.instance.currentRoom);
    }

    private void Update()
    {
        UpdateLines();
    }

    public void UpdateLines()
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
        }
    }

}
