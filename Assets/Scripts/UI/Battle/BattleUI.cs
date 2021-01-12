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
   
    public GameObject TilePrefab;
    public GameObject TileIndicatorPrefab;
    public GameObject UnitInfoUIPrefab;
    public GameObject[] SkillButton;
    public GameObject[] ItemButton;
    public GameObject MoveButton;
    public GameObject MoveCancelButton;

    List<UnitInfoUI> UnitsInfoList = new List<UnitInfoUI>();

    public UnitInfoUI selectedUnitInfo;

    public EventTrigger[,] AllTiles = new EventTrigger[10,10];

    Transform tileIndicator;
    UnitPosition tileIndicatorPosition = new UnitPosition();

    RectTransform unitTurnIndicator;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        //전체 유닛들 클릭시 유닛 설명창 활성화 기능 추가
        foreach(Unit unit in BattleManager.instance.AllUnits)
        {
            unit.gameObject.AddComponent<BoxCollider2D>();
            EventTrigger eventTrigger = unit.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
            entry_PointerClick.eventID = EventTriggerType.PointerClick;
            entry_PointerClick.callback.AddListener((data) =>
            {
                UnitDescriptionUI.instance.Enable(unit);
            });
            eventTrigger.triggers.Add(entry_PointerClick);
        }

        //파티원 유닛과 대응하는 UI 생성.
        for(int i = 0; i < PartyManager.instance.AllUnits.Count; i++)
        {
            GameObject g = Instantiate(UnitInfoUIPrefab);
          
            g.transform.SetParent(unitsInfoPanel.transform);
            g.transform.SetAsFirstSibling();
            g.transform.localScale = Vector3.one;

            UnitsInfoList.Add(g.GetComponent<UnitInfoUI>());

        }

        //해당 ui 초기화
        foreach (UnitInfoUI unitInfoUI in UnitsInfoList) unitInfoUI.Init();

        //이동시 선택하는 타일 전부 생성.
        Transform allTilesParent = new GameObject("TileSelector").transform;
        for(int i = 0; i < 10; i++)
            for(int j = 0; j < 10; j++)
            {
                AllTiles[i, j] = Instantiate(TilePrefab).GetComponent<EventTrigger>();
                AllTiles[i, j].transform.SetParent(allTilesParent);
                AllTiles[i, j].transform.position = new Vector3(i, j,-2);
                AllTiles[i, j].gameObject.SetActive(false);
                
            }

        //타일 선택기 생성
        tileIndicator = Instantiate(TileIndicatorPrefab).transform;
        tileIndicator.gameObject.SetActive(false);



        //유닛 턴 표시기 생성
        unitTurnIndicator = new GameObject().AddComponent<RectTransform>();
        unitTurnIndicator.SetParent(this.transform);
        unitTurnIndicator.sizeDelta = new Vector2(245, 120);
        unitTurnIndicator.gameObject.AddComponent<Image>().color = Color.yellow;
        unitTurnIndicator.localScale = Vector3.one;
        unitTurnIndicator.gameObject.SetActive(false);

        
    }

   

    //턴종료 버튼 이벤트
    public void SetNextTurn()
    {
        BattleManager.instance.SetNextTurn();
    }

    IEnumerator ShowMoveAnimation()
    {
        Unit unit = BattleManager.instance.thisTurnUnit;
        List<UnitPosition> path = BattleManager.PathFindAlgorithm(unit, unit.unitPosition, tileIndicatorPosition);

        var waitForSeconds = new WaitForSeconds(0.1f);
        foreach (UnitPosition position in path)
        {
            unit.Move(position);
            yield return waitForSeconds;
        }
    }

    //유닛 정보창 갱신.
    public void UpdateInfoList()
    {
        for (int i = 0; i < PartyManager.instance.AllUnits.Count; i++)
        {
            Unit unit = PartyManager.instance.AllUnits[i];
            UnitsInfoList[i].Set(unit);

            if(unit == BattleManager.instance.thisTurnUnit)
            {
                unitTurnIndicator.SetParent(UnitsInfoList[i].transform);
                unitTurnIndicator.SetAsFirstSibling();
                unitTurnIndicator.anchoredPosition = Vector2.zero;
            }
        }
    }

    public void UpdateTurnStatus(Unit unit)
    {
        UpdateInfoList();

        if (PartyManager.instance.AllUnits.Contains(unit))
        {
            selectedUnitInfo.Set(unit);

            foreach (GameObject skillButton in SkillButton)
                skillButton.SetActive(false);
            for(int i = 0; i < unit.skills.Count; i++)
            {
                SkillButton[i].GetComponent<Image>().sprite = unit.skills[i].skillImage;
                SkillButton[i].SetActive(true);
            }
               
            selectedUnitInfo.gameObject.SetActive(true);
            unitTurnIndicator.gameObject.SetActive(true);
        }
        else
        {
            selectedUnitInfo.gameObject.SetActive(false);
            unitTurnIndicator.gameObject.SetActive(false);
        }
        Debug.Log("현재 턴:" +  unit.name);

        MoveButton.SetActive(true);
    }

    public void SetTileIndicator(List<Vector2Int> position) // 스킬용 인디케이터 보여주기
    {

    }

    public void SetTileIndicator(UnitPosition position)
    {
        tileIndicatorPosition.Set(position);//깊은복사

        tileIndicator.localScale =
                new Vector3(
                    position.upperRight.x - position.lowerLeft.x + 1,
                    position.upperRight.y - position.lowerLeft.y + 1,
                    1
                );

        Vector3 screenPosition = position.lowerLeft + (Vector2)(position.upperRight - position.lowerLeft) / 2;
        screenPosition.z = -4;
        tileIndicator.localPosition = screenPosition;
        tileIndicator.gameObject.SetActive(true);
    }

    public void ShowTile(List<Vector2Int> positions)
    {
        HideTile();
    }

    public void ShowMovableTile()
    {
        //HideTile();
        SetTileIndicator(BattleManager.instance.thisTurnUnit.unitPosition);
        List<UnitPosition> positions = BattleManager.instance.thisTurnUnit.GetMovablePosition();
      
        foreach (UnitPosition unitPosition in positions)
            for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
            {
                for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
                {
                    SetMoveTile(i,j);
                    AllTiles[i, j].gameObject.SetActive(true);
                }
            }
    }

    void SetMoveTile(int x, int y)
    {
        AllTiles[x, y].triggers.Clear();
        //타일 클릭시 이벤트 추가
        EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
        entry_PointerClick.eventID = EventTriggerType.PointerClick;
        entry_PointerClick.callback.AddListener((data) =>
        {
            //BattleManager.instance.thisTurnUnit.Move(tileIndicatorPosition);
            
            HideTile();
            MoveCancelButton.SetActive(false);
            StartCoroutine(ShowMoveAnimation());
            //BattleManager.instance.thisTurnUnit.Move(tileIndicatorPosition);
        });
        AllTiles[x, y].triggers.Add(entry_PointerClick);

        //타일 위로 마우스 지나갈 때 이벤트 추가 
        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) =>
        {
            Vector2Int distance = new Vector2Int();
            if (tileIndicatorPosition.lowerLeft.x > x)
            {
                distance.x = x - tileIndicatorPosition.lowerLeft.x;
            }
            else if (tileIndicatorPosition.upperRight.x < x)
            {
                distance.x = x - tileIndicatorPosition.upperRight.x;
            }
            if (tileIndicatorPosition.lowerLeft.y > y)
            {
                distance.y = y - tileIndicatorPosition.lowerLeft.y;
            }
            else if (tileIndicatorPosition.upperRight.y < y)
            {
                distance.y = y - tileIndicatorPosition.upperRight.y;
            }

            for (int k = tileIndicatorPosition.lowerLeft.x; k <= tileIndicatorPosition.upperRight.x; k++)
            {
                for (int l = tileIndicatorPosition.lowerLeft.y; l <= tileIndicatorPosition.upperRight.y; l++)
                {
                    if (!AllTiles[k + distance.x, l + distance.y].gameObject.activeSelf) return;
                }
            }
            tileIndicatorPosition.Add(distance);
            SetTileIndicator(tileIndicatorPosition);

        });
        AllTiles[x, y].triggers.Add(entry_PointerEnter);
    }

    public void HideTile()
    {
        tileIndicator.gameObject.SetActive(false);
        foreach (var tile in AllTiles)
            if (tile.gameObject.activeSelf)
                tile.gameObject.SetActive(false);

    }



}
