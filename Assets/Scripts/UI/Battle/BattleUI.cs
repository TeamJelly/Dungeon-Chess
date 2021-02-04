using Common;
using Model.Managers;
using Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        public static BattleUI instance;

        public Dictionary<Unit, GameObject> unitObjects = new Dictionary<Unit, GameObject>();
        private Dictionary<Unit, Slider> hpBars = new Dictionary<Unit, Slider>();

        public GameObject unitsInfoPanel;
        public GameObject UnitInfoUIPrefab;
        public GameObject HPBarPrefab;

        [Header("Skill Info")]
        public GameObject SkillInfoInstance;
        public TextMeshProUGUI SkillInfoNameText;
        public TextMeshProUGUI SkillInfoText;

        [Header("Buttons")]
        public Button currentPushedButton; // 현재 누르고 있는 버튼
        public Button moveButton;
        public List<Button> skillButtons;
        public List<Button> itemButtons;

        List<UnitInfoUI> UnitsInfoList = new List<UnitInfoUI>();
        public UnitInfoUI thisTurnUnitInfo;

        RectTransform unitTurnIndicator;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Init_UnitInfoUI();
            Init_UnitTurnIndicator();

            foreach (var unit in BattleManager.GetUnit())
            {
                MakeUnitObject(unit);
                UpdateUnitObejct(unit);
            }

            SetNextTurn();
        }

        private void Update()
        {
            foreach (var unit in BattleManager.GetUnit())
            {
                UpdateUnitObejct(unit);
            }
        }

        public void MakeUnitObject(Unit unit)
        {
            // 미리 존재 여부 확인
            if (unitObjects.ContainsKey(unit) == true) 
            {
                Debug.LogError("이미 필드에 유닛 오브젝트가 존재합니다.");
            } 
            else
            {
                // 게임 오브젝트 생성
                GameObject gameObject = new GameObject(unit.Name);

                // 위치 지정
                gameObject.transform.position = new Vector3(unit.Position.x, unit.Position.y, -1);
                    
                // 이미지 컴포넌트 추가
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

                // 박스 콜라이더 컴포넌트 추가
                gameObject.AddComponent<BoxCollider2D>();

                // 이벤트 트리거 컴포넌트 추가
                EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

                // 스프라이트 설정
                spriteRenderer.sprite = unit.Sprite;

                // 이벤트 트리거 설정
                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                entry_PointerClick.callback.AddListener((data) =>
                {
                    UnitDescriptionUI.instance.Enable(unit);
                });
                eventTrigger.triggers.Add(entry_PointerClick);

                // 자식으로 슬라이더 추가
                Slider slider = Instantiate(HPBarPrefab, transform).GetComponent<Slider>();
                slider.maxValue = unit.MaximumHP;
                slider.minValue = 0;
                slider.value = unit.CurrentHP;

                // 딕셔너리에 오브젝트를 추가
                unitObjects.Add(unit, gameObject);
                hpBars.Add(unit, slider);
            }
        }

        public void UpdateUnitObejct(Unit unit)
        {
            // 유닛 state가 Idle이 아니고, 유닛오브젝트 애니메이터가 Idle이면
            if (unit.animationState != Unit.AnimationState.Idle &&
                unitObjects[unit].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                // 애니메이션 실행

            }

            if (new Vector3(unit.Position.x, unit.Position.y) != unitObjects[unit].transform.position)
            {
                // 이동 애니메이션 실행
                unitObjects[unit].transform.position = new Vector3(unit.Position.x, unit.Position.y, -1);
                hpBars[unit].transform.position = Camera.main.WorldToScreenPoint(
                    unitObjects[unit].transform.position + Vector3.up * 0.5f
                    );
            }

            if (unit.CurrentHP != hpBars[unit].value)
            {
                // hpBars 갱신
                hpBars[unit].value = unit.CurrentHP;
            }
        }

        /// <summary>
        /// 임시용 게임종료 버튼 이벤트.
        /// UnitManager의 EnemyUnit을 Clear한다.
        /// </summary>
        public void EndGame()
        {
            SceneLoader.GotoStage();
        }

        /// <summary>
        /// 파티원 유닛 정보창 생성
        /// </summary>
        void Init_UnitInfoUI()
        {
            for (int i = 0; i < GameManager.PartyUnits.Count; i++)
            {
                GameObject g = Instantiate(UnitInfoUIPrefab);

                g.transform.SetParent(unitsInfoPanel.transform);
                g.transform.SetAsFirstSibling();
                g.transform.localScale = Vector3.one;

                UnitInfoUI unitInfoUI = g.GetComponent<UnitInfoUI>();
                unitInfoUI.Init(); ;
                UnitsInfoList.Add(unitInfoUI);
                UpdateUnitInfo(GameManager.PartyUnits[i]);
            }
        }

        /// <summary>
        /// 유닛 턴 표시기 생성
        /// </summary>
        void Init_UnitTurnIndicator()
        {
            unitTurnIndicator = new GameObject().AddComponent<RectTransform>();
            unitTurnIndicator.SetParent(transform);
            unitTurnIndicator.sizeDelta = UnitInfoUIPrefab.GetComponent<RectTransform>().sizeDelta + new Vector2(10, 10);
            unitTurnIndicator.gameObject.AddComponent<Image>().color = Color.yellow;
            unitTurnIndicator.localScale = Vector3.one;
            unitTurnIndicator.gameObject.SetActive(false);
        }

        /// <summary>
        /// 현재 턴의 유닛 정보창에 테두리 표시
        /// </summary>
        /// <param name="unit">현재 턴 유닛</param>
        void SetUnitTurnIndicator(Unit unit)
        {
            int index = GameManager.PartyUnits.IndexOf(unit);
            unitTurnIndicator.SetParent(UnitsInfoList[index].transform);
            unitTurnIndicator.SetAsFirstSibling();
            unitTurnIndicator.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// 턴 전환 이벤트
        /// </summary>
        public void SetNextTurn()
        {
            Unit nextTurnUnit = BattleManager.GetNextTurnUnit();
            InitThisTurnPanel(nextTurnUnit);
        }

        /// <summary>
        /// 길찾기 알고리즘으로 유닛 이동.
        /// </summary>
        /// <param name="unit">이동 유닛</param>
        /// <param name="from">출발 위치</param>
        /// <param name="to">도착 위치</param>
        /// <returns></returns>
        IEnumerator MoveUnit(Unit unit, Vector2Int from, Vector2Int to)
        {
            List<Vector2Int> path = PathFind.PathFindAlgorithm(from, to);

            var waitForSeconds = new WaitForSeconds(0.1f);
            foreach (Vector2Int position in path)
            {
                UnitAction.Move(unit, position);
                // 화면 갱신 코드 필요.
                yield return waitForSeconds;
            }
        }

        /// <summary>
        /// 유닛 정보창 갱신.
        /// </summary>
        /// <param name="unit">갱신할 유닛</param>
        public void UpdateUnitInfo(Unit unit)
        {
            int index = GameManager.PartyUnits.IndexOf(unit);
            UnitsInfoList[index].Set(unit);
        }

        /// <summary>
        /// 현재 턴의 유닛정보를 가져와서 스킬 및 아이템 창을 초기화 한다.
        /// </summary>
        /// <param name="unit">현재 턴 유닛</param>
        public void InitThisTurnPanel(Unit unit)
        {
            currentPushedButton = null;
            //indicatorManager.DestoryAll();

            bool isPartyUnit = GameManager.PartyUnits.Contains(unit);
            if (isPartyUnit) // 파티원인가?
            {
                foreach (Button button in skillButtons) // 이전 스킬 이미지 제거.
                {
                    button.interactable = false;
                    button.GetComponent<Image>().sprite = null;
                }

                for (int i = 0; i < unit.Skills.Length; i++) //현재 턴 유닛의 스킬 이미지로 갱신.
                {
                    if (unit.Skills[i] != null && unit.Skills[i].Sprite != null)
                    {
                        skillButtons[i].GetComponent<Image>().sprite = unit.Skills[i].Sprite;
                    }
                }
                SetUnitTurnIndicator(unit); // 현재 턴의 유닛 정보창에 테두리 표시
                //UpdateThisTurnPanel(unit);
            }
            // 파티원일때만 UI 활성화.
            thisTurnUnitInfo.gameObject.SetActive(isPartyUnit);
            unitTurnIndicator.gameObject.SetActive(isPartyUnit);
        }
        /// <summary>
        /// thisTurnUnitInfo 패널의 버튼 기능 정의.
        /// </summary>
        /// <param name="unit">현재 턴 유닛</param>
        //public void UpdateThisTurnPanel(Unit unit)
        //{
        //    thisTurnUnitInfo.Set(unit);
        //    moveButton.interactable = false; // 이동 버튼 비활성화
        //    foreach (Button skillButton in skillButtons) // 스킬 버튼 비활성화
        //        skillButton.interactable = false;
        //    foreach (Button itemButton in itemButtons) // 스킬 버튼 비활성화
        //        itemButton.interactable = false;

        //    if (currentPushedButton == null) // 아무 버튼도 안눌린 기본 상태
        //    {
        //        if (unit.moveCount > 0) // 이동 가능한가?
        //        {
        //            moveButton.interactable = true;
        //            moveButton.onClick.RemoveAllListeners();

        //            moveButton.onClick.AddListener(() => // 이동 버튼을 눌렀을 때 작동하는 코드
        //            {
        //                indicatorManager.InitMainIndicator(unit.position, indicatorManager.mainTileIndicatorPrefab);
        //                //indicatorManager.AddIndicatorBoundary(unit.GetMovablePositions(), indicatorManager.tileIndicatorBoundaryPrefab);
        //                /*indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();

        //                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
        //                entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                entry_PointerClick.callback.AddListener((data) =>
        //                {
        //                    StartCoroutine(MoveUnit(unit, unit.unitPosition, indicatorManager.GetUnitPositionOnMainIndicator()));
        //                    indicatorManager.DestoryAll();

        //                    unit.moveCount--;
        //                    currentPushedButton = null;
        //                    UpdateThisTurnPanel(unit);
        //                });

        //                indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);

        //                currentPushedButton = moveButton;
        //                UpdateThisTurnPanel(unit);
        //                */
        //            });
        //        }

        //        for (int i = 0; i < unit.skills.Length; i++)
        //        {
        //            Skill skill = unit.skills[i];

        //            if (skill == null) continue;
        //            if (skill.currentReuseTime == 0 && unit.skillCount > 0) // 스킬이 사용가능한 조건
        //            {
        //                skillButtons[i].interactable = true;
        //                skillButtons[i].onClick.RemoveAllListeners();

        //                int temp = i;
                        
        //                skillButtons[i].onClick.AddListener(() => // 스킬 버튼을 눌렀을 때 작동하는 코드
        //                {
        //                    EventTrigger.Entry entry_PointerClick;

        //                    SkillInfoNameText.text = skill.name;
                            
        //                    SkillInfoText.text = skill.description;
        //                    SkillInfoInstance.SetActive(true);

        //                    /*if ((skill.target == Skill.Target.AnyTile || skill.target == Skill.Target.NoUnitTile)
        //                        && skill.domain == Skill.Domain.Fixed)
        //                    {
        //                        if (skill.GetPositionsInDomain(unit).Count != 0)
        //                        {
        //                            indicatorManager.InitMainIndicator(skill.GetPositionsInDomain(unit)[0], indicatorManager.mainTileIndicatorPrefab);
        //                            // 메인 인디케이터 생성
        //                            entry_PointerClick = new EventTrigger.Entry();
        //                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToTile(indicatorManager.GetTilesOnIndicator()); });
        //                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
        //                            // 인디케이터에 커스텀 클릭 트리거 설정
        //                        }
        //                        indicatorManager.AddSubIndicator(skill.GetRangePositions(), indicatorManager.subTileIndicatorPrefab);
        //                        // 서브 인디케이터 생성
        //                        indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
        //                        // 인디케이터 바운더리 생성
        //                        indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();
        //                        // 인디케이터 바운더리에 따라오기 엔터 트리거 설정
        //                    }
        //                    else if ((skill.target == Skill.Target.AnyTile || skill.target == Skill.Target.NoUnitTile)
        //                        && skill.domain == Skill.Domain.Rotate)
        //                    {
        //                        if (skill.GetPositionsInDomain(unit).Count != 0)
        //                        {
        //                            indicatorManager.InitMainIndicator(skill.GetPositionsInDomain(unit)[0], indicatorManager.mainTileIndicatorPrefab);
        //                            // 메인 인디케이터 생성
        //                            entry_PointerClick = new EventTrigger.Entry();
        //                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToTile(indicatorManager.GetTilesOnIndicator()); });
        //                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
        //                            // 인디케이터에 커스텀 클릭 트리거 설정
        //                        }
        //                        indicatorManager.AddSubIndicator(skill.GetRangePositions(), indicatorManager.subTileIndicatorPrefab);
        //                        // 서브 인디케이터 생성
        //                        indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
        //                        // 인디케이터 바운더리 생성

        //                        indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();
        //                        // 인디케이터 바운더리에 따라오기 엔터 트리거 설정
        //                        indicatorManager.SetRotateEnterTriggerOnIndicatorBoundary();
        //                        // 인디케이터 바운더리에 회전 엔터 트리거 설정
        //                    }
        //                    else if ((skill.target == Skill.Target.AnyTile || skill.target == Skill.Target.NoUnitTile)
        //                      && skill.domain == Skill.Domain.RandomOne)
        //                    {
        //                        if (skill.GetPositionsInDomain(unit).Count != 0)
        //                        {
        //                            indicatorManager.InitMainIndicator(skill.GetPositionsInDomain(unit)[0], indicatorManager.mainTileIndicatorPrefab);
        //                            // 메인 인디케이터 생성
        //                            entry_PointerClick = new EventTrigger.Entry();
        //                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToTile(indicatorManager.GetTilesOnIndicator()); });
        //                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
        //                            // 인디케이터에 커스텀 클릭 트리거 설정
        //                        }

        //                        indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.mainTileIndicatorPrefab);
        //                        // 인디케이터 바운더리 생성

        //                        indicatorManager.SetFollowEnterTriggerOnIndicatorBoundary();
        //                        // 인디케이터 바운더리에 따라오기 엔터 트리거 설정
        //                    }
        //                    else if ((skill.target == Skill.Target.AnyUnit || skill.target == Skill.Target.EnemyUnit
        //                        || skill.target == Skill.Target.FriendlyUnit || skill.target == Skill.Target.PartyUnit)
        //                        && skill.domain == Skill.Domain.RandomOne)
        //                    {
        //                        if (skill.GetUnitPositionsInDomain(unit).Count != 0)
        //                        {
        //                            indicatorManager.InitMainIndicator(skill.GetUnitPositionsInDomain(unit)[0], indicatorManager.mainUnitIndicatorPrefab);
        //                            // 메인 인디케이터 생성
        //                            entry_PointerClick = new EventTrigger.Entry();
        //                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToUnit(unit,indicatorManager.GetUnitsOnIndicator()); });
        //                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
        //                            // 인디케이터에 커스텀 클릭 트리거 설정
        //                        }
        //                        indicatorManager.AddIndicatorBoundary(skill.GetUnitPositionsInDomain(unit), indicatorManager.mainUnitIndicatorPrefab);
        //                        // 인디케이터 바운더리 생성
        //                        indicatorManager.SetEqualizeEnterTriggerOnIndicatorBoundary();
        //                        // 인디케이터 바운더리에 동일화 엔터 트리거 설정

        //                        indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
        //                        // 엔터 트리거 없는 바운더리 추가
        //                    }
        //                    else if ((skill.target == Skill.Target.AnyUnit || skill.target == Skill.Target.EnemyUnit
        //                        || skill.target == Skill.Target.FriendlyUnit || skill.target == Skill.Target.PartyUnit)
        //                        && skill.domain == Skill.Domain.SelectOne)
        //                    {
        //                        if (skill.GetUnitPositionsInDomain(unit).Count != 0)
        //                        {
        //                            indicatorManager.InitMainIndicator(skill.GetUnitPositionsInDomain(unit)[0], indicatorManager.mainUnitIndicatorPrefab);
        //                            // 메인 인디케이터 생성
        //                            entry_PointerClick = new EventTrigger.Entry();
        //                            entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                            entry_PointerClick.callback.AddListener((data) => { skill.UseSkillToUnit(unit, indicatorManager.GetUnitsOnIndicator()); });
        //                            indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
        //                            // 인디케이터에 커스텀 클릭 트리거 설정
        //                        }
        //                        indicatorManager.AddIndicatorBoundary(skill.GetUnitPositionsInDomain(unit), indicatorManager.unitIndicatorBoundaryPrefab);
        //                        // 인디케이터 바운더리 생성
        //                        indicatorManager.SetEqualizeEnterTriggerOnIndicatorBoundary();
        //                        // 인디케이터 바운더리에 동일화 엔터 트리거 설정

        //                        indicatorManager.AddIndicatorBoundary(skill.GetPositionsInDomain(unit), indicatorManager.tileIndicatorBoundaryPrefab);
        //                        // 엔터 트리거 없는 바운더리 추가
        //                    }*/

        //                    entry_PointerClick = new EventTrigger.Entry();
        //                    entry_PointerClick.eventID = EventTriggerType.PointerClick;
        //                    entry_PointerClick.callback.AddListener((data) =>
        //                    {
        //                        indicatorManager.DestoryAll();
        //                        unit.moveCount--;
        //                        unit.skillCount--;
        //                        currentPushedButton = null;
        //                        UpdateThisTurnPanel(unit);
        //                    });
        //                    indicatorManager.SetCustomClickTriggerOnIndicator(entry_PointerClick);
        //                    // 공통 커스텀 클릭 트리거 설정

        //                    currentPushedButton = skillButtons[temp];
        //                    UpdateThisTurnPanel(unit);
        //                });
                            
        //            }
        //        }

        //        for (int i = 0; i < unit.items.Length; i++)
        //        {
        //            //아이템 버튼
        //        }
        //    }
        //    else // 버튼 눌린 상태, 타 버튼 비활성화, 다시 누르면 타일 표시기들 제거
        //    {
        //        currentPushedButton.interactable = true; // 취소버튼만 활성화

        //        currentPushedButton.onClick.RemoveAllListeners();
        //        currentPushedButton.onClick.AddListener(() =>
        //        {
        //            indicatorManager.DestoryAll();
        //            SkillInfoInstance.SetActive(false);

        //            currentPushedButton = null;
        //            UpdateThisTurnPanel(unit);
        //        });
        //    }
        //}
    }
}