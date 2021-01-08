using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bolt;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<Unit> AllUnits;
    public Tile[,] AllTiles; //10x10일때 0,0 ~ 9,9으로
    public Unit thisTurnUnit;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //UI세팅
        BattleUI.instance.Init();

        //정보 저장용 타일 생성 
        AllTiles = new Tile[10, 10];
        for (int i = 0; i < AllTiles.GetLength(0); i++)
            for (int j = 0; j < AllTiles.GetLength(1); j++)
                AllTiles[i, j] = new Tile();

        //파티 유닛들 AllUnits에 추가
        foreach (Unit unit in PartyManager.instance.AllUnits)
            AllUnits.Add(unit);

        //유닛들 타일 할당
        foreach (Unit unit in AllUnits)
            AllocateUnitTiles(unit,unit.unitPosition);

        StartBattle();
    }

    public void StartBattle()
    {
        OnBattleStart();
        SetNextTurn();
    }
    public void EndBattle()
    {
        OnBattleEnd();
    }
    public Tile GetTile(Vector2Int position)
    {
        return AllTiles[position.x, position.y];
    }
    public Tile GetTile(int x, int y)
    {
        return AllTiles[x,y];
    }

    public void AllocateUnitTiles(Unit unit, UnitPosition position)
    {
        for (int i = position.lowerLeft.x; i <= position.upperRight.x; i++)
            for (int j = position.lowerLeft.y; j <= position.upperRight.y; j++)
            {
                AllTiles[i, j].SetUnit(unit);
            }
    }

    public void SetNextTurn()
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

        //누적 행동력(actionRate) 기준 정렬
       /* AllUnits.Sort(delegate(Unit A, Unit B)
        {
            if (A.actionRate < B.actionRate) return 1;
            else if (A.actionRate > B.actionRate) return -1;
            return 0;
        });*/

        TurnStart(nextUnit);
    }

    public void TurnStart(Unit unit)
    {
        thisTurnUnit = unit;
        thisTurnUnit.actionRate = 0;

        //턴 상태 갱신(이동 가능한 타일 보여주기)
        BattleUI.instance.UpdateTurnStatus(unit);
    }

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
    }

    public static List<UnitPosition> PathFindAlgorithm(Unit unit, UnitPosition from, UnitPosition to)
    {
        Node node = new Node(from, to);
        List<Node> frontier = new List<Node>(); // priority queue ordered by Path-Cost, with node as the only element
        List<Node> explored = new List<Node>(); // an empty set
        List<UnitPosition> path = new List<UnitPosition>();

        frontier.Add(node);

        while(true)
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

    public static int Heuristic(UnitPosition from, UnitPosition to)
    {
        Vector2Int fromCenter = new Vector2Int((from.lowerLeft.x + from.upperRight.x) / 2, (from.lowerLeft.y + from.upperRight.y) / 2);
        Vector2Int toCenter = new Vector2Int((to.lowerLeft.x + to.upperRight.x) / 2, (to.lowerLeft.y + to.upperRight.y) / 2);

        Vector2Int temp = fromCenter - toCenter;

        return Math.Abs(temp.x) + Math.Abs(temp.y);
    }
}
