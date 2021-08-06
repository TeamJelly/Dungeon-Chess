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

                    for (int i = 0; i < tileIndicators.GetLength(0); i++)
                        for (int j = 0; j < tileIndicators.GetLength(1); j++)
                        {
                            tileIndicators[i, j] = Instantiate(instance.tileIndicatorPrefab, new Vector3(i, j, -1), Quaternion.identity, TileIndicatorParent.transform);

                            EventTrigger eventTrigger = tileIndicators[i, j].GetComponent<EventTrigger>();
                            Vector2Int position = new Vector2Int(i, j);

                            EventTrigger.Entry entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((data) =>
                            {
                                // 현재 눌러놓은 위치와 동일하다면, 
                                if (curAvlPositions.Contains(position) && curPosition == position)
                                {
                                    if (currentSkill != null)
                                    {
                                        // currentUnit.OnUseSkill.before();

                                        instance.StartCoroutine(currentSkill.Use(currentUnit, position));
                                        BattleView.UnitControlView.RefreshButtons(currentUnit);
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
        Color possibleColor = new Color(0, 0, 1, 0.5f); // blue
        [SerializeField]
        Color subPossibleColor = new Color(0, 0.5f, 1, 0.5f); // cyan
        [SerializeField]
        Color impossibleColor = new Color(1, 0, 0, 0.5f); // red
        [SerializeField]
        Color subImpossibleColor = new Color(1, 0, 1, 0.5f); // magenta
        [SerializeField]
        Color inBoundaryColor = new Color(0, 1, 0, 0.5f); // green
        [SerializeField]
        Color outBoundaryColor = new Color(1, 1, 1, 0.5f); // white

        private static Skill currentSkill;
        private static Unit currentUnit;
        public static Action<Vector2Int> tileAction;
        private static List<Vector2Int> curAvlPositions;
        private static Vector2Int? curPosition;

        public static void ChangeTileIndicatorColor(List<Vector2Int> positions, Color color)
        {
            foreach (var position in positions)
                ChangeTileIndicatorColor(position, color);
        }

        public static void ChangeTileIndicatorColor(Vector2Int position, Color color)
        {
            if (FieldManager.IsInField(position))
                TileIndicators[position.x, position.y].GetComponent<SpriteRenderer>().color = color;
        }

        public static void ShowSkillIndicator(Unit user, Skill skill)
        {
            currentUnit = user;
            currentSkill = skill;
            curAvlPositions = currentSkill.GetAvailablePositions(currentUnit);
            curPosition = null;
            TileIndicatorParent.SetActive(true);
            UpdateSkillIndicator();
        }

        public static void UpdateSkillIndicator(Vector2Int? position = null)
        {
            List<Vector2Int> RelatedPosition = position != null ? RelatedPosition = currentSkill.GetRelatePositions(currentUnit, (Vector2Int)position) : new List<Vector2Int>();

            for (int i = 0; i < TileIndicators.GetLength(0); i++)
                for (int j = 0; j < TileIndicators.GetLength(1); j++)
                {
                    Vector2Int tempPosition = new Vector2Int(i, j);
                    if (RelatedPosition != null && RelatedPosition.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.subPossibleColor);
                    else if (curAvlPositions.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.inBoundaryColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.outBoundaryColor);
                }

            curPosition = position;
            if (curPosition != null)
            {
                if (curAvlPositions.Contains((Vector2Int)curPosition))
                    ChangeTileIndicatorColor((Vector2Int)curPosition, instance.possibleColor);
                else
                    ChangeTileIndicatorColor((Vector2Int)curPosition, instance.impossibleColor);
            }
        }

        /// <summary>
        /// 파티 소환, 에 사용
        /// </summary>
        /// <param name="positions">사용가능한 위치</param>
        /// <param name="action">두번째 클릭시 발동되는 함수</param>
        public static void ShowTileIndicator(List<Vector2Int> positions, Action<Vector2Int> action)
        {
            currentSkill = null;
            curAvlPositions = positions;
            curPosition = null;
            tileAction = action;
            UpdateTileIndicator(curPosition);
        }

        private static void UpdateTileIndicator(Vector2Int? position = null)
        {
            for (int i = 0; i < TileIndicators.GetLength(0); i++)
                for (int j = 0; j < TileIndicators.GetLength(1); j++)
                {
                    Vector2Int tempPosition = new Vector2Int(i, j);
                    if (curAvlPositions.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.inBoundaryColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.outBoundaryColor);
                }

            curPosition = position;
            if (curPosition != null)
            {
                if (curAvlPositions.Contains((Vector2Int)position))
                    ChangeTileIndicatorColor((Vector2Int)position, instance.possibleColor);
                else
                    ChangeTileIndicatorColor((Vector2Int)position, instance.impossibleColor);
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