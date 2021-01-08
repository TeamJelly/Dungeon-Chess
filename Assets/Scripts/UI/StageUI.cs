using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public static StageUI instance;

    public GameObject contentPanel;
    public GameObject linePrefab;
    public GameObject RoomPrefab;
    public GameObject[] startPosition;
    public List<GameObject> floors;

    private void Awake()
    {
        instance = this;
    }

    public void InitStage()
    {
        for (int i = 0; i < StageManager.instance.AllRooms.GetLength(0); i++)
        {
            GameObject floor = Instantiate(new GameObject(), contentPanel.transform);

            floor.name = "floor " + i;
            VerticalLayoutGroup verticalLayoutGroup =  floor.AddComponent<VerticalLayoutGroup>();

            verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childControlWidth = false;
            verticalLayoutGroup.childForceExpandHeight = true;
            verticalLayoutGroup.childForceExpandWidth = true;

            floors.Add(floor);

            for (int j = 0; j < StageManager.instance.AllRooms.GetLength(1); j++)
                if (StageManager.instance.AllRooms[i, j].isActivate)
                    Instantiate(RoomPrefab, floor.transform);
        }
    }

    public void UpdateStage()
    {
        StageManager.instance.AllRooms.GetLength(0);

    }

    private void DrawLine()
    {
        LineRenderer lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();

        int numberOfFloor = StageManager.instance.AllRooms.GetLength(0);



    }

}
