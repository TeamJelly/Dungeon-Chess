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

        public List<Unit> AllUnits;
        public Tile[,] AllTiles; //10x10일때 0,0 ~ 9,9으로
        public Unit thisTurnUnit;

        private void Awake()
        {
            instance = this;
            GenerateTiles(10, 10);

            /***유닛 더미데이터 추가. 추후 BattleUI의 EndGame 함수 포함하여 코드 제거***/
            AddUnitsIntoRoom(UnitManager.Instance.EnemyUnits);
            AddUnitsIntoRoom(UnitManager.Instance.PartyUnits);
            /***************************************************************************/
        }

        public List<Unit> GetEnemyUnits()
        {
            List<Unit> units = new List<Unit>();

            foreach (var item in AllUnits)
            {
                if (item.category == Category.Enemy)
                    units.Add(item);
            }

            return units;
        }

        public static Unit GetUnit(Vector2Int position)
        {
            return GetTile(position).GetUnit();
        }

        public static Tile GetTile(Vector2Int position)
        {
            return GetTile(position.x, position.y);
        }

        public static Tile GetTile(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < instance.AllTiles.GetLength(0) && y < instance.AllTiles.GetLength(1))
                return instance.AllTiles[x, y];
            else
                return null;
        }

        /// <summary>
        /// 룸에 유닛들 추가
        /// </summary>
        /// <param name="units">추가할 유닛 리스트</param>
        void AddUnitsIntoRoom(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                AllUnits.Add(unit);
                Common.UnitAction.Summon(unit, unit.position);
            }
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
        public Unit GetNextTurnUnit()
        {
            float max = 100; // 주기의 최댓값
            float minTime = 100;
            Unit nextUnit = null; // 다음 턴에 행동할 유닛

            foreach (var unit in AllUnits)
            {
                float velocity = unit.agility * 10 + 100;
                float time = (max - unit.actionRate) / velocity; // 거리 = 시간 * 속력 > 시간 = 거리 / 속력
                if (minTime >= time) // 시간이 가장 적게 걸리는애가 먼저된다.
                {
                    minTime = time;
                    nextUnit = unit;
                }
            }

            //나머지 유닛들도 해당 시간만큼 이동.
            foreach (var unit in AllUnits)
            {
                float velocity = unit.agility * 10 + 100;
                unit.actionRate += velocity * minTime;
            }

            //다음 턴 유닛 값들 초기화
            nextUnit.actionRate = 0;
            nextUnit.moveCount = 1;
            nextUnit.skillCount = 1;
            nextUnit.itemCount = 1;

            thisTurnUnit = nextUnit;
            return nextUnit;
        }
    }
}