using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public static StageUI instance;

    public GameObject contentPanel;
    public GameObject viewport;
    public GameObject floorPrefab;
    public GameObject roomPrefab;
    public GameObject linePrefab;
    public GameObject[] startPosition;

    public GameObject[,] AllRoomButtons;

    public List<GameObject> floors;
    public List<GameObject> lines;

    private void Awake()
    {
        instance = this;
    }

    public void InitStageUI()
    {
        AllRoomButtons = new GameObject[StageManager.instance.AllRooms.GetLength(0), StageManager.instance.AllRooms.GetLength(1)];

        for (int i = 0; i < StageManager.instance.AllRooms.GetLength(0); i++)
        {
            GameObject floor = Instantiate(floorPrefab, contentPanel.transform);
            floor.name = "floor " + i;

            floors.Add(floor);

            for (int j = 0; j < StageManager.instance.AllRooms.GetLength(1); j++)
            {
                if (StageManager.instance.AllRooms[i, j].isActivate)
                    AllRoomButtons[i,j] = Instantiate(roomPrefab, floor.transform);
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

                Debug.LogError(path[i] + " " + roomPosition);
                lineRenderer.SetPosition(i, roomPosition);
            }
        }
    }

}
