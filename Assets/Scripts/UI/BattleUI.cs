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
   
    public GameObject TilePrefab;
    public GameObject TileIndicatorPrefab;
    public GameObject UnitInfoUIPrefab;

    List<UnitInfoUI> UnitsInfoList = new List<UnitInfoUI>();

    public UnitInfoUI selectedUnitInfo;

    public EventTrigger[,] AllTiles = new EventTrigger[10,10];

    public Button endTurn;
    public Button endTurn_enemy; //임시로 배치한 몬스터 전용 턴 종료 버튼.

    Transform tileIndicator;
    UnitPosition tileIndicatorPosition = new UnitPosition();

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        //유닛과 대응하는 UI 생성.
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
                int x = i; int y = j;

                //타일 클릭시 이벤트 추가
                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                entry_PointerClick.callback.AddListener((data) =>
                {
                    StartCoroutine(ShowMoveAnimation());
                    //BattleManager.instance.thisTurnUnit.Move(tileIndicatorPosition);
                });
                AllTiles[i, j].triggers.Add(entry_PointerClick);


                //타일 위로 마우스 지나갈 때 이벤트 추가 
                EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
                entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
                entry_PointerEnter.callback.AddListener((data) =>
                {
                    Vector2Int distance = new Vector2Int();
                    if(tileIndicatorPosition.lowerLeft.x > x)
                    {
                        distance.x = x - tileIndicatorPosition.lowerLeft.x;
                    }
                    else if(tileIndicatorPosition.upperRight.x < x)
                    {
                        distance.x = x - tileIndicatorPosition.upperRight.x;
                    }
                    if (tileIndicatorPosition.lowerLeft.y > y)
                    {
                        distance.y = y - tileIndicatorPosition.lowerLeft.y;
                    }
                    else if(tileIndicatorPosition.upperRight.y < y)
                    {
                        distance.y = y - tileIndicatorPosition.upperRight.y;
                    }

                    for(int k = tileIndicatorPosition.lowerLeft.x; k <= tileIndicatorPosition.upperRight.x; k++)
                    {
                        for(int l = tileIndicatorPosition.lowerLeft.y; l <= tileIndicatorPosition.upperRight.y; l++)
                        {
                            if (!AllTiles[k + distance.x, l + distance.y].gameObject.activeSelf) return;
                        }
                    }
                    tileIndicatorPosition.Add(distance);
                    SetTileIndicator(tileIndicatorPosition);

                });
                AllTiles[i, j].triggers.Add(entry_PointerEnter);
            }

        tileIndicator = Instantiate(TileIndicatorPrefab).transform;
        endTurn.onClick.AddListener(() =>
        {
            BattleManager.instance.SetNextTurn();
        });
        endTurn_enemy.onClick.AddListener(() =>
        {
            BattleManager.instance.SetNextTurn();
        });
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
        }
    }

    public void UpdateTurnStatus(Unit unit)
    {
        UpdateInfoList();

        if (PartyManager.instance.AllUnits.Contains(unit))
        {
            selectedUnitInfo.Set(unit);
            selectedUnitInfo.gameObject.SetActive(true);
            endTurn_enemy.gameObject.SetActive(false);
        }
        else
        {
            selectedUnitInfo.gameObject.SetActive(false);
            endTurn_enemy.gameObject.SetActive(true);
        }
        Debug.Log("현재 턴:" +  unit.name);

        SetTileIndicator(unit.unitPosition);

        ShowTile(unit.GetMovablePosition());
    }

    public void SetTileIndicator(UnitPosition position)
    {
        tileIndicatorPosition.Set(position);//깊은복사
        tileIndicator.localScale =
                new Vector3(position.upperRight.x - position.lowerLeft.x + 1,
                            position.upperRight.y - position.lowerLeft.y + 1,
                            1);
        Vector3 screenPosition = position.lowerLeft + (Vector2)(position.upperRight - position.lowerLeft) / 2;
        screenPosition.z = -4;
        tileIndicator.localPosition = screenPosition;
    }

    public void ShowTile(List<UnitPosition> positions)
    {
        HideTile();
        foreach (UnitPosition unitPosition in positions)
            for (int i = unitPosition.lowerLeft.x; i <= unitPosition.upperRight.x; i++)
            {
                for (int j = unitPosition.lowerLeft.y; j <= unitPosition.upperRight.y; j++)
                {
                    AllTiles[i, j].gameObject.SetActive(true);
                }
            }
    }

    public void HideTile()
    {
        foreach (var tile in AllTiles)
            if (tile.gameObject.activeSelf)
                tile.gameObject.SetActive(false);
    }



}
