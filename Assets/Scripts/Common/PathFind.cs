using System.Collections;
using UnityEngine;
using Model.Managers;
using System.Collections.Generic;

namespace Common
{
    public class PathFind
    {
        class Node
        {
            public Vector2Int unitPosition;
            public Vector2Int destination;
            public Node parent;
            public int heuristicCost;
            public int fromCost;
            public int evaluationCost;

            public Node(Vector2Int unitPosition, Vector2Int destination) // root 초기화
            {
                this.unitPosition = unitPosition;
                this.destination = destination;
                parent = null;
                heuristicCost = Heuristic(unitPosition, destination);
                fromCost = 0;
                evaluationCost = heuristicCost + fromCost;
            }

            public Node(Vector2Int unitPosition, Node parent)
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

            public static List<Node> GetAvilableNeighbor(Node node)
            {
                List<Node> neighbor = new List<Node>();

                Vector2Int[] vector2Ints = {
                    node.unitPosition + Vector2Int.up,
                    node.unitPosition + Vector2Int.down,
                    node.unitPosition + Vector2Int.right,
                    node.unitPosition + Vector2Int.left,
                };

                foreach (var item in vector2Ints)
                    if (BattleManager.GetTile(item) != null && BattleManager.GetUnit(item) == null)
                        neighbor.Add(new Node(item, node));

                return neighbor;
            }

            public static List<Vector2Int> RebuildPath(Node current)
            {
                List<Vector2Int> totalPath = new List<Vector2Int>();
                totalPath.Add(current.unitPosition);

                while (current.parent != null)
                {
                    current = current.parent;
                    totalPath.Add(current.unitPosition);
                }
                totalPath.Reverse();

                return totalPath;
            }
            static int Heuristic(Vector2Int from, Vector2Int to)
            {
                Vector2Int temp = from - to;
                return Mathf.Abs(temp.x) + Mathf.Abs(temp.y);
            }
        }

        /// <summary>
        /// 유닛의 이동경로를 찾는 알고리즘.
        /// </summary>
        /// <param name="unit">이동 유닛</param>
        /// <param name="from">출발 위치</param>
        /// <param name="to">도착 위치</param>
        /// <returns></returns>
        public static List<Vector2Int> PathFindAlgorithm(Vector2Int from, Vector2Int to)
        {
            if (!BattleManager.GetTile(to).IsUsable())
            {
                Debug.LogWarning("목적지에 유닛이 존재합니다.");
                return null;
            }

            Node node = new Node(from, to);
            List<Node> frontier = new List<Node>(); // priority queue ordered by Path-Cost, with node as the only element
            List<Node> explored = new List<Node>(); // an empty set

            frontier.Add(node);

            while (true)
            {
                if (frontier.Count == 0)
                    return null; // 답이 없음.

                node = Node.PopSmallestCostNode(frontier);
                frontier.Remove(node);

                if (node.unitPosition.Equals(to)) // goal test
                    return Node.RebuildPath(node);

                explored.Add(node); // add node.State to explored

                foreach (var child in Node.GetAvilableNeighbor(node))
                {
                    bool isExplored = false;
                    foreach (var item in explored)
                        if (item.unitPosition == child.unitPosition)
                            isExplored = true;
                    if (isExplored.Equals(true))
                        continue;

                    bool isFrontiered = false;

                    for (int i = frontier.Count -1; i >= 0; i--)
                        if (frontier[i].unitPosition.Equals(child.unitPosition))
                        {
                            isFrontiered = true;
                            if (child.unitPosition == frontier[i].unitPosition && child.evaluationCost < frontier[i].evaluationCost)
                            {
                                frontier.Remove(frontier[i]);
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