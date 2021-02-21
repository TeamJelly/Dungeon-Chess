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

        // 현재 전투의 모든 타일을 참조할 수 있습니다.
        private Tile[,] AllTiles; 

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
            GenerateTiles(10, 10);

            /***유닛 더미데이터 추가. 추후 BattleUI의 EndGame 함수 포함하여 코드 제거***/
            List<Unit> EnemyUnits = new List<Unit>();


            EnemyUnits.Add(new Proto_Skeleton());
            EnemyUnits.Add(new Proto_Skeleton());
            EnemyUnits.Add(new Proto_RedSkeleton());
            EnemyUnits.Add(new Proto_RedSkeleton());
            EnemyUnits.Add(new Proto_Judgement());

            EnemyUnits[0].Position = new Vector2Int(3, 3);
            EnemyUnits[1].Position = new Vector2Int(3, 6);
            EnemyUnits[2].Position = new Vector2Int(6, 3);
            EnemyUnits[3].Position = new Vector2Int(6, 6);
            EnemyUnits[4].Position = new Vector2Int(0, 0);

            if (GameManager.PartyUnits.Count == 0)
            {
                GameManager.PartyUnits.Add(UnitManager.Instance.AllUnits[0]);
                GameManager.PartyUnits.Add(UnitManager.Instance.AllUnits[1]);
            }
                 //GameManager.Instance.InitForTesting();

            Common.UnitAction.Summon(EnemyUnits);


            for(int i = 0; i <  GameManager.PartyUnits.Count; i++)
            {
                GameManager.PartyUnits[i].Category = Category.Party;
                GameManager.PartyUnits[i].Position = new Vector2Int(i + 4, i + 4);
            }

            Common.UnitAction.Summon(GameManager.PartyUnits);
            /***************************************************************************/
        }

        public static State CheckGameState()
        {
            // 승리조건이 모든 적을 죽이는 것일때
            if (instance.WinCondition == Condition.KillAllEnemy && GetAliveUnitCount(Category.Enemy) == 0)
                return State.Win;
            
            // 패배조건이 모든 아군이 죽이는 것일때
            if (instance.DefeatCondition == Condition.KillAllParty && GetAliveUnitCount(Category.Party) == 0)
                return State.Defeat;

            // 계속
            return State.Continue; 
        }

        private static int GetAliveUnitCount(Category category)
        {
            int count = 0;

            foreach (var unit in GetUnit(category))
                if (Common.UnitAction.GetEffectByNumber(unit, 1) == null && (Common.UnitAction.GetEffectByNumber(unit, 2) == null))
                    count++;

            return count;
        }

        public static bool IsAvilablePosition(Vector2Int position)
        {
            if (position.x >= 0 &&
                position.y >= 0 &&
                position.x < instance.AllTiles.GetLength(0) &&
                position.y < instance.AllTiles.GetLength(1))
                return true;
            else
                return false;
        }

        public static List<Unit> GetUnit(Category category)
        {
            List<Unit> units = new List<Unit>();

            foreach (var unit in instance.AllUnits)
                if (unit.Category == category)
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
            return GetTile(position)?.GetUnit();
        }

        public static Tile GetTile(Vector2Int position)
        {
            return GetTile(position.x, position.y);
        }

        public static Tile GetTile(int x, int y)
        {
            if (IsAvilablePosition(new Vector2Int(x, y)))
                return instance.AllTiles[x, y];
            else
                return null;
        }
        public static Tile[,] GetTile()
        {
            return instance.AllTiles;
        }

        /// <summary>
        /// 정보 저장용 타일 생성
        /// </summary>
        /// <param name="width">맵 너비</param>
        /// <param name="height">맵 높이</param>
        void GenerateTiles(int width, int height)
        {
            AllTiles = new Tile[width, height];
            for (int i = 0; i < AllTiles.GetLength(0); i++)
                for (int j = 0; j < AllTiles.GetLength(1); j++)
                    AllTiles[i, j] = new Tile();
        }

        /// <summary>
        /// 다음 턴 유닛 선택 알고리즘
        /// </summary>
        /// <returns>다음 턴 유닛</returns>
        public static Unit GetNextTurnUnit()
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
                unit.ActionRate += velocity * minTime;

            instance.thisTurnUnit = nextUnit;
        }
    }
}