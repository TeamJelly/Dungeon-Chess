using Model.Managers;
using Model.Models;
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
        Transform Indicator; // 메인 인디케이터의 위치, 서브 인디케이터는 자식으로 생성된다.

        [SerializeField]
        Transform IndicatorBoundary; // 인디케이터 경계의 위치, 각종 인디케이터 경계가 자식으로 생성된다.

        [Header("Tile Indicator")]
        [SerializeField]
        public GameObject mainTileIndicatorPrefab; // 메인 타일 인디케이터
        [SerializeField]
        public GameObject subTileIndicatorPrefab;
        [SerializeField]
        public GameObject tileIndicatorBoundaryPrefab;

        [Header("Unit Indicator")]
        [SerializeField]
        public GameObject mainUnitIndicatorPrefab; // 메인 유닛 인디케이터
                                                   //    [SerializeField]
                                                   //    public GameObject subUnitIndicatorPrefab;
        [SerializeField]
        public GameObject unitIndicatorBoundaryPrefab;


        /// <summary>
        /// 현재 유닛 인디케이터로부터 유닛을 받는다.
        /// </summary>
        /// <returns> 유닛들 리스트 </returns>
        public List<Unit> GetUnitsOnIndicator()
        {
            List<Unit> units = new List<Unit>();

            foreach (var tile in GetTilesOnIndicator())
            {
                Unit temp = tile.GetUnit();
                if (units.Contains(temp))
                    continue;
                else
                    units.Add(temp);
            }

            return units;
        }

        /// <summary>
        /// 현재 타일 인디케이터로부터 타일을 받는다.
        /// </summary>
        /// <returns> 타일들 리스트 </returns>
        public List<Tile> GetTilesOnIndicator()
        {
            List<Tile> tiles = new List<Tile>();

            foreach (var position in GetPositionsOnIndicators())
            {
                tiles.Add(BattleManager.instance.GetTile(position));
            }

            return tiles;
        }

        public void debug()
        {
            foreach (var item in GetPositionsOnIndicators())
                Debug.LogError(item);
        }

        public List<Vector2Int> GetPositionsOnIndicators()
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (var unitPosition in GetUnitPositionsOnIndicators())
            {
                foreach (var position in unitPosition.GetPositions())
                {
                    if (positions.Contains(position))
                        continue;
                    else
                        positions.Add(position);
                }
            }

            return positions;
        }

        /// <summary>
        /// 인디케이터로부터 유닛 포지션을 받는다.
        /// </summary>
        /// <returns> 유닛 포지션 리스트 </returns>
        public List<UnitPosition> GetUnitPositionsOnIndicators()
        {
            List<UnitPosition> unitPositions = new List<UnitPosition>();

            unitPositions.Add(GetUnitPositionOnMainIndicator()); // 메인 인디케이터 위치 추가

            for (int i = 0; i < Indicator.childCount; i++)
            {
                // 서브 인디케이터 위치들 추가
                unitPositions.Add(UnitPosition.TransformToUnitPosition(Indicator.GetChild(i).transform));
            }

            return unitPositions;
        }

        public UnitPosition GetUnitPositionOnMainIndicator()
        {
            return UnitPosition.TransformToUnitPosition(Indicator.transform);
        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitPosition"></param>
        public void InitMainIndicator(UnitPosition unitPosition, GameObject prefab)
        {
            DestoryAll();
            Indicator = Instantiate(prefab).transform;
            Indicator.name = "MainIndicator";
            unitPosition.SetTransform(Indicator);
            Indicator.position += Vector3.back * 2;
            Indicator.gameObject.SetActive(false);
        }

        public void InitMainIndicator(Vector2Int position, GameObject prefab)
        {
            InitMainIndicator(new UnitPosition(position), prefab);
        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@서브 인디케이터 추가
        /// <summary>
        /// 서브 타일 인디케이터를 인디케이터의 자식으로 만들어 더한다.
        /// </summary>
        /// <param name="unitPosition">유닛 포지션</param>
        /// <param name="prefab">만들 위치</param>
        /*    public void AddSubIndicator(UnitPosition unitPosition, GameObject prefab)
            {
                if (Indicator == null)
                {
                    Debug.LogError("메인 인디케이터를 먼저 생성해 주세요");
                    return;
                }

                GameObject gameObject = Instantiate(prefab, Indicator);
                unitPosition.SetTransform(gameObject.transform);
            }

            public void AddSubIndicator(List<UnitPosition> unitPositions, GameObject prefab)
            {
                foreach (var unitPosition in unitPositions)
                    AddSubIndicator(unitPosition, prefab);
            }*/

        public void AddSubIndicator(List<Vector2Int> positions, GameObject prefab)
        {
            foreach (var position in positions)
                AddSubIndicator(position, prefab);
        }

        public void AddSubIndicator(Vector2Int position, GameObject prefab)
        {
            if (Indicator == null)
            {
                Debug.LogError("메인 인디케이터를 먼저 생성해 주세요");
                return;
            }

            GameObject child = Instantiate(prefab, Indicator);
            child.transform.localPosition = new Vector3(position.x, position.y, 0);
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@유닛 인디케이터 경계

        /// <summary>
        /// 타일 인디케이터 경계를 생성한다.
        /// </summary>
        /// <param name="unitPositions">  </param>
        public void AddIndicatorBoundary(UnitPosition unitPosition, GameObject prefab)
        {
            if (IndicatorBoundary == null)
                IndicatorBoundary = new GameObject("IndicatorBoundary").transform;

            Transform transform = Instantiate(prefab, IndicatorBoundary).transform;
            unitPosition.SetTransform(transform);
        }

        public void AddIndicatorBoundary(Vector2Int position, GameObject prefab)
        {
            AddIndicatorBoundary(new UnitPosition(position), prefab);
        }

        public void AddIndicatorBoundary(List<UnitPosition> unitPositions, GameObject prefab)
        {
            foreach (var unitPosition in unitPositions)
            {
                AddIndicatorBoundary(unitPosition, prefab);
            }
        }

        public void AddIndicatorBoundary(List<Vector2Int> positions, GameObject prefab)
        {
            foreach (var position in positions)
            {
                AddIndicatorBoundary(position, prefab);
            }
        }

        public void SetCustomClickTriggerOnIndicator(EventTrigger.Entry entryPointerClick)
        {
            if (Indicator == null)
            {
                Debug.LogWarning("인디케이터가 없습니다.");
                return;
            }

            if (Indicator.gameObject.GetComponent<EventTrigger>() == null)
                Indicator.gameObject.AddComponent<EventTrigger>();
            if (Indicator.gameObject.GetComponent<BoxCollider2D>() == null)
                Indicator.gameObject.AddComponent<BoxCollider2D>();

            EventTrigger eventTrigger = Indicator.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(entryPointerClick);
        }

        /// <summary>
        /// 인디케이터 바운더리에 회전 엔터 트리거를 추가한다.
        /// </summary>
        public void SetRotateEnterTriggerOnIndicatorBoundary()
        {
            Transform thisTurnUnit = BattleManager.instance.thisTurnUnit.transform;
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
                    if (Indicator.gameObject.activeSelf == false)
                        Indicator.gameObject.SetActive(true);

                    int x = Mathf.RoundToInt(child.position.x - thisTurnUnit.position.x);
                    int y = Mathf.RoundToInt(child.position.y - thisTurnUnit.position.y);
                    Vector3 vector = new Vector3(x, y, 0);
                    vector.Normalize();

                    float angle;
                    if (x < 0)
                        angle = -Mathf.Rad2Deg * Mathf.Acos(vector.y);
                    else
                        angle = Mathf.Rad2Deg * Mathf.Acos(vector.y);

                    Indicator.rotation = Quaternion.Euler(0, 0, -angle);
                });

                //child.position += Vector3.back;
                eventTrigger.triggers.Add(entryPointerEnter);
            }
        }

        /// <summary>
        /// 인디케이터 바운더리에 커서 따라오기 엔터 트리거를 추가한다.
        /// </summary>
        public void SetFollowEnterTriggerOnIndicatorBoundary()
        {
            for (int i = 0; i < IndicatorBoundary.childCount; i++)
            {
                Transform child = IndicatorBoundary.GetChild(i);

                if (child.GetComponent<EventTrigger>() == null)
                    child.gameObject.AddComponent<EventTrigger>();
                if (child.GetComponent<BoxCollider2D>() == null)
                    child.gameObject.AddComponent<BoxCollider2D>();

                EventTrigger eventTrigger = IndicatorBoundary.GetChild(i).GetComponent<EventTrigger>();
                Transform transform = IndicatorBoundary.GetChild(i);
                int x = Mathf.RoundToInt(transform.position.x);
                int y = Mathf.RoundToInt(transform.position.y);

                EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
                entryPointerEnter.eventID = EventTriggerType.PointerEnter;

                entryPointerEnter.callback.AddListener((data) =>
                {
                    if (Indicator.gameObject.activeSelf == false)
                        Indicator.gameObject.SetActive(true);

                    Vector2Int distance = new Vector2Int();
                    UnitPosition indicatorUnitPosition = UnitPosition.TransformToUnitPosition(Indicator.transform);

                    if (indicatorUnitPosition.lowerLeft.x > x) // 인디케이터 왼쪽 좌표보다 현재 마우스 위치가 더 왼쪽이면
                        distance.x = x - indicatorUnitPosition.lowerLeft.x; // 음의 x 값
                    else if (indicatorUnitPosition.upperRight.x < x) // 인디케이터 오른쪽 좌표보다 현재 마우스 위치가 더 오른쪽이면
                        distance.x = x - indicatorUnitPosition.upperRight.x; // 양의 x 값

                    if (indicatorUnitPosition.lowerLeft.y > y) // 인디케이터 아래 좌표보다 현재 마우스 위치가 더 아래이면
                        distance.y = y - indicatorUnitPosition.lowerLeft.y; // 음의 y 값
                    else if (indicatorUnitPosition.upperRight.y < y) // 인디케이터 아래 좌표보다 현재 마우스 위치가 더 위쪽이면
                        distance.y = y - indicatorUnitPosition.upperRight.y; // 양의 y 값

                    for (int k = indicatorUnitPosition.lowerLeft.x; k <= indicatorUnitPosition.upperRight.x; k++)
                        for (int l = indicatorUnitPosition.lowerLeft.y; l <= indicatorUnitPosition.upperRight.y; l++)
                        {
                            bool isContain = false;
                            for (int j = 0; j < IndicatorBoundary.childCount; j++)
                            {
                                int temp_x = Mathf.RoundToInt(IndicatorBoundary.GetChild(j).transform.position.x);
                                int temp_y = Mathf.RoundToInt(IndicatorBoundary.GetChild(j).transform.position.y);

                                if (temp_x == k + distance.x && temp_y == l + distance.y)
                                    isContain = true;
                            }
                            if (!isContain)
                                return;
                        }

                    indicatorUnitPosition.Add(distance);
                    indicatorUnitPosition.SetTransform(Indicator.transform);
                });

                eventTrigger.triggers.Add(entryPointerEnter);
            }
        }

        /// <summary>
        /// 인디케이터 바운더리에 사이즈와 위치를 똑같게하는 엔터 트리거를 추가한다.
        /// </summary>
        public void SetEqualizeEnterTriggerOnIndicatorBoundary()
        {
            if (IndicatorBoundary == null)
            {
                Debug.LogWarning("인디케이터 바운더리가 존재하지 않습니다.");
                return;
            }

            for (int i = 0; i < IndicatorBoundary.childCount; i++)
            {
                Transform child = IndicatorBoundary.GetChild(i);

                // Debug.LogError(child.position);

                if (child.GetComponent<EventTrigger>() == null)
                    child.gameObject.AddComponent<EventTrigger>();
                if (child.GetComponent<BoxCollider2D>() == null)
                    child.gameObject.AddComponent<BoxCollider2D>();

                EventTrigger eventTrigger = child.GetComponent<EventTrigger>();
                EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
                entryPointerEnter.eventID = EventTriggerType.PointerEnter;

                entryPointerEnter.callback.AddListener((data) =>
                {
                    if (Indicator.gameObject.activeSelf == false)
                        Indicator.gameObject.SetActive(true);

                    Indicator.position = child.position + Vector3.back;
                    Indicator.localScale = child.localScale;
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

        public void DestoryAll()
        {
            if (Indicator != null)
            {
                Destroy(Indicator.gameObject);
                Indicator = null;
            }

            if (IndicatorBoundary != null)
            {
                Destroy(IndicatorBoundary.gameObject);
                IndicatorBoundary = null;
            }
        }
    }
}