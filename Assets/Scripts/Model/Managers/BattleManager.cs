using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model.Models;

namespace Model.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;
        public List<Unit> AllUnits;

        public List<Unit> PartyUnits;// 임시용 적군 더미데이터
        public List<Unit> EnemyUnits;// 임시용 적군 더미데이터

        public Tile[,] AllTiles; //10x10일때 0,0 ~ 9,9으로
        public Unit thisTurnUnit;

        private void Awake()
        {
            instance = this;
            GenerateTiles(10, 10);

            /***유닛 더미데이터 추가. 추후 BattleUI의 EndGame 함수 포함하여 코드 제거***/
            foreach (Unit unit in PartyUnits) UnitManager.Instance.AddPartyUnit(unit);
            foreach (Unit unit in EnemyUnits) UnitManager.Instance.AddEnemyUnit(unit);
            AddUnitsIntoRoom(UnitManager.Instance.EnemyUnits);
            AddUnitsIntoRoom(UnitManager.Instance.PartyUnits);
            /***************************************************************************/

            OnBattleStart();
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
                //유닛 타일 할당
                AllocateUnitTiles(unit, unit.unitPosition);
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

        public Tile GetTile(Vector2Int position)
        {
            if (position.x >= 0 && position.y >= 0 && position.x < AllTiles.GetLength(0) && position.y < AllTiles.GetLength(1))
                return AllTiles[position.x, position.y];
            else
                return null;
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < AllTiles.GetLength(0) && y < AllTiles.GetLength(1))
                return AllTiles[x, y];
            else
                return null;
        }

        public void AllocateUnitTiles(Unit unit, UnitPosition position)
        {
            for (int i = position.lowerLeft.x; i <= position.upperRight.x; i++)
                for (int j = position.lowerLeft.y; j <= position.upperRight.y; j++)
                {
                    AllTiles[i, j].SetUnit(unit);
                }
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

        /// <summary>
        /// 전투가 시작 시 발동되는 효과 활성화
        /// </summary>
        public void OnBattleStart()
        {
            foreach (var unit in AllUnits)
            {
                foreach (var effect in unit.stateEffects)
                    effect.OnBattleStart();
            }
        }

        public void OnBattleEnd()
        {
            foreach (var unit in AllUnits)
            {
                foreach (var effect in unit.stateEffects)
                    effect.OnBattleEnd();
            }
        }

        public static List<Unit> GetUnitsInPositions(List<Vector2Int> positions)
        {
            List<Unit> unitList = new List<Unit>();

            foreach (var item in positions)
            {
                Unit unit = instance.AllTiles[item.x, item.y].GetUnit();
                if (unit != null)
                    unitList.Add(unit);

            }

            return unitList;
        }

        public static List<Unit> GetRandomUnitsInPositions(List<Vector2Int> positions, int number)
        {
            List<Unit> unitPool = GetUnitsInPositions(positions);
            List<Unit> unitList = new List<Unit>();

            for (int i = 0; i < number; i++)
            {
                int rand = UnityEngine.Random.Range(0, unitPool.Count);
                unitList.Add(unitPool[rand]);
                unitPool.RemoveAt(rand);
            }
            return unitList;
        }

        public static List<Vector2Int> GetRandomTilesInPositions(List<Vector2Int> positions, int number)
        {
            List<Vector2Int> tilePool = positions;
            List<Vector2Int> tileList = new List<Vector2Int>();

            for (int i = 0; i < number; i++)
            {
                int rand = UnityEngine.Random.Range(0, tilePool.Count);
                tileList.Add(tilePool[rand]);
                tilePool.RemoveAt(rand);
            }
            return tileList;
        }

        class Node
        {
            public UnitPosition unitPosition;
            public UnitPosition destination;
            public Node parent;
            public int heuristicCost;
            public int fromCost;
            public int evaluationCost;

            public Node(UnitPosition unitPosition, UnitPosition destination) // root 초기화
            {
                this.unitPosition = unitPosition;
                this.destination = destination;
                parent = null;
                heuristicCost = Heuristic(unitPosition, destination);
                fromCost = 0;
                evaluationCost = heuristicCost + fromCost;
            }

            public Node(UnitPosition unitPosition, Node parent)
            {
                this.unitPosition = unitPosition;
                this.parent = parent;
                destination = parent.destination;
                heuristicCost = Heuristic(unitPosition, destination);
                fromCost = parent.fromCost + 1;
                evaluationCost = heuristicCost + fromCost;
            }

            public static Node PopSmallestCostNode(List<Node> nodeList)
            {
                if (nodeList.Count == 0)
                    return null;

                Node temp = nodeList[0];

                foreach (var item in nodeList)
                    if (temp.evaluationCost > item.evaluationCost)
                        temp = item;

                return temp;
            }

            public static List<Node> GetAvilableNeighbor(Unit unit, Node node)
            {
                List<Node> neighbor = new List<Node>();

                if (UnitPosition.UpPosition(node.unitPosition, 1).IsMovableUnitPosition(unit))
                    neighbor.Add(new Node(UnitPosition.UpPosition(node.unitPosition, 1), node));
                if (UnitPosition.DownPosition(node.unitPosition, 1).IsMovableUnitPosition(unit))
                    neighbor.Add(new Node(UnitPosition.DownPosition(node.unitPosition, 1), node));
                if (UnitPosition.RightPosition(node.unitPosition, 1).IsMovableUnitPosition(unit))
                    neighbor.Add(new Node(UnitPosition.RightPosition(node.unitPosition, 1), node));
                if (UnitPosition.LeftPosition(node.unitPosition, 1).IsMovableUnitPosition(unit))
                    neighbor.Add(new Node(UnitPosition.LeftPosition(node.unitPosition, 1), node));

                return neighbor;
            }

            public static List<UnitPosition> RebuildPath(Node current)
            {
                List<UnitPosition> totalPath = new List<UnitPosition>();
                totalPath.Add(current.unitPosition);

                while (current.parent != null)
                {
                    current = current.parent;
                    totalPath.Add(current.unitPosition);
                }

                totalPath.Reverse();

                return totalPath;
            }
            static int Heuristic(UnitPosition from, UnitPosition to)
            {
                Vector2Int fromCenter = new Vector2Int((from.lowerLeft.x + from.upperRight.x) / 2, (from.lowerLeft.y + from.upperRight.y) / 2);
                Vector2Int toCenter = new Vector2Int((to.lowerLeft.x + to.upperRight.x) / 2, (to.lowerLeft.y + to.upperRight.y) / 2);

                Vector2Int temp = fromCenter - toCenter;

                return Math.Abs(temp.x) + Math.Abs(temp.y);
            }
        }

        /// <summary>
        /// 유닛의 이동경로를 찾는 알고리즘.
        /// </summary>
        /// <param name="unit">이동 유닛</param>
        /// <param name="from">출발 위치</param>
        /// <param name="to">도착 위치</param>
        /// <returns></returns>
        public static List<UnitPosition> PathFindAlgorithm(Unit unit, UnitPosition from, UnitPosition to)
        {
            Node node = new Node(from, to);
            List<Node> frontier = new List<Node>(); // priority queue ordered by Path-Cost, with node as the only element
            List<Node> explored = new List<Node>(); // an empty set
            List<UnitPosition> path = new List<UnitPosition>();

            frontier.Add(node);

            while (true)
            {
                if (frontier.Count == 0)
                    return null; // 답이 없음.

                node = Node.PopSmallestCostNode(frontier);
                frontier.Remove(node);

                if (node.unitPosition.Equals(to)) // goal test
                {
                    //                Debug.LogError("hello");
                    return Node.RebuildPath(node);
                }

                explored.Add(node); // add node.State to explored

                foreach (var child in Node.GetAvilableNeighbor(unit, node))
                {
                    bool isExplored = false;
                    foreach (var item in explored)
                        if (item.unitPosition == child.unitPosition)
                            isExplored = true;
                    if (isExplored.Equals(true))
                        continue;

                    bool isFrontiered = false;
                    foreach (var item in frontier)
                        if (item.unitPosition.Equals(child.unitPosition))
                        {
                            isFrontiered = true;
                            if (child.unitPosition == item.unitPosition && child.evaluationCost < item.evaluationCost)
                            {
                                frontier.Remove(item);
                                frontier.Add(child);
                            }
                        }

                    if (isFrontiered.Equals(false))
                        frontier.Add(child);
                }
            }
        }


    }
}