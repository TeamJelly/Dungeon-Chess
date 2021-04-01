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
        private GameObject hPBarPrefab;

        public UnitInfoView ThisTurnUnitInfo { get; set; }
        public UnitInfoView OtherUnitInfo { get; set; }

        public Button TurnEndButton { get; private set; }
        public Dictionary<Unit, GameObject> UnitObjects { get; } = new Dictionary<Unit, GameObject>();
        public Dictionary<Unit, GameObject> HPBars { get; } = new Dictionary<Unit, GameObject>();
        private void Awake()
        {
            hPBarPrefab = Resources.Load<GameObject>("Prefabs/UI/Battle/HP_BAR");
            ThisTurnUnitInfo = transform.Find("Panel/ThisTurnUnitInfo").GetComponent<UnitInfoView>();
            OtherUnitInfo = transform.Find("Panel/OtherUnitInfo").GetComponent<UnitInfoView>();
            OtherUnitInfo.gameObject.SetActive(false);
            TurnEndButton = transform.Find("Panel/TurnEndButton").GetComponent<Button>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                TurnEndButton.onClick.Invoke();
        }

        public void SetTurnEndButton(UnityAction action)
        {
            TurnEndButton.onClick.RemoveAllListeners();
            TurnEndButton.onClick.AddListener(action);
        }
        public void SetTurnUnitPanel(Unit unit)
        {
            if (unit.Category != Category.Party)
                ThisTurnUnitInfo.SetUnitInfo(unit, false);
            else
                ThisTurnUnitInfo.SetUnitInfo(unit, true);
        }

        public void MakeUnitObject(Unit unit)
        {
            // 미리 존재 여부 확인
            if (UnitObjects.ContainsKey(unit) == true)
            {
                Debug.LogError("이미 필드에 유닛 오브젝트가 존재합니다.");
                return;
            }

            // 게임 오브젝트 생성
            GameObject newObj = new GameObject(unit.Name);

            // 위치 지정
            newObj.transform.position = new Vector3(unit.Position.x, unit.Position.y, 0);

            // 박스 콜라이더 컴포넌트 추가
            BoxCollider2D boxCollider2D = newObj.AddComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(1, 1);

            // 이벤트 트리거 컴포넌트 추가
            EventTrigger eventTrigger = newObj.AddComponent<EventTrigger>();

            // 스프라이터 랜더러 추가
            SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();

            // 애니메이터 추가
            Animator animator = newObj.AddComponent<Animator>();
            animator.runtimeAnimatorController = unit.Animator;

            // 이벤트 트리거 설정
            EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
            entry_PointerClick.eventID = EventTriggerType.PointerClick;
            entry_PointerClick.callback.AddListener((data) =>
            {
                OtherUnitInfo.gameObject.SetActive(true);
                OtherUnitInfo.SetUnitInfo(unit, false);
            });
            eventTrigger.triggers.Add(entry_PointerClick);
            UnitObjects.Add(unit, newObj);

            // HP 바 생성
            HPBar newHPBar = Instantiate(hPBarPrefab, ViewManager.instance.MainPanel).GetComponent<HPBar>();
            newHPBar.Init(unit);
            HPBars.Add(unit, newHPBar.gameObject);

            //유닛 오브젝트 상호작용 콜백 등록
            unit.OnPositionChanged.AddListener((v) =>
            {
                Vector3 w = new Vector3(v.x, v.y);
                newObj.transform.position = w;
                newHPBar.SetPosition(w);
            });
            unit.OnCurrentHP.changed.AddListener(newHPBar.SetValue);

            //최초 갱신
            newHPBar.SetValue(unit.CurrentHP);
            unit.OnPositionChanged.Invoke(unit.Position);
        }

        public void DestroyUnitObject(Unit unit)
        {
            GameObject unitObj = UnitObjects[unit];
            UnitObjects.Remove(unit);
            Destroy(unitObj);

            unitObj = HPBars[unit];
            HPBars.Remove(unit);
            Destroy(unitObj);

            //AgilityViewer.instance.DestroyObject(unit);



        }
    }
}