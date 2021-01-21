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
    public GameObject UnitInfoUIPrefab;

    [Header("Buttons")]
    public Button currentPushedButton; // 현재 누르고 있는 버튼
    public Button moveButton;
    public List<Button> skillButtons;
    public List<Button> itemButtons;

    List<UnitInfoUI> UnitsInfoList = new List<UnitInfoUI>();
    public UnitInfoUI thisTurnUnitInfo;

    RectTransform unitTurnIndicator;
    IndicatorManager indicatorManager;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        indicatorManager = IndicatorManager.instance;

        //전체 유닛들 클릭시 유닛 설명창 활성화 기능 추가
        foreach (Unit unit in BattleManager.instance.AllUnits)
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

    IEnumerator ShowMoveAnimation(Unit unit, UnitPosition from, UnitPosition to)
    {
        List<UnitPosition> path = BattleManager.PathFindAlgorithm(unit, from, to);

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

    public void InitThisTurnPanel(Unit unit)
    {
        currentPushedButton = null;
        indicatorManager.DestoryAll();

        UpdateInfoList();

        if (PartyManager.instance.AllUnits.Contains(unit)) // 파티원인가?
        {
            foreach (var item in skillButtons) // 스킬 버튼 일단 다 없앰
            {
                item.interactable = false;
                item.GetComponent<Image>().sprite = null;
            }

            for (int i = 0; i < unit.skills.Count; i++) // 스킬이 존재하면 이미지를 넣어준다.
                skillButtons[i].GetComponent<Image>().sprite = unit.skills[i].skillImage;

            UpdateThisTurnPanel();

            thisTurnUnitInfo.gameObject.SetActive(true);
            unitTurnIndicator.gameObject.SetActive(true);
        }
        else // 파티원이 아니라면 현재 유닛 정보와 인디케이터를 보여주지 않는다.
        {
            thisTurnUnitInfo.gameObject.SetActive(false);
            unitTurnIndicator.gameObject.SetActive(false);
        }
    }

    public void UpdateThisTurnPanel()
    {
        UpdateThisTurnPanel(BattleManager.instance.thisTurnUnit);
    }

    public void UpdateThisTurnPanel(Unit unit)
    {
        thisTurnUnitInfo.Set(unit);
        moveButton.interactable = false; // 이동 버튼 비활성화
        foreach (Button skillButton in skillButtons) // 스킬 버튼 비활성화
            skillButton.interactable = false;
        foreach (Button itemButton in itemButtons) // 스킬 버튼 비활성화
            itemButton.interactable = false;

        if (currentPushedButton == null) // 아무 버튼도 안눌린 기본 상태
        {
            if (unit.moveCount > 0) // 이동 가능한가?
            {
                moveButton.interactable = true;
                moveButton.onClick.RemoveAllListeners();

                moveButton.onClick.AddListener(() => // 이동 버튼을 눌렀을 때 작동하는 코드
                {
                    indicatorManager.InitMainIndicator(BattleManager.instance.thisTurnUnit.unitPosition, indicatorManager.mainTileIndicatorPrefab);
                    indicatorManager.AddIndicatorBoundary(BattleManager.instance.thisTurnUnit.GetMovablePositions(), indicatorManager.tileIndicatorBoundaryPrefab);
                    indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();

                    EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                    entry_PointerClick.eventID = EventTriggerType.PointerClick;
                    entry_PointerClick.callback.AddListener((data) =>
                    {
                        StartCoroutine(ShowMoveAnimation(unit, unit.unitPosition, indicatorManager.GetUnitPositionOnMainIndicator()));
                        indicatorManager.DestoryAll();

                        BattleManager.instance.thisTurnUnit.moveCount--;
                        currentPushedButton = null;
                        UpdateThisTurnPanel();
                    });

                    indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);

                    currentPushedButton = moveButton;
                    UpdateThisTurnPanel();
                });
            }

            for (int i = 0; i < unit.skills.Count; i++)
            {
                Skill skill = unit.skills[i];

                if (skill.currentReuseTime == 0 && unit.skillCount > 0) // 스킬이 사용가능한 조건
                {
                    skillButtons[i].interactable = true;
                    skillButtons[i].onClick.RemoveAllListeners();

                    int temp = i;

                    skillButtons[i].onClick.AddListener(() => // 스킬 버튼을 눌렀을 때 작동하는 코드
                    {
                        EventTrigger.Entry entry_PointerClick;

                        if ((skill.target == Skill.Target.AnyTile || skill.target == Skill.Target.NoUnitTile)
                            && skill.domain == Skill.Domain.Fixed)
                        {
                            if (skill.GetPositionsInDomain(unit).Count != 0)
                                indicatorManager.InitMainIndicator(skill.GetPositionsInDomain(unit)[0], indicatorManager.mainTileIndicatorPrefab);
                            // 메인 인디케이터 생성
                            indicatorManager.AddSubIndicator(skill.GetRangePositions(), indicatorManager.subTileIndicatorPrefab);
                            // 서브 인디케이터 생성
                            indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
                            // 인디케이터 바운더리 생성
                            indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();
                            // 인디케이터 바운더리에 따라오기 엔터 트리거 설정

                            entry_PointerClick = new EventTrigger.Entry();
                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToTile(indicatorManager.GetTilesOnIndicator()); });
                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
                            // 인디케이터에 커스텀 클릭 트리거 설정
                        }
                        else if ((skill.target == Skill.Target.AnyTile || skill.target == Skill.Target.NoUnitTile)
                            && skill.domain == Skill.Domain.Rotate)
                        {
                            if (skill.GetPositionsInDomain(unit).Count != 0)
                                indicatorManager.InitMainIndicator(skill.GetPositionsInDomain(unit)[0], indicatorManager.mainTileIndicatorPrefab);
                            // 메인 인디케이터 생성
                            indicatorManager.AddSubIndicator(skill.GetRangePositions(), indicatorManager.subTileIndicatorPrefab);
                            // 서브 인디케이터 생성
                            indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
                            // 인디케이터 바운더리 생성

                            indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();
                            // 인디케이터 바운더리에 따라오기 엔터 트리거 설정
                            indicatorManager.SetRotateEnterTriggerOnIndicatorBoundary();
                            // 인디케이터 바운더리에 회전 엔터 트리거 설정

                            entry_PointerClick = new EventTrigger.Entry();
                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToTile(indicatorManager.GetTilesOnIndicator()); });
                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
                            // 인디케이터에 커스텀 클릭 트리거 설정
                        }
                        else if ((skill.target == Skill.Target.AnyTile || skill.target == Skill.Target.NoUnitTile)
                          && skill.domain == Skill.Domain.RandomOne)
                        {
                            if (skill.GetPositionsInDomain(unit).Count != 0)
                                indicatorManager.InitMainIndicator(skill.GetPositionsInDomain(unit)[0], indicatorManager.mainTileIndicatorPrefab);
                            // 메인 인디케이터 생성
                            indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.mainTileIndicatorPrefab);
                            // 인디케이터 바운더리 생성
                                
                            indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();
                            // 인디케이터 바운더리에 따라오기 엔터 트리거 설정

                            entry_PointerClick = new EventTrigger.Entry();
                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToTile(indicatorManager.GetTilesOnIndicator()); });
                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
                            // 인디케이터에 커스텀 클릭 트리거 설정
                        }
                        else if ((skill.target == Skill.Target.AnyUnit || skill.target == Skill.Target.EnemyUnit
                            || skill.target == Skill.Target.FriendlyUnit || skill.target == Skill.Target.PartyUnit)
                            && skill.domain == Skill.Domain.RandomOne)
                        {
                            if (skill.GetUnitPositionsInDomain(unit).Count != 0)
                                indicatorManager.InitMainIndicator(skill.GetUnitPositionsInDomain(unit)[0], indicatorManager.mainUnitIndicatorPrefab);
                            // 메인 인디케이터 생성
                            indicatorManager.AddIndicatorBoundary(skill.GetUnitPositionsInDomain(unit), indicatorManager.mainUnitIndicatorPrefab);
                            // 인디케이터 바운더리 생성
                            indicatorManager.SetEqualizeEnterTriggerOnIndicatorBoundary();
                            // 인디케이터 바운더리에 동일화 엔터 트리거 설정

                            entry_PointerClick = new EventTrigger.Entry();
                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToUnit(indicatorManager.GetUnitsOnIndicator()); });
                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
                            // 인디케이터에 커스텀 클릭 트리거 설정

                            indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
                            // 엔터 트리거 없는 바운더리 추가
                        }
                        else if ((skill.target == Skill.Target.AnyUnit || skill.target == Skill.Target.EnemyUnit
                            || skill.target == Skill.Target.FriendlyUnit || skill.target == Skill.Target.PartyUnit) 
                            && skill.domain == Skill.Domain.SelectOne)
                        {
                            if (skill.GetUnitPositionsInDomain(unit).Count != 0)
                                indicatorManager.InitMainIndicator(skill.GetUnitPositionsInDomain(unit)[0], indicatorManager.mainUnitIndicatorPrefab);
                            // 메인 인디케이터 생성
                            indicatorManager.AddIndicatorBoundary(skill.GetUnitPositionsInDomain(unit), indicatorManager.unitIndicatorBoundaryPrefab);
                            // 인디케이터 바운더리 생성
                            indicatorManager.SetEqualizeEnterTriggerOnIndicatorBoundary();
                            // 인디케이터 바운더리에 동일화 엔터 트리거 설정

                            entry_PointerClick = new EventTrigger.Entry();
                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToUnit(indicatorManager.GetUnitsOnIndicator()); });
                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
                            // 인디케이터에 커스텀 클릭 트리거 설정

                            indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
                            // 엔터 트리거 없는 바운더리 추가
                        }

                        entry_PointerClick = new EventTrigger.Entry();
                        entry_PointerClick.eventID = EventTriggerType.PointerClick;
                        entry_PointerClick.callback.AddListener((data) =>
                        {
                            indicatorManager.DestoryAll();
                            BattleManager.instance.thisTurnUnit.moveCount--;
                            BattleManager.instance.thisTurnUnit.skillCount--;
                            currentPushedButton = null;
                            UpdateThisTurnPanel();
                        });
                        indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
                        // 공통 커스텀 클릭 트리거 설정

                        currentPushedButton = skillButtons[temp];
                        UpdateThisTurnPanel();
                    });
                }
            }

            for (int i = 0; i < unit.items.Count; i++)
            {
                //아이템 버튼
            }
        }
        else // 버튼 눌린 상태, 타 버튼 비활성화, 다시 누르면 타일 표시기들 제거
        {
            currentPushedButton.interactable = true; // 취소버튼만 활성화

            currentPushedButton.onClick.RemoveAllListeners();
            currentPushedButton.onClick.AddListener(() =>
            {
                indicatorManager.DestoryAll();

                currentPushedButton = null;
                UpdateThisTurnPanel();
            });
        }
    }
}
