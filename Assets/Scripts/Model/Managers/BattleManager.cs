using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model.Units;
using View;

namespace Model.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;

        // 현재 전투의 모든 유닛을 참조할 수 있습니다.
        public List<Unit> AllUnits = new List<Unit>();

        // 현재 전투의 모든 획득품들을 참조할 수 있습니다.
        public List<Obtainable> AllObtainables = new List<Obtainable>();

        // 현재 턴의 유닛
        [NonSerialized]
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
                Unit unit = new Proto_Skeleton();//new M000_Judgement();
                Common.Command.Summon(unit, new Vector2Int(4, 4));
                Common.Command.AddArtifact(unit, new Model.Artifacts.A999_test());
                Common.Command.AddArtifact(unit, new Model.Artifacts.A999_test());
                Common.Command.AddArtifact(unit, new Model.Artifacts.A999_test());
                Common.Command.AddArtifact(unit, new Model.Artifacts.A999_test());
                // Common.Command.AddEffect(unit, new Model.Effects.E004_Stun(unit));
                // Common.Command.AddEffect(unit, new Model.Effects.E005_Regeneration(unit, 99));
                // Common.Command.AddEffect(unit, new Model.Effects.E021_Barrier(unit, 10));
            }
            else if (GameManager.Instance.currentRoom.category == Room.Category.Monster)
            {
                int rand = UnityEngine.Random.Range(0, 2);

                if (rand == 0)
                {
                    Common.Command.Summon(new Proto_Skeleton(), new Vector2Int(4, 4));
                    Common.Command.Summon(new Proto_Skeleton(), new Vector2Int(5, 4));
                }
                else
                {
                    Common.Command.Summon(new Proto_RedSkeleton(), new Vector2Int(4, 4));
                }
            }
            else if (GameManager.Instance.currentRoom.category == Room.Category.Elite)
            {
                Common.Command.Summon(new Proto_RedSkeleton(), new Vector2Int(6, 7));
                Common.Command.Summon(new Proto_Skeleton(), new Vector2Int(4, 4));
                Common.Command.Summon(new Proto_Skeleton(), new Vector2Int(4, 6));
            }
            else if (GameManager.Instance.currentRoom.category == Room.Category.Boss)
            {
                Unit unit = new M000_Judgement();
                Common.Command.Summon(unit, new Vector2Int(4, 4));
                Common.Command.AddEffect(unit, new Model.Effects.E004_Stun(unit));
                Common.Command.AddEffect(unit, new Model.Effects.E005_Regeneration(unit, 99));
            }

            if (GameManager.PartyUnits.Count == 0)
            {
                GameManager.PartyUnits.Add(new Unit(UnitAlliance.Party, 11));
                GameManager.PartyUnits.Add(new Unit(UnitAlliance.Party, 10));
                GameManager.LeaderUnit = GameManager.PartyUnits[0];
            }
            GameManager.InBattle = true;

            if (GameManager.InBattle)
            {
                BattleView.SummonPartyUnits();// 파티 유닛 최초 소환
            }
            else
            {
                GameManager.LeaderUnit.Position = FieldManager.instance.GetStairPosition();
                //StartCoroutine(BattleView.SetNonBattleMode());
                BattleView.SetNonBattleMode();
            }

            // Common.Command.Summon(new Model.Artifacts.A000_Test1(), new Vector2Int(6, 6));
            // Common.Command.Summon(new Model.Artifacts.A000_Test1(), new Vector2Int(10, 5));
             Common.Command.Summon(new Model.Artifacts.A006_AutoHill(), new Vector2Int(6, 8));
            // Common.Command.Summon(new Model.Artifacts.A000_Test1(), new Vector2Int(9, 9));
            // Common.Command.Summon(new Model.Artifacts.A000_Test1(), new Vector2Int(6, 4));

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
                if (Common.Command.GetEffectByNumber(unit, 1) == null && (Common.Command.GetEffectByNumber(unit, 2) == null))
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

        public static List<Unit> GetUnit()
        {
            return instance.AllUnits;
        }
        
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
            for (int i = 0; i < bufferSize; i++)
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