using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;

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

        public enum Goal
        {
            KillAllEnemy,
            KillAllParty,
            EndureTurn
        }

        public Goal WinCondition = Goal.KillAllEnemy;
        public Goal DefeatCondition = Goal.KillAllParty;

        private void Awake()
        {
            instance = this;
            GenerateTiles(10, 10);

            /***유닛 더미데이터 추가. 추후 BattleUI의 EndGame 함수 포함하여 코드 제거***/
            List<Unit> EnemyUnits = new List<Unit>();

            Unit temp = new Units.Unit_001
            {
                Name = "슬라임1",
                Category = Category.Enemy,
                Position = new Vector2Int(0, 0)                
            };
            EnemyUnits.Add(temp);

            temp = new Units.Unit_001
            {
                Name = "슬라임2",
                Category = Category.Enemy,
                Position = new Vector2Int(9, 0)
            };
            EnemyUnits.Add(temp);

            temp = new Units.Unit_001
            {
                Name = "슬라임3",
                Category = Category.Enemy,
                Position = new Vector2Int(9, 9)
            };
            EnemyUnits.Add(temp);

            temp = new Units.Unit_001
            {
                Name = "슬라임4",
                Category = Category.Enemy,
                Position = new Vector2Int(0, 9)
            };
            EnemyUnits.Add(temp);

            if (GameManager.PartyUnits.Count == 0)
                 GameManager.Instance.InitForTesting();

            Common.UnitAction.Summon(EnemyUnits);


            for(int i = 0; i <  GameManager.PartyUnits.Count; i++)
            {
                GameManager.PartyUnits[i].Category = Category.Party;
                GameManager.PartyUnits[i].Position = new Vector2Int(i + 4, i + 4);
            }

            Common.UnitAction.Summon(GameManager.PartyUnits);
            /***************************************************************************/
        }

        public static bool IsWin()
        {
            // 승리조건이 모든 적을 죽이는 것일때
            if (instance.WinCondition == Goal.KillAllEnemy)
            {
                List<Unit> enemyUnits = GetUnit(Category.Enemy);
                if (enemyUnits.Count == 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }


        public static bool IsDefeat()
        {
            // 승리조건이 모든 적을 죽이는 것일때
            if (instance.WinCondition == Goal.KillAllParty)
            {
                List<Unit> partyUnits = GetUnit(Category.Party);
                if (partyUnits.Count == 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        static int GetUnitCount(Category category)
        {
            int count = 0;
            foreach (var unit in instance.AllUnits)
                if (unit.Category == category)
                {
                    if (unit.Agility != -10) count++;
                }
            return count;
        }

        public static int CheckGameState()
        {
            if (GetUnitCount(Category.Enemy) == 0)
            {
                return 1; // 승리
            }
            else if (GetUnitCount(Category.Party) == 0)
            {
                return 2; // 패배
            }
            return 0; // 계속
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

            nextUnit.ActionRate = 0;
            instance.thisTurnUnit = nextUnit;
        }
    }
}