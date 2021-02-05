using Model.Managers;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Battle
{
    public class IndicatorUI : MonoBehaviour
    {
        public static IndicatorUI instance;

        private void Awake()
        {
            instance = this;
        }

        [SerializeField]
        private static Transform mainIndicator; // 메인 인디케이터의 위치, 서브 인디케이터는 자식으로 생성된다.

        public static Transform MainIndicator {
            get
            {
                if (mainIndicator == null)
                {
                    mainIndicator = Instantiate(instance.mainTileIndicatorPrefab).transform;
                    mainIndicator.name = "MainIndicator";
                    mainIndicator.position = Vector3.back * 2;
                    mainIndicator.gameObject.SetActive(false);
                }

                return mainIndicator;
            }

            set => mainIndicator = value; 
        }

        [SerializeField]
        private static Transform indicatorBoundary; // 인디케이터 경계의 위치, 각종 인디케이터 경계가 자식으로 생성된다.

        public static Transform IndicatorBoundary
        {
            get
            {
                if (indicatorBoundary == null)
                {
                    indicatorBoundary = new GameObject("IndicatorBoundary").transform;
                    indicatorBoundary.position += Vector3.back;
                }
                return indicatorBoundary;
            }
            set => indicatorBoundary = value;
        }

        [Header("Tile Indicator")]
        [SerializeField]
        public GameObject mainTileIndicatorPrefab; // 메인 타일 인디케이터
        [SerializeField]
        public GameObject subTileIndicatorPrefab;
        [SerializeField]
        public GameObject tileIndicatorBoundaryPrefab;

        public static List<Vector2Int> GetPositionsOnIndicators()
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            positions.Add(GetPositionOnMainIndicator()); // 메인 인디케이터 위치 추가

            for (int i = 0; i < MainIndicator.childCount; i++)
            {
                // 서브 인디케이터 위치들 추가
                Vector3 position = MainIndicator.GetChild(i).transform.position;
                positions.Add(new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)));
            }

            return positions;
        }

        public static Vector2Int GetPositionOnMainIndicator()
        {
            Vector3 position = MainIndicator.position;
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@서브 인디케이터 추가

        public static void AddSubIndicator(List<Vector2Int> positions, GameObject prefab)
        {
            foreach (var position in positions)
                AddSubIndicator(position, prefab);
        }

        public static void AddSubIndicator(Vector2Int position, GameObject prefab)
        {
            GameObject child = Instantiate(prefab, MainIndicator);
            child.transform.localPosition = new Vector3(position.x, position.y, 0);
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@유닛 인디케이터 경계

        public static void AddIndicatorBoundary(Vector2Int position, GameObject prefab)
        {
            Transform transform = Instantiate(prefab, IndicatorBoundary).transform;
            transform.localPosition = new Vector3(position.x, position.y, 0);
        }

        public static void AddIndicatorBoundary(List<Vector2Int> positions, GameObject prefab)
        {
            foreach (var position in positions)
            {
                AddIndicatorBoundary(position, prefab);
            }
        }

        public static void SetCustomClickTriggerOnIndicator(EventTrigger.Entry entryPointerClick)
        {
            if (MainIndicator.gameObject.GetComponent<EventTrigger>() == null)
                MainIndicator.gameObject.AddComponent<EventTrigger>();
            if (MainIndicator.gameObject.GetComponent<BoxCollider2D>() == null)
                MainIndicator.gameObject.AddComponent<BoxCollider2D>();

            EventTrigger eventTrigger = MainIndicator.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(entryPointerClick);
        }

        /// <summary>
        /// 인디케이터 바운더리에 회전 엔터 트리거를 추가한다.
        /// </summary>
        public void SetRotateEnterTriggerOnIndicatorBoundary()
        {
            Transform thisTurnUnit = BattleUI.instance.unitObjects[BattleManager.instance.thisTurnUnit].transform;

            for (int i = 0; i < IndicatorBoundary.childCount; i++)
            {
                Transform child = IndicatorBoundary.GetChild(i);

                if (child.GetComponent<EventTrigger>() == null)
                    child.gameObject.AddComponent<EventTrigger>();
                if (child.GetComponent<BoxCollider2D>() == null)
                    child.gameObject.AddComponent<BoxCollider2D>();

                EventTrigger eventTrigger = child.GetComponent<EventTrigger>();
                EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
                entryPointerEnter.eventID = EventTriggerType.PointerEnter;

                entryPointerEnter.callback.AddListener((data) =>
                {
                    if (MainIndicator.gameObject.activeSelf == false)
                        MainIndicator.gameObject.SetActive(true);

                    int x = Mathf.RoundToInt(child.position.x - thisTurnUnit.position.x);
                    int y = Mathf.RoundToInt(child.position.y - thisTurnUnit.position.y);
                    Vector3 vector = new Vector3(x, y, 0);
                    vector.Normalize();

                    float angle;
                    if (x < 0)
                        angle = -Mathf.Rad2Deg * Mathf.Acos(vector.y);
                    else
                        angle = Mathf.Rad2Deg * Mathf.Acos(vector.y);

                    MainIndicator.rotation = Quaternion.Euler(0, 0, -angle);
                });

                //child.position += Vector3.back;
                eventTrigger.triggers.Add(entryPointerEnter);
            }
        }

        /// <summary>
        /// 인디케이터 바운더리에 사이즈와 위치를 똑같게하는 엔터 트리거를 추가한다.
        /// </summary>
        public static void SetFollowEnterTriggerOnIndicatorBoundary()
        {
            if (IndicatorBoundary == null)
            {
                Debug.LogWarning("인디케이터 바운더리가 존재하지 않습니다.");
                return;
            }

            for (int i = 0; i < IndicatorBoundary.childCount; i++)
            {
                Transform child = IndicatorBoundary.GetChild(i);

                if (child.GetComponent<EventTrigger>() == null)
                    child.gameObject.AddComponent<EventTrigger>();
                if (child.GetComponent<BoxCollider2D>() == null)
                    child.gameObject.AddComponent<BoxCollider2D>();

                EventTrigger eventTrigger = child.GetComponent<EventTrigger>();
                EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
                entryPointerEnter.eventID = EventTriggerType.PointerEnter;

                entryPointerEnter.callback.AddListener((data) =>
                {
                    if (MainIndicator.gameObject.activeSelf == false)
                        MainIndicator.gameObject.SetActive(true);

                    MainIndicator.position = child.position + Vector3.back;
                    // MainIndicator.localScale = child.localScale; 사이즈 1*1라 필요가 없음 이제.
                });

                child.position += Vector3.back;
                eventTrigger.triggers.Add(entryPointerEnter);
            }
        }

        public void SetCustomEnterTriggerOnIndicatorBoundary(EventTrigger.Entry entryPointerEnter)
        {
            for (int i = 0; i < IndicatorBoundary.childCount; i++)
            {
                if (IndicatorBoundary.GetChild(i).GetComponent<EventTrigger>() == null)
                    IndicatorBoundary.GetChild(i).gameObject.AddComponent<EventTrigger>();

                EventTrigger eventTrigger = IndicatorBoundary.GetChild(i).GetComponent<EventTrigger>();
                eventTrigger.triggers.Add(entryPointerEnter);
            }
        }

        public static void DestoryAll()
        {
            if (MainIndicator != null)
            {
                Destroy(MainIndicator.gameObject);
                MainIndicator = null;
            }

            if (IndicatorBoundary != null)
            {
                Destroy(IndicatorBoundary.gameObject);
                IndicatorBoundary = null;
            }
        }
    }
}