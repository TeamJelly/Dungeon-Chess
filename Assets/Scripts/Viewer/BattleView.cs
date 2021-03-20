using Model;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UI.Battle;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<Unit, Transform> unitObjects = new Dictionary<Unit, Transform>();

        [SerializeField]
        private Dictionary<Unit, HPBar> hpBars = new Dictionary<Unit, HPBar>();

        [SerializeField]
        private GameObject hPBarPrefab;
        private UnitInfoView thisTurnUnitInfo;
        private UnitInfoView otherUnitInfo;
        private Button turnEndButton;

        public UnitInfoView ThisTurnUnitInfo { get => thisTurnUnitInfo; set => thisTurnUnitInfo = value; }
        public UnitInfoView OtherUnitInfo { get => otherUnitInfo; set => otherUnitInfo = value; }

        public Button TurnEndButton => turnEndButton;
        public Dictionary<Unit, Transform> UnitObjects { get => unitObjects; set => unitObjects = value; }
        public Dictionary<Unit, HPBar> HpBars { get => hpBars; set => hpBars = value; }
        private void Awake()
        {
            hPBarPrefab = Resources.Load<GameObject>("Prefabs/UI/Battle/HP_BAR");
            thisTurnUnitInfo = transform.Find("Panel/ThisTurnUnitInfo").GetComponent<UnitInfoView>();
            otherUnitInfo = transform.Find("Panel/OtherUnitInfo").GetComponent<UnitInfoView>();
            otherUnitInfo.gameObject.SetActive(false);
            turnEndButton = transform.Find("Panel/TurnEndButton").GetComponent<Button>();
        }


        //추후 콜백으로 수정
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                turnEndButton.onClick.Invoke();

            foreach (var unit in BattleManager.GetUnit())
                if (unit.IsModified)
                {
                    UpdateUnitObejct(unit);

                    if (unit == ThisTurnUnitInfo.Unit)
                        ThisTurnUnitInfo.UpdateUnitInfo();
                    if (unit == OtherUnitInfo.Unit)
                        OtherUnitInfo.UpdateUnitInfo();

                    unit.IsModified = false;
                }
        }

        //콜백으로 수정하기.
        private void UpdateUnitObejct(Unit unit)
        {
            // 유닛 state가 Idle이 아니고, 유닛오브젝트 애니메이터가 Idle이면
            if (unit.animationState != Unit.AnimationState.Idle /*&&
                unitObjects[unit].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")*/)
            {
                unit.animationState = Unit.AnimationState.Idle;
                // 애니메이션 실행
            }
            Viewer.battle.MoveUnitObject(unit);
            Viewer.battle.HpBars[unit].SetValue(unit.CurrentHP);
        }

        public void SetTurnEndButton(UnityAction action)
        {
            turnEndButton.onClick.RemoveAllListeners();
            turnEndButton.onClick.AddListener(action);
        }
        public void SetTurnUnitPanel(Unit unit)
        {
            if (unit.Category != Category.Party)
                thisTurnUnitInfo.SetUnitInfo(unit, false);
            else
                thisTurnUnitInfo.SetUnitInfo(unit, true);
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

                // 박스 콜라이더 컴포넌트 추가
                BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
                boxCollider2D.size = new Vector2(1, 1);

                // 이벤트 트리거 컴포넌트 추가
                EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

                // 스프라이터 랜더러 추가
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

                // 애니메이터 추가
                Animator animator = gameObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = unit.Animator;

                // 이벤트 트리거 설정
                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                entry_PointerClick.callback.AddListener((data) =>
                {
                    otherUnitInfo.gameObject.SetActive(true);
                    otherUnitInfo.SetUnitInfo(unit, false);
                });
                eventTrigger.triggers.Add(entry_PointerClick);

                //HP 바 생성
                AddHPBar(unit);
                UnitObjects.Add(unit, gameObject.transform);
                //최초 갱신. 추후 콜백으로 바뀌면 검토하기
                UpdateUnitObejct(unit);
            }
        }

        private void AddHPBar(Unit unit)
        {
            HPBar newHPBar = Instantiate(hPBarPrefab, Viewer.instance.MainPanel).GetComponent<HPBar>();
            newHPBar.Init(unit);
            hpBars.Add(unit, newHPBar);
        }

        private void MoveUnitObject(Unit unit)
        {
            Vector3 w = new Vector3(unit.Position.x, unit.Position.y);
            unitObjects[unit].transform.position = w;
            hpBars[unit].SetPosition(w);
        }



    }
}