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

    public void SelectRoom(int x, int y)
    {
        //이전에 방문했던 방이 있다면 방문했음을 표시.
        if (StageManager.instance.currentRoom != null)
        {
            Vector2Int pos = StageManager.instance.currentRoom.position;
            Image image = AllRoomButtons[pos.x, pos.y].GetComponent<Image>();
            Color color = image.color / 4; color.a = 1;
            image.color = color;

            Transform t = Instantiate(clearSignPrefab, AllRoomButtons[pos.x, pos.y].transform).transform;
            t.position = AllRoomButtons[pos.x, pos.y].transform.position;
        }

        DisableRoomButtons();//일단 모든 방 비활성화

        //현재 방 갱신 및 기록 추가
        StageManager.instance.currentRoom = StageManager.instance.AllRooms[x, y];
        StageManager.instance.roomHistory.Add(new Vector2Int(x,y));
       
        //현재 방 위치 표시기 갱신
        selector.SetParent(AllRoomButtons[x, y].transform);
        selector.position = AllRoomButtons[x, y].transform.position;

        AllRoomButtons[x, y].enabled = false; // 현재 방 버튼 비활성화

        //이동 가능한 방 활성화
        Room roomChecker = StageManager.instance.AllRooms[x, y].left;
        if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
        roomChecker = StageManager.instance.AllRooms[x, y].center;
        if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
        roomChecker = StageManager.instance.AllRooms[x, y].right;
        if (roomChecker != null) AllRoomButtons[roomChecker.position.x, roomChecker.position.y].interactable = true;
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
    public void InitStageUI()
    {
        AllRoomButtons = new Button[StageManager.instance.AllRooms.GetLength(0), StageManager.instance.AllRooms.GetLength(1)];

        for (int i = 0; i < StageManager.instance.AllRooms.GetLength(0); i++)
        {
            GameObject floor = Instantiate(floorPrefab, contentPanel.transform);
            floor.name = "floor " + i;

            floors.Add(floor);

            for (int j = 0; j < StageManager.instance.AllRooms.GetLength(1); j++)
            {
                if (StageManager.instance.AllRooms[i, j].isActivate)
                {
                    Button roomButton = Instantiate(roomPrefab, floor.transform).GetComponent<Button>();
                    roomButton.GetComponent<Image>().sprite = roomImages[StageManager.instance.AllRooms[i, j].category.GetHashCode()];

                    //각 방의 버튼마다 이벤트 부여
                    int x = i, y = j;
                    roomButton.GetComponent<Button>().onClick.AddListener(() => SelectRoom(x, y));
                    AllRoomButtons[i, j] = roomButton;
                    if (i != 0) AllRoomButtons[i, j].interactable = false;
                }
            }
        }

        for (int i = 0; i < StageManager.instance.AllRooms.GetLength(1); i++)
        {
            lines.Add(Instantiate(linePrefab));
        }
    }

    private void Update()
    {
        UpdateLines();
    }

    public void UpdateStage()
    {
        StageManager.instance.AllRooms.GetLength(0);
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
