using Ludiq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;

    public GameObject moveTileButton;

    public GameObject unitsInfoPanel;
    public List<TextMeshProUGUI> unitsInfoText = new List<TextMeshProUGUI>();

    public GameObject TileSelectorPrefab;
    public EventTrigger[,] tileSelectorList = new EventTrigger[10,10];
    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        //유닛과 대응하는 UI 생성
        foreach(Unit unit in BattleManager.instance.AllUnits)
        {
            RectTransform t = new GameObject("Info_" + unit.name).AddComponent<RectTransform>(); ;
            t.transform.SetParent(unitsInfoPanel.transform);
            unitsInfoText.Add(t.AddComponent<TextMeshProUGUI>());
        }
        UpdateInfoList();

        //이동시 선택하는 타일 전부 생성.
        Transform tileSelectorParent = new GameObject("TileSelector").transform;
        for(int i = 0; i < 10; i++)
            for(int j = 0; j < 10; j++)
            {
                tileSelectorList[i, j] = GameObject.Instantiate(TileSelectorPrefab).GetComponent<EventTrigger>();
                tileSelectorList[i, j].transform.SetParent(tileSelectorParent);
                tileSelectorList[i, j].transform.position = new Vector3(i, j,-2);
                tileSelectorList[i, j].gameObject.SetActive(false);
            }
    }

    //유닛들 정보 갱신.
    public void UpdateInfoList()
    {
        for (int i = 0; i < BattleManager.instance.AllUnits.Count; i++)
        {
            UpdateInfo(i);
        }
    }

    public void UpdateInfo(int index)
    {
        Unit unit = BattleManager.instance.AllUnits[index];
        unitsInfoText[index].text = unit.name +
            "\nHP:" + unit.currentHP + "/" + unit.maxHP;
    }
    public void UpdateTurnStatus(Unit unit)
    {
        showTileSelector(unit.GetMovablePosition());
    }
    public void showTileSelector(List<UnitPosition> positions)
    {
        foreach (var tileSelector in tileSelectorList)
            tileSelector.gameObject.SetActive(false);

        foreach(UnitPosition unitPosition in positions)
        for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
        {
            for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
            {
                    //tileSelectorList[i, j].
                    tileSelectorList[i, j].gameObject.SetActive(true);
            }
        }
    }

    public void HideTileSelector()
    {

    }



}
