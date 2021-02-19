using Common;
using Model.Managers;
using Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Model.Skills;

namespace UI.Battle
{
    public class BattleUI : MonoBehaviour
    {
        public static BattleUI instance;

        [SerializeField]
        private Dictionary<Unit, GameObject> unitObjects = new Dictionary<Unit, GameObject>();
        [SerializeField]
        private Dictionary<Unit, Slider> hpBars = new Dictionary<Unit, Slider>();
        [SerializeField]
        private GameObject hPBarPrefab;
        [SerializeField]
        private UnitInfoUI thisTurnUnitInfo;
        [SerializeField]
        private UnitInfoUI otherUnitInfo;
        [SerializeField]
        private Button turnEndButton;
        public UnitInfoUI ThisTurnUnitInfo { get => thisTurnUnitInfo; set => thisTurnUnitInfo = value; }

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
            instance = this;
            hPBarPrefab = Resources.Load<GameObject>("Prefabs/UI/Battle/HP_BAR");
            thisTurnUnitInfo = transform.Find("Panel/ThisTurnUnitInfo").GetComponent<UnitInfoUI>();
            otherUnitInfo = transform.Find("Panel/OtherUnitInfo").GetComponent<UnitInfoUI>();
            turnEndButton = transform.Find("Panel/TurnEndButton").GetComponent<Button>();
        }

        private void Start()
        {
            turnEndButton.onClick.AddListener(() =>
            {
                TurnEnd();
            });

            foreach (var unit in BattleManager.GetUnit())
            {
                MakeUnitObject(unit);
                UpdateUnitObejct(unit);
            }

            Unit nextUnit = BattleManager.GetNextTurnUnit();
            BattleManager.SetNextTurnUnit(nextUnit);
            TurnStart();
        }

        private void Update()
        {
            foreach (var unit in BattleManager.GetUnit())
                if (unit.IsModified)
                {
                    UpdateUnitObejct(unit);

                    if (unit == thisTurnUnitInfo.Unit)
                        thisTurnUnitInfo.UpdateUnitInfo();
                    if (unit == otherUnitInfo.Unit)
                        otherUnitInfo.UpdateUnitInfo();

                    unit.IsModified = false;
                }
        }

        public void TurnStart()
        {
            Unit thisTurnUnit = BattleManager.instance.thisTurnUnit;

            // 턴시작시 유닛 값들 초기화
            thisTurnUnit.MoveCount = 1;
            thisTurnUnit.SkillCount = 1;
            thisTurnUnit.ItemCount = 1;

            // 턴 시작시 스킬쿨 줄어듬
            foreach (var skill in thisTurnUnit.Skills)
                if (skill != null && skill.currentReuseTime != 0)
                    skill.currentReuseTime--;

            // 유닛정보창 초기화
            if (thisTurnUnit.Category == Category.Enemy || thisTurnUnit.Category == Category.Boss)
                thisTurnUnitInfo.SetUnitInfo(thisTurnUnit, false);
            else
                thisTurnUnitInfo.SetUnitInfo(thisTurnUnit, true);

            // 뒤에서부터 돌면 중간에 삭제해도 문제 없음.
            for (int i = thisTurnUnit.StateEffects.Count - 1; i >= 0; i--)
                thisTurnUnit.StateEffects[i].OnTurnStart();

            // AI라면 자동 행동 실행
            if (thisTurnUnit.Category == Category.Enemy || thisTurnUnit.Category == Category.Boss)
            {
                AI.Action action = AI.GetAction(thisTurnUnit);

                if (action != null)
                    action.Invoke();
            }
        }

        public void TurnEnd()
        {
            IndicatorUI.HideTileIndicator();
            thisTurnUnitInfo.CurrentPushedButton = null;

            Unit thisTurnUnit = BattleManager.instance.thisTurnUnit;

            // 턴 종료 효과 처리
            for (int i = thisTurnUnit.StateEffects.Count - 1; i >= 0; i--)
                thisTurnUnit.StateEffects[i].OnTurnEnd();

            // 다음 턴의 유닛을 받아 시작한다.
            Unit nextUnit = BattleManager.GetNextTurnUnit();
            BattleManager.SetNextTurnUnit(nextUnit);

            TurnStart();
        }

        public void Win()
        {

        }

        public void Defeat()
        {

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
                gameObject.transform.position = new Vector3(unit.Position.x, unit.Position.y, 0);

                // 이미지 컴포넌트 추가
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

                // 박스 콜라이더 컴포넌트 추가
                BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
                boxCollider2D.size = new Vector2(1, 1);
//                boxCollider2D.isTrigger = true;

                // 이벤트 트리거 컴포넌트 추가
                EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

                // 스프라이트 설정
                spriteRenderer.sprite = unit.Sprite;

                // 이벤트 트리거 설정
                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                entry_PointerClick.callback.AddListener((data) =>
                {
                    otherUnitInfo.SetUnitInfo(unit, false);
                });
                eventTrigger.triggers.Add(entry_PointerClick);

                // BattleUI 자식으로 슬라이더 추가
                Slider slider = Instantiate(hPBarPrefab, transform).GetComponent<Slider>();
                slider.name = $"{unit.Name} HPBAR"; 
                slider.maxValue = unit.MaximumHP;
                slider.minValue = 0;
                slider.value = unit.CurrentHP;
                slider.transform.position = Camera.main.WorldToScreenPoint(
                    new Vector3(unit.Position.x, unit.Position.y) + Vector3.up * 0.5f
                );

                // 딕셔너리에 오브젝트를 추가
                unitObjects.Add(unit, gameObject);
                hpBars.Add(unit, slider);
            }
        }

        public void UpdateUnitObejct(Unit unit)
        {
            // 유닛 state가 Idle이 아니고, 유닛오브젝트 애니메이터가 Idle이면
            if (unit.animationState != Unit.AnimationState.Idle /*&&
                unitObjects[unit].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")*/)
            {
                unit.animationState = Unit.AnimationState.Idle;
                // 애니메이션 실행

            }

            if (new Vector3(unit.Position.x, unit.Position.y) != unitObjects[unit].transform.position)
            {
                // 이동 애니메이션 실행                
                unitObjects[unit].transform.position = new Vector3(unit.Position.x, unit.Position.y, 0);

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
    }
}