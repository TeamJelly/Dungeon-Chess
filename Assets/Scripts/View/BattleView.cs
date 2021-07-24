using Model;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Battle;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View.UI;

namespace View
{
    public class BattleView : MonoBehaviour
    {
        private GameObject hPBarPrefab;

        [SerializeField]
        private Image currentUnitPortrait;
        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private Button turnEndButton;
        [SerializeField]
        private UnitControlView unitControlUI;
        static BattleView instance;
        // public UnitInfoView ThisTurnUnitInfo;
        // public UnitInfoView OtherUnitInfo { get; set; }

        private Dictionary<Unit, GameObject> unitObjects =   new Dictionary<Unit, GameObject>();
        private Dictionary<Unit, HPBar> hpBars =   new Dictionary<Unit, HPBar>();
        private Dictionary<Obtainable, GameObject> obtainableObjects =  new Dictionary<Obtainable, GameObject>();
        public static Button TurnEndButton => instance.turnEndButton;
        public static Image CurrentUnitPortrait => instance.currentUnitPortrait;
        public static GameObject MainPanel => instance.mainPanel;
        public static UnitControlView UnitControlUI => instance.unitControlUI;
        public static Dictionary<Unit, GameObject> UnitObjects => instance.unitObjects;
        public static Dictionary<Unit, HPBar> HPBars => instance.hpBars;
        public static Dictionary<Obtainable, GameObject> ObtainableObjects => instance.obtainableObjects;

        private void Awake()
        {
            instance = this;

            hPBarPrefab = Resources.Load<GameObject>("Prefabs/UI/HP_BAR");
            //ThisTurnUnitInfo = transform.Find("Panel/ThisTurnUnitInfo").GetComponent<UnitInfoView>();
            //OtherUnitInfo = transform.Find("Panel/OtherUnitInfo").GetComponent<UnitInfoView>();
            //OtherUnitInfo.gameObject.SetActive(false);
            turnEndButton = transform.Find("MainPanel/TurnEndButton").GetComponent<Button>();
            unitControlUI = GetComponent<UnitControlView>();

            TurnEndButton.gameObject.SetActive(false);
            UnitControlUI.panel.SetActive(false);
        }
        /// <summary>
        /// 비전투시에는 리더 유닛만 움직일 수 있음
        /// </summary>
        /// <returns></returns>
        public static IEnumerator SetNonBattleMode()
        {
            BattleManager.instance.thisTurnUnit = null;
            GameManager.InBattle = false;
            TurnEndButton.gameObject.SetActive(false);
            UnitControlUI.panel.SetActive(false);
            if (GameManager.LeaderUnit == null) GameManager.LeaderUnit = GameManager.PartyUnits[0];
            Common.Command.Summon(GameManager.LeaderUnit, GameManager.LeaderUnit.Position);
            IEnumerator coroutine = GameManager.LeaderUnit.MoveSkill.Use(GameManager.LeaderUnit, Vector2Int.zero);
            while (true)
            {
                if(Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.one * 0.5f;
                    Vector2Int destination = new Vector2Int(Mathf.Clamp((int)mousepos.x, 0, 15), Mathf.Clamp((int)mousepos.y, 0, 15));

                    // 리더 유닛이 해당 타일에 위치가능하다면
                    if (FieldManager.GetTile(destination).IsPositionable(GameManager.LeaderUnit))
                    {
                        //리더 유닛 이동 코루틴 실행. 기존 실행되던 코루틴은 정지.
                        instance.StopCoroutine(coroutine);
                        coroutine = GameManager.LeaderUnit.MoveSkill.Use(GameManager.LeaderUnit, destination);
                        instance.StartCoroutine(coroutine);
                    }
                }
                yield return null;
            }
        }

        public static void SummonPartyUnits(int index = 0)
        {
            Unit unit = GameManager.PartyUnits[index];

            CurrentUnitPortrait.sprite = unit.Sprite;

            IndicatorView.ShowTileIndicator(FieldManager.instance.GetStairAroundPosition(),
                (Vector2Int position) =>
                {
                    Common.Command.Summon(unit, position);
                    index++;

                    if (index == GameManager.PartyUnits.Count)
                    {
                        IndicatorView.HideTileIndicator();
                        BattleController.instance.NextTurnStart();
                    }
                    // 전부 소환할때까지 재귀로 돈다.
                    else
                        SummonPartyUnits(index);
                }
            );
        }

        private void Update()
        {
            foreach(Unit unit in HPBars.Keys)
            {
                Vector3 unitPos = new Vector3(unit.Position.x, unit.Position.y);
                HPBars[unit].SetPosition(unitPos);
            }
            if (Input.GetKeyDown(KeyCode.Space))
                TurnEndButton.onClick.Invoke();
        }

        public static void SetTurnEndButton(UnityAction action)
        {
            TurnEndButton.onClick.RemoveAllListeners();
            TurnEndButton.onClick.AddListener(action);
        }

        public static void SetTurnUnitPanel(Unit unit)
        {
            CurrentUnitPortrait.sprite = unit.Sprite;

            TurnEndButton.gameObject.SetActive(true);
            UnitControlUI.panel.SetActive(true);

            UnitControlUI?.UpdateUI(unit);

            //if (unit.Category != Category.Party)
            //    ThisTurnUnitInfo?.SetUnitInfo(unit, false);
            //else
            //    ThisTurnUnitInfo?.SetUnitInfo(unit, true);
        }

        /// <summary>
        /// 유닛을 필드에 오브젝트로 생성시키는 함수
        /// 생성하면서 이벤트 콜벡을 할당해줘야 한다.
        /// </summary>
        /// <param name="unit"></param>
        public static void MakeUnitObject(Unit unit)
        {
            // 미리 존재 여부 확인
            if (UnitObjects.ContainsKey(unit) == true)
            {
                Debug.LogError($"이미 필드에 유닛({unit.Name}) 오브젝트가 존재합니다.");
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
//                OtherUnitInfo.gameObject.SetActive(true);
//                OtherUnitInfo.SetUnitInfo(unit, false);
            });
            eventTrigger.triggers.Add(entry_PointerClick);
            UnitObjects.Add(unit, newObj);

            // HP 바 생성
            HPBar newHPBar = Instantiate(instance.hPBarPrefab, MainPanel.transform).GetComponent<HPBar>();
            newHPBar.Init(unit);
            HPBars.Add(unit, newHPBar);

            //유닛 오브젝트 상호작용 콜백 등록
            unit.OnPosition.after.AddListener(MoveObject);
            unit.OnCurHP.after.AddListener(newHPBar.SetValue);

            //최초 갱신
            //newHPBar.SetValue(unit.CurHP);
            int tempHP = unit.CurHP;
            Vector2Int tempPosition = unit.Position;

            unit.OnPosition.after.Invoke(tempPosition);
            unit.OnCurHP.after.Invoke(tempHP);
        }

        public static Vector2Int MoveObject(Vector2Int v)
        {
            foreach(Unit unit in BattleManager.instance.AllUnits)
            {
                if(unit.Position == v)
                {
                    Vector3 w = new Vector3(v.x, v.y);
                    UnitObjects[unit].transform.position = w;
                    HPBars[unit].SetPosition(w);
                    return v;
                }
            }
            return v;
        }

        public static void MakeObtainableObject(Obtainable ob, Vector2Int pos)
        {

            // 게임 오브젝트 생성
            GameObject obObj = new GameObject();
            // 위치 지정
            obObj.transform.position = new Vector3(pos.x, pos.y, -0.1f);

            // 스프라이터 랜더러 추가
            SpriteRenderer spriteRenderer = obObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = ob.Sprite;

            ObtainableObjects.Add(ob, obObj);
        }
        public static void DestroyObtainableObject(Obtainable ob)
        {
            GameObject obObj = ObtainableObjects[ob];
            ObtainableObjects.Remove(ob);
            Destroy(obObj);
        }
        public static void DestroyUnitObject(Unit unit)
        {
            GameObject unitObj = UnitObjects[unit];
            UnitObjects.Remove(unit);
            Destroy(unitObj);

            HPBar hpBar = HPBars[unit];
            HPBars.Remove(unit);
            Destroy(hpBar.gameObject);

            unit.OnPosition.after.RemoveAllListeners();
            unit.OnCurHP.after.RemoveAllListeners();

            // AgilityViewer.instance.DestroyObject(unit);
        }
    }
}