using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Model.Managers
{
    public class BattleManager : Singleton<BattleManager>
    {
        public enum BattleStateEnum
        {
            Continue,
            Win,
            Defeat,
            Stop
        }

        public enum MissionEnum
        {
            KillAllEnemy,
            KillAllParty,
            EndureTurn
        }

        public static BattleManager instance;

        // 현재 전투에 참여하는 모든 유닛을 참조할 수 있습니다.
        public List<Unit> AllUnits = new List<Unit>();

        // 현재 턴의 유닛
        public Unit thisTurnUnit;

        public MissionEnum Mission = MissionEnum.KillAllEnemy;


        private void Start()
        {
            // FieldManager.FieldData temp = new FieldManager.FieldData(16, 16,
            // "WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL \n" +
            // "WL PW HL PW PW FR FR FR FR FR SL SL SL LK WL WL \n" +
            // "WL PW PW HL PW FR FR FR FR FR SL SL SL LK FR WL \n" +
            // "WL FR HL FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL TN TN TN TN TN TN TN TN TN TN TN TN TN TN WL \n" +
            // "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR WL WL FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR WL WL FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
            // "WL US US FR FR DS DS FR FR FR FR FR FR FR FR WL \n" +
            // "WL US US FR FR DS DS FR FR FR FR FR FR FR FR WL \n" +
            // "WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL ");

            //FieldManager.FieldData temp1 = Common.Data.LoadFieldData();
            // FieldManager.FieldData temp2 = Common.Data.LoadFieldData();
            //FieldManager.FieldData temp = FieldManager.instance.Merge2FieldData(temp1, temp2, new Vector2Int(temp1.width,temp1.height));

            // // 청크필드 매니저 선언
            // ChunkField chunk = new ChunkField();

            // // 청크 맵 생성
            // FieldManager.instance.InitField(chunk.GenerateChunkMap(map_size: 3, isAttacked: false));

            // // 테스트 파티 생성 (없을시)
            // if (GameManager.PartyUnits.Count == 0)
            // {
            //     GameManager.PartyUnits.Add(new Unit(UnitAlliance.Party, UnitSpecies.Human, 2));
            //     GameManager.PartyUnits.Add(new Unit(UnitAlliance.Party, UnitSpecies.Human, 2));
            //     GameManager.PartyUnits.Add(new Unit(UnitAlliance.Party, UnitSpecies.Human, 2));
            //     GameManager.PartyUnits.Add(new Unit(UnitAlliance.Party, UnitSpecies.Human, 2));
            // }

            // // 게임 시작시 재사용대기시간 초기화
            // GameManager.PartyUnits.ForEach(u => u.Skills.ForEach(s => s.WaitingTime = 0));

            // // 파티 유닛 최초 소환
            // BattleView.SummonPartyUnits();

            // // 적 생성
            // List<Unit> enemies = new List<Unit>();
            // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));
            // enemies.Add(new Unit(UnitAlliance.Enemy, UnitSpecies.Human, 1));

            // // 적 소환 타일 계산
            // List<Tile> enemyTiles = FieldManager.GetBlankFloorTiles(enemies.Count);

            // // 적 소환
            // for (int i = 0; i < enemies.Count; i++)
            //     Common.Command.Summon(enemies[i], enemyTiles[i].position);

            // // 배틀 설정
            // BattleController.SetBattleMode(true);
            // BattleView.TurnEndButton.gameObject.SetActive(false);
            // BattleController.instance.NextTurnStart();

            // // 모든 처리가 끝난 뒤에 애니메이션 재생 가능
            // // FadeOutTextView.PlayText();
        }

        public static BattleStateEnum CheckGameState()
        {
            // 배틀중이 아니라면 멈춘다.
            if (SettingManager.Instance.inBattle == false)
                return BattleStateEnum.Stop;

            // 승리조건이 모든 적이 죽는 것일 때
            else if (GetAliveUnitCount(UnitAlliance.Enemy) == 0)
                return BattleStateEnum.Win;

            // 패배조건이 모든 아군이 죽는 것일 때
            else if (GetAliveUnitCount(UnitAlliance.Party) == 0)
                return BattleStateEnum.Defeat;

            // 계속
            else
                return BattleStateEnum.Continue;
        }

        private static int GetAliveUnitCount(UnitAlliance alliance)
        {
            int count = 0;

            foreach (var unit in GetUnit(alliance))
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

        // public static Unit GetUnit(Vector2Int position)
        // {
        //     return FieldManager.GetTile(position)?.GetUnit();
        // }

        Queue<Unit> unitBuffer = new Queue<Unit>();

        int bufferSize = 5;

        public int UnitBufferSize => bufferSize;

        public Queue<Unit> UnitBuffer => unitBuffer;

        // public void InitializeUnitBuffer()
        // {
        //     unitBuffer.Clear();
        //     for (int i = 0; i < bufferSize; i++)
        //     {
        //         Unit unit = CalculateNextUnit();
        //         if (unit == null) continue;
        //         // Debug.Log(unit.Name);
        //         unitBuffer.Enqueue(unit);
        //     }
        // }

        // Unit CalculateNextUnit()
        // {
        //     float max = 100; // 주기의 최댓값
        //     float minTime = 100;
        //     Unit nextUnit = null; // 다음 턴에 행동할 유닛
        //     foreach (var unit in instance.AllUnits)
        //     {
        //         if (unit.Agility <= -10)
        //             continue;

        //         float velocity = unit.Agility * 10 + 100;
        //         float time = (max - unit.ActionRate) / velocity; // 거리 = 시간 * 속력 > 시간 = 거리 / 속력
        //         if (minTime >= time) // 시간이 가장 적게 걸리는애가 먼저된다.
        //         {
        //             minTime = time;
        //             nextUnit = unit;
        //         }
        //     }

        //     foreach (var unit in instance.AllUnits)
        //         unit.ActionRate += (unit.Agility * 10 + 100) * minTime;

        //     if (nextUnit != null) nextUnit.ActionRate = 0;
        //     return nextUnit;
        // }

        // public static Unit GetNextTurnUnit()
        // {
        //     instance.unitBuffer.Enqueue(instance.CalculateNextUnit());
        //     return instance.unitBuffer.Dequeue();
        // }

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