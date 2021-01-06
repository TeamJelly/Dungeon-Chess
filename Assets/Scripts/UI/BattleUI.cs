using Ludiq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;

    public GameObject unitsInfoPanel;
   
    public GameObject TileSelectorPrefab;
    public GameObject UnitInfoUIPrefab;

    public List<UnitInfoUI> UnitsInfoList = new List<UnitInfoUI>();

    public EventTrigger[,] tileSelectorList = new EventTrigger[10,10];

    public Button endTurn;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        //유닛과 대응하는 UI 생성. AllUnit에서 아군유닛 수로 수정 해야 함.
        //1을 빼는 이유는 선택 유닛 UI는 미리 생성되어있기 때문.
        for(int i = 0; i < BattleManager.instance.AllUnits.Count - 1; i++)
        {
            GameObject g = GameObject.Instantiate(UnitInfoUIPrefab);
          
            g.transform.SetParent(unitsInfoPanel.transform);
            g.transform.SetAsFirstSibling();
            g.transform.localScale = Vector3.one;
            UnitsInfoList.Add(g.GetComponent<UnitInfoUI>());
        }

        //해당 ui 초기화
        foreach (UnitInfoUI unitInfoUI in UnitsInfoList) unitInfoUI.Init();

        //이동시 선택하는 타일 전부 생성.
        Transform tileSelectorParent = new GameObject("TileSelector").transform;
        for(int i = 0; i < 10; i++)
            for(int j = 0; j < 10; j++)
            {
                tileSelectorList[i, j] = GameObject.Instantiate(TileSelectorPrefab).GetComponent<EventTrigger>();
                tileSelectorList[i, j].transform.SetParent(tileSelectorParent);
                tileSelectorList[i, j].transform.position = new Vector3(i, j,-1);
                tileSelectorList[i, j].gameObject.SetActive(false);
                int x = i; int y = j;

                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                entry_PointerClick.callback.AddListener((data) =>
                {
                    BattleManager.instance.thisTurnUnit.Move(new Vector2Int(x, y));
                });
                tileSelectorList[i, j].triggers.Add(entry_PointerClick);
            }
        endTurn.onClick.AddListener(() =>
        {
            BattleManager.instance.SetNextTurn();
        });
    }

    //유닛 정보창 갱신.
    public void UpdateInfoList()
    {
        for (int i = 0; i < BattleManager.instance.AllUnits.Count; i++)
        {
            Unit unit = BattleManager.instance.AllUnits[i];
            UnitsInfoList[i].Set(unit);
        }
    }

    public void UpdateTurnStatus(Unit unit)
    {
        UpdateInfoList();
        Debug.Log("현재 턴:" +  unit.name);
        ShowTileSelector(unit.GetMovablePosition());
    }

    public void ShowTileSelector(List<UnitPosition> positions)
    {
        HideTileSelector();
        foreach (UnitPosition unitPosition in positions)
        for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
        {
            for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
            {
                   // Debug.Log(i + "," + j);
                    //tileSelectorList[i, j].
                    if(i >= 0 && i < 10 && j >= 0 && j < 10 && BattleManager.instance.AllTiles[i,j].IsUsable())
                    {
                        tileSelectorList[i, j].gameObject.SetActive(true);
                    }
                    
            }
        }

    }

    public void HideTileSelector()
    {
        foreach (var tileSelector in tileSelectorList)
            if (tileSelector.gameObject.activeSelf)
                tileSelector.gameObject.SetActive(false);
    }



}
