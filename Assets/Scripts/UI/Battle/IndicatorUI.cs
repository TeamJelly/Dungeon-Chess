using Model.Managers;
using Model;
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

        private static GameObject tileIndicatorParent;
        public static GameObject TileIndicatorParent
        {
            get
            {
                if (tileIndicatorParent == null)
                    tileIndicatorParent = new GameObject("Tile Indicator Parent");

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
                    tileIndicators = new GameObject[BattleManager.GetTile().GetLength(0), BattleManager.GetTile().GetLength(1)];

                    for (int i = 0; i < tileIndicators.GetLength(0); i++)
                        for (int j = 0; j < tileIndicators.GetLength(1); j++)
                        {
                            tileIndicators[i, j] = Instantiate(instance.tileIndicatorPrefab, new Vector3(i, j, 0), Quaternion.identity, TileIndicatorParent.transform);

                            EventTrigger eventTrigger = tileIndicators[i, j].GetComponent<EventTrigger>();
                            Vector2Int position = new Vector2Int(i, j);
                            
                            EventTrigger.Entry entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerEnter;
                            entry.callback.AddListener((data) => UpdateTileIndicator(position));
                            eventTrigger.triggers.Add(entry);

                            entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((data) =>
                            {
                                if (currentSkill.IsAvailablePosition(currentUnit, position))
                                {
                                    instance.StartCoroutine(currentSkill.Use(currentUnit, position));
                                    HideTileIndicator();

                                    BattleUI.instance.currentPushedButton = null;
                                    BattleUI.instance.UpdateThisTurnPanel(currentUnit);
                                }
                                else
                                {
                                    Debug.LogError($"{position}에 사용할수 없습니다.");
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
        public GameObject tileIndicatorPrefab;

        [SerializeField]
        Color possibleColor = Color.blue;
        [SerializeField]
        Color impossibleColor = Color.red;
        [SerializeField]
        Color subIndicatorColor = Color.yellow;
        [SerializeField]
        Color inBoundaryColor = Color.green;
        [SerializeField]
        Color outBoundaryColor = Color.white;

        private static Skill currentSkill;
        private static Unit currentUnit;

        public static void ChangeTileIndicatorColor(List<Vector2Int> positions, Color color)
        {
            foreach (var position in positions)
                ChangeTileIndicatorColor(position, color);
        }

        public static void ChangeTileIndicatorColor(Vector2Int position, Color color)
        {
            if (BattleManager.IsAvilablePosition(position))
                TileIndicators[position.x, position.y].GetComponent<SpriteRenderer>().color = color;
        }

        public static void UpdateTileIndicator(Vector2Int position)
        {
            for (int i = 0; i < TileIndicators.GetLength(0); i++)
                for (int j = 0; j < TileIndicators.GetLength(1); j++)
                {
                    Vector2Int tempPosition = new Vector2Int(i, j);
                    if (currentSkill.IsAvailablePosition(currentUnit, tempPosition))
                        ChangeTileIndicatorColor(tempPosition, instance.inBoundaryColor);
                    else
                        ChangeTileIndicatorColor(tempPosition, instance.outBoundaryColor);
                }

            if (currentSkill.IsAvailablePosition(currentUnit, position))
                ChangeTileIndicatorColor(position, instance.possibleColor);
            else
                ChangeTileIndicatorColor(position, instance.impossibleColor);
        }

        public static void ShowTileIndicator(Unit user, Skill skill)
        {
            currentUnit = user;
            currentSkill = skill;

            TileIndicatorParent.SetActive(true);
            UpdateTileIndicator(user.Position);
        }

        public static void HideTileIndicator()
        {
            TileIndicatorParent.SetActive(false);
        }
    }
}