﻿using Model.Managers;
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
                            entry.eventID = EventTriggerType.PointerEnter;
                            entry.callback.AddListener(
                                (data) =>
                                {
                                    if (currentSkill != null)
                                        UpdateSkillIndicator(position);
                                    else
                                    {
                                        UpdateTileIndicator(position);
                                    }
                                }
                            );
                            eventTrigger.triggers.Add(entry);

                            entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((data) =>
                            {
                                if (currentAvailablePositions.Contains(position))
                                {
                                    if (currentSkill != null)
                                    {
                                        instance.StartCoroutine(currentSkill.Use(currentUnit, position));

                                        BattleView.UnitControlUI.RefreshButtons(currentUnit);
                                        HideTileIndicator();

                                        for (int k = currentUnit.StateEffects.Count - 1; k >= 0; k--)
                                            currentUnit.StateEffects[k].AfterUseSkill();
                                    }
                                    else
                                    {
                                        tileAction(position);
                                        // 다음 유닛을 소환해야 해서 타일을 자동으로 숨길수가 없다.
                                        // 수동으로 타일을 숨겨주세요.
                                        // HideTileIndicator();
                                    }

                                    //ViewManager.battle.ThisTurnUnitInfo.CurrentPushedButton = null;
                                    // ViewManager.battle.ThisTurnUnitInfo.UpdateUnitInfo();
                                }
                                else
                                {
                                    Debug.LogError($"{position}에 사용할 수 없습니다.");
                                }
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
        Color possibleColor = new Color(0, 0, 1, 0.5f); // red
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
        private static List<Vector2Int> currentAvailablePositions;

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

        public static void UpdateSkillIndicator(Vector2Int position)
        {
            List<Vector2Int> RelatedPosition = currentSkill.GetRelatePositions(currentUnit, position);

            for (int i = 0; i < TileIndicators.GetLength(0); i++)
                for (int j = 0; j < TileIndicators.GetLength(1); j++)
                {
                    Vector2Int tempPosition = new Vector2Int(i, j);
                    if (RelatedPosition != null && RelatedPosition.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.subPossibleColor);
                    else if (currentAvailablePositions.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.inBoundaryColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.outBoundaryColor);
                }

            if (currentAvailablePositions.Contains(position))
                ChangeTileIndicatorColor(position, instance.possibleColor);
            else
                ChangeTileIndicatorColor(position, instance.impossibleColor);
        }

        /// <summary>
        /// 파티 소환에 사용
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="action"></param>
        public static void ShowTileIndicator(List<Vector2Int> positions, Action<Vector2Int> action)
        {
            currentSkill = null;
            currentAvailablePositions = positions;
            tileAction = action;
            UpdateTileIndicator(positions[0]);
        }

        private static void UpdateTileIndicator(Vector2Int position)
        {
            for (int i = 0; i < TileIndicators.GetLength(0); i++)
                for (int j = 0; j < TileIndicators.GetLength(1); j++)
                {
                    Vector2Int tempPosition = new Vector2Int(i, j);
                    if (currentAvailablePositions.Contains(tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.inBoundaryColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.outBoundaryColor);
                }

            if (currentAvailablePositions.Contains(position))
                ChangeTileIndicatorColor(position, instance.possibleColor);
            else
                ChangeTileIndicatorColor(position, instance.impossibleColor);
        }

        public static void ShowSkillIndicator(Unit user, Skill skill)
        {
            currentUnit = user;
            currentSkill = skill;
            currentAvailablePositions = currentSkill.GetAvailablePositions(currentUnit);

            TileIndicatorParent.SetActive(true);
            UpdateSkillIndicator(user.Position);
        }

        public static void HideTileIndicator()
        {
            TileIndicatorParent.SetActive(false);
        }
    }
}