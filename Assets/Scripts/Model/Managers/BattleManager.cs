using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;
using Model.Units;

namespace Model.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;

        // 현재 전투의 모든 유닛을 참조할 수 있습니다.
        public List<Unit> AllUnits = new List<Unit>();

        // 현재 턴의 유닛
        public Unit thisTurnUnit;

        public int turnCount;

        public enum State
        {
            Continue,
            Win,
            Defeat
        }

        public enum Condition
        {
            KillAllEnemy,
            KillAllParty,
            EndureTurn
        }

        public Condition WinCondition = Condition.KillAllEnemy;
        public Condition DefeatCondition = Condition.KillAllParty;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            FieldManager.instance.InitField(FieldManager.instance.FieldDatas[0]);

            if (GameManager.Instance.currentRoom == null)
            {
                Unit unit = new M000_Judgement();
                Common.UnitAction.Summon(unit, new Vector2Int(4, 4));
                Common.UnitAction.AddEffect(unit, new Model.Effects.E004_Stun(unit));
                Common.UnitAction.AddEffect(unit, new Model.Effects.E005_Regeneration(unit, 99));
                Common.UnitAction.AddEffect(unit, new Model.Effects.E021_Barrier(unit, 10, 99));
            }
            else if (GameManager.Instance.currentRoom.category == Room.Category.Monster)
            {
                int rand = UnityEngine.Random.Range(0, 2);

                if (rand == 0)
                {
                    Common.UnitAction.Summon(new Proto_Skeleton(), new Vector2Int(4, 4));
                    Common.UnitAction.Summon(new Proto_Skeleton(), new Vector2Int(5, 4));
                }
                else
                {
                    Common.UnitAction.Summon(new Proto_RedSkeleton(), new Vector2Int(4, 4));
                }
            }
            else if (GameManager.Instance.currentRoom.category == Room.Category.Elite)
            {
                Common.UnitAction.Summon(new Proto_RedSkeleton(), new Vector2Int(6, 7));
                Common.UnitAction.Summon(new Proto_Skeleton(), new Vector2Int(4, 4));
                Common.UnitAction.Summon(new Proto_Skeleton(), new Vector2Int(4, 6));
            }
            else if (GameManager.Instance.currentRoom.category == Room.Category.Boss)
            {
                Unit unit = new M000_Judgement();
                Common.UnitAction.Summon(unit, new Vector2Int(4, 4));
                Common.UnitAction.AddEffect(unit, new Model.Effects.E004_Stun(unit));
                Common.UnitAction.AddEffect(unit, new Model.Effects.E005_Regeneration(unit, 99));
            }

            if (GameManager.PartyUnits.Count == 0)
            {
                GameManager.PartyUnits.Add(UnitManager.Instance.AllUnits[0]);
                GameManager.PartyUnits.Add(UnitManager.Instance.AllUnits[1]);
            }

            GameManager.InBattle = true;

            if (GameManager.InBattle)
            {
                View.ViewManager.battle.SummonPartyUnits();// 파티 유닛 최초 소환
            }
            else
            {
                Common.UnitAction.Summon(GameManager.LeaderUnit, new Vector2Int(8, 8));
                StartCoroutine(View.ViewManager.battle.MoveLeaderUnit());
            }


            //유물 타일 맵 테스트
            FieldManager.instance.SetObtainableObj(new Artifact_Test(), new Vector2Int(6, 6));
            FieldManager.instance.SetObtainableObj(new Artifact_Test(), new Vector2Int(10, 5));
            FieldManager.instance.SetObtainableObj(new Artifact_Test(), new Vector2Int(6, 8));
            FieldManager.instance.SetObtainableObj(new Artifact_Test(), new Vector2Int(9, 9));
            FieldManager.instance.SetObtainableObj(new Artifact_Test(), new Vector2Int(6, 4));

            // UI.Battle.IndicatorUI.ShowTileIndicator()

            //Vector2Int[] party_position = { new Vector2Int(4, 0), new Vector2Int(5, 0), new Vector2Int(3, 0), new Vector2Int(6, 0) };

            //for (int i = 0; i < GameManager.PartyUnits.Count; i++)
            //{
            //    // Common.UnitAction.Summon(GameManager.PartyUnits[i], party_position[i]);
            //    GameManager.PartyUnits[i].ActionRate = 0;
            //    foreach (var skill in GameManager.PartyUnits[i].Skills)
            //        if (skill != null)
            //            skill.CurrentReuseTime = 0;
            //}

            /***************************************************************************/
        }

        public static State CheckGameState()
        {
            // 승리조건이 모든 적이 죽는 것일 때
            if (instance.WinCondition == Condition.KillAllEnemy && GetAliveUnitCount(UnitAlliance.Enemy) == 0)
                return State.Win;
            
            // 패배조건이 모든 아군이 죽는 것일 때
            if (instance.DefeatCondition == Condition.KillAllParty && GetAliveUnitCount(UnitAlliance.Party) == 0)
                return State.Defeat;

            // 계속
            return State.Continue; 
        }

        private static int GetAliveUnitCount(UnitAlliance alliance)
        {
            int count = 0;

            foreach (var unit in GetUnit(alliance))
                if (Common.UnitAction.GetEffectByNumber(unit, 1) == null && (Common.UnitAction.GetEffectByNumber(unit, 2) == null))
                    count++;

            return count;
        }

        public static List<Unit> GetUnit(UnitAlliance alliance)
        {
            List<Unit> units = new List<Unit>();

            foreach (var unit in instance.AllUnits)
                if (unit.Alliance == alliance)
                    units.Add(unit);

            return units;
        }

        /// <summary>
        /// 모든 유닛 리턴
        /// </summary>
        /// <returns></returns>
        public static List<Unit> GetUnit()
        {
            return instance.AllUnits;
        }

        /// <summary>
        /// 위치의 유닛 리턴
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Unit GetUnit(Vector2Int position)
        {
            return FieldManager.GetTile(position)?.GetUnit();
        }

        Queue<Unit> unitBuffer = new Queue<Unit>();
        int bufferSize = 5;

        public int UnitBufferSize => bufferSize;

        public Queue<Unit> UnitBuffer => unitBuffer;

        public void InitializeUnitBuffer()
        {
            unitBuffer.Clear();
            for(int i = 0; i < bufferSize; i++)
            {
                Unit unit = CalculateNextUnit();
                Debug.Log(unit.Name);
                unitBuffer.Enqueue(unit);
            }
        }

        Unit CalculateNextUnit()
        {
            float max = 100; // 주기의 최댓값
            float minTime = 100;
            Unit nextUnit = null; // 다음 턴에 행동할 유닛
            foreach (var unit in instance.AllUnits)
            {
                if (unit.Agility <= -10)
                    continue;

                float velocity = unit.Agility * 10 + 100;
                float time = (max - unit.ActionRate) / velocity; // 거리 = 시간 * 속력 > 시간 = 거리 / 속력
                if (minTime >= time) // 시간이 가장 적게 걸리는애가 먼저된다.
                {
                    minTime = time;
                    nextUnit = unit;
                }
            }

            foreach (var unit in instance.AllUnits)
                unit.ActionRate += (unit.Agility * 10 + 100) * minTime;

            nextUnit.ActionRate = 0;
            return nextUnit;
        }

        public static Unit GetNextTurnUnit()
        {
            instance.unitBuffer.Enqueue(instance.CalculateNextUnit());
            return instance.unitBuffer.Dequeue();
        }

        public static void SetNextTurnUnit(Unit nextUnit)
        {
            instance.thisTurnUnit = nextUnit;
        }

        /* public static Unit GetNextTurnUnit()
            {
                float max = 100; // 주기의 최댓값
                float minTime = 100;
                Unit nextUnit = null; // 다음 턴에 행동할 유닛

                foreach (var unit in instance.AllUnits)
                {
                    if (unit.Agility <= -10)
                        continue;

                    float velocity = unit.Agility * 10 + 100;
                    float time = (max - unit.ActionRate) / velocity; // 거리 = 시간 * 속력 > 시간 = 거리 / 속력
                    if (minTime >= time) // 시간이 가장 적게 걸리는애가 먼저된다.
                    {
                        minTime = time;
                        nextUnit = unit;
                    }
                }
                return nextUnit;
            }

            public static void SetNextTurnUnit(Unit nextUnit)
            {
                float max = 100; // 주기의 최댓값
                float velocity = nextUnit.Agility * 10 + 100;
                float minTime = (max - nextUnit.ActionRate) / velocity; // 거리 = 시간 * 속력 > 시간 = 거리 / 속력

                //나머지 유닛들도 해당 시간만큼 이동.
                foreach (var unit in instance.AllUnits)
                    unit.ActionRate += (unit.Agility * 10 + 100) * minTime;

                instance.thisTurnUnit = nextUnit;
            }*/
    }
}