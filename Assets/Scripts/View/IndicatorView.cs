using Model.Managers;
using Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Model.Skills;
using System;

namespace View
{
    public class IndicatorView : MonoBehaviour
    {
        public static IndicatorView instance;
        private void Awake()
        {
            instance = this;
            tileIndicators = null;
        }
        private static GameObject tileIndicatorParent;
        public static GameObject TileIndicatorParent
        {
            get
            {
                if (tileIndicatorParent == null)
                {
                    tileIndicatorParent = new GameObject("Tile Indicator Parent");
                    tileIndicatorParent.AddComponent<ScreenTouchManager>().cameraTransform = Camera.main.transform;
                }

                return tileIndicatorParent;
            }
            set => tileIndicatorParent = value;
        }
        private static GameObject[,] tileIndicators;
        public static GameObject[,] TileIndicators
        {
            get
            {
                if (tileIndicators == null)
                {
                    tileIndicators = new GameObject[FieldManager.GetField().GetLength(0), FieldManager.GetField().GetLength(1)];

                    // Debug.Log(tileIndicators.GetLength(0) + ", " + tileIndicators.GetLength(1));

                    for (int y = 0; y < tileIndicators.GetLength(0); y++)
                        for (int x = 0; x < tileIndicators.GetLength(1); x++)
                        {
                            tileIndicators[y, x] = Instantiate(instance.tileIndicatorPrefab, new Vector3(x, y, -1), Quaternion.identity, TileIndicatorParent.transform);

                            EventTrigger eventTrigger = tileIndicators[y, x].GetComponent<EventTrigger>();
                            Vector2Int position = new Vector2Int(x, y);

                            EventTrigger.Entry entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((data) =>
                            {
                                Debug.Log("indicator" + position);

                                // 현재 눌러놓은 위치와 동일하다면, 
                                if (curAvlPositions.Contains(position) && curPosition == position)
                                {
                                    if (currentSkill != null)
                                    {
                                        // currentUnit.OnUseSkill.before();

                                        instance.StartCoroutine(currentSkill.Use(currentUnit, position));
                                        BattleView.UnitControlView.RefreshButtons();
                                        HideTileIndicator();

                                        // currentUnit.OnUseSkill.after();

                                    }
                                    if (tileAction != null)
                                    {
                                        tileAction(position);
                                        // 다음 유닛을 소환해야 해서 타일을 자동으로 숨길수가 없다.
                                        // 수동으로 타일을 숨겨주세요.
                                        // HideTileIndicator();
                                    }
                                }
                                else if (currentSkill != null)
                                    UpdateSkillIndicator(position);
                                else
                                    UpdateTileIndicator(position);
                            });
                            eventTrigger.triggers.Add(entry);
                        }
                }

                return tileIndicators;
            }
            set => tileIndicators = value;
        }

        [Header("Tile Indicator")]
        [SerializeField]
        GameObject tileIndicatorPrefab;

        [SerializeField]
        Color possibleCursorColor = new Color(0, 0, 1, 0.5f); // blue
        [SerializeField]
        Color relatedColor = new Color(0, 0.5f, 1, 0.5f); // cyan
        [SerializeField]
        Color sklRangeColor = new Color(1f, 1f, 0, 0.5f); // yello

        [SerializeField]
        Color impossibleCursorColor = new Color(1, 0, 0, 0.5f); // red
        [SerializeField]
        Color subImpossibleColor = new Color(1, 0, 1, 0.5f); // magenta

        [SerializeField]
        Color sklAvlColor = new Color(0, 1, 0, 0.5f); // green
        [SerializeField]
        Color sklNotAvlColor = new Color(1, 1, 1, 0.5f); // white

        private static Skill currentSkill;
        private static Unit currentUnit;
        public static Action<Vector2Int> tileAction;
        private static List<Vector2Int> curAvlPositions;
        private static Vector2Int? curPosition;
        public delegate List<Vector2Int> TileRelatedPositionsFunc (Vector2Int targetPosition);
        private static TileRelatedPositionsFunc TileRelativeFunc = null;

        private static void ChangeTileIndicatorColor(List<Vector2Int> positions, Color color)
        {
            foreach (var position in positions)
                ChangeTileIndicatorColor(position, color);
        }

        private static void ChangeTileIndicatorColor(Vector2Int position, Color color)
        {
            if (FieldManager.IsInField(position))
                TileIndicators[position.y, position.x].GetComponent<SpriteRenderer>().color = color;
        }

        public static void ShowSkillIndicator(Unit user, Skill skill)
        {
            currentUnit = user;
            currentSkill = skill;
            curPosition = null;
            curAvlPositions = currentSkill.GetAvlUsePositions(currentUnit);
            tileAction = null;
            TileRelativeFunc = null;
            TileIndicatorParent.SetActive(true);
            UpdateSkillIndicator();
        }

        private static void UpdateSkillIndicator(Vector2Int? position = null)
        {
            List<Vector2Int> RelatedPosition = position != null ? currentSkill.GetRelatePositions(currentUnit, (Vector2Int)position) : new List<Vector2Int>();
            List<Vector2Int> sklUseRange = currentSkill.GetUseRange(currentUnit, currentUnit.Position);

            Debug.Log(sklUseRange.Count);

            foreach (var item in sklUseRange)
            {
                Debug.Log(item);
            }

            curPosition = position;

            for (int y = 0; y < TileIndicators.GetLength(0); y++)
                for (int x = 0; x < TileIndicators.GetLength(1); x++)
                {
                    Vector2Int tempPosition = new Vector2Int(x, y);

                    if (RelatedPosition.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.relatedColor);
                    else if (curAvlPositions.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.sklAvlColor);
                    else if (sklUseRange.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.sklRangeColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.sklNotAvlColor);
                }

            if (position != null)
            {
                if (curAvlPositions.Contains((Vector2Int)position))
                    ChangeTileIndicatorColor((Vector2Int)position, instance.possibleCursorColor);
                else
                    ChangeTileIndicatorColor((Vector2Int)position, instance.impossibleCursorColor);
            }
        }

        private static List<Vector2Int> defaultFunc(Vector2Int position) => new List<Vector2Int>();

        /// <summary>
        /// 파티 소환, 에 사용
        /// </summary>
        /// <param name="positions">사용가능한 위치</param>
        /// <param name="action">두번째 클릭시 발동되는 함수</param>
        /// <param name="tileRelativeFunc">관련 타일 계산 함수 </param>
        public static void ShowTileIndicator(List<Vector2Int> positions, Action<Vector2Int> action, TileRelatedPositionsFunc tileRelativeFunc = null)
        {
            currentUnit = null;
            currentSkill = null;
            curPosition = null;
            curAvlPositions = positions;
            tileAction = action;
            TileRelativeFunc = tileRelativeFunc == null ? defaultFunc : tileRelativeFunc;
            TileIndicatorParent.SetActive(true);
            UpdateTileIndicator();
        }

        private static void UpdateTileIndicator(Vector2Int? position = null)
        {
            List<Vector2Int> RelatedPosition = position != null ? TileRelativeFunc((Vector2Int)position) : new List<Vector2Int>();
            curPosition = position;

            for (int y = 0; y < TileIndicators.GetLength(0); y++)
                for (int x = 0; x < TileIndicators.GetLength(1); x++)
                {
                    Vector2Int tempPosition = new Vector2Int(x, y);
                    if (RelatedPosition.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.relatedColor);
                    else if (curAvlPositions.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.sklAvlColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.sklNotAvlColor);
                }

            if (position != null)
            {
                Debug.Log("hello" + position);
                if (curAvlPositions.Contains((Vector2Int)position))
                    ChangeTileIndicatorColor((Vector2Int)position, instance.possibleCursorColor);
                else
                    ChangeTileIndicatorColor((Vector2Int)position, instance.impossibleCursorColor);
            }
        }

        public static void HideTileIndicator()
        {
            TileIndicatorParent.SetActive(false);
            currentUnit = null;
            currentSkill = null;
            tileAction = null;
        }
    }
}