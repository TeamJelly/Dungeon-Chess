using System.Collections;
using UnityEngine;
using Model.Managers;
using System.Collections.Generic;
using Model.Skills.Move;
using Model;

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

            public static List<Node> GetAvilableNeighbor(/*Model.Unit agent,*/ MoveSkill moveSkill, Node center)
            {
                List<Node> neighbor = new List<Node>();
                
                List<Vector2Int> positions = moveSkill.GetAvlPositions(center.unitPosition);

                foreach(Vector2Int position in positions)
                {
                    // Debug.Log(center.unitPosition + "?? " + position);
                    neighbor.Add(new Node(position, center));
                }

                return neighbor;
            }

            public static List<Node> GetAvilableNeighbor(Model.Unit agent, Node node)
            {
                List<Node> neighbor = new List<Node>();

                Vector2Int[] UDLR = {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
                Vector2Int[] diagonals = {
                    Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right,
                    Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right
                };

                List<Vector2Int> positions = new List<Vector2Int>();

                foreach (Vector2Int vector in UDLR)
                    positions.Add(node.unitPosition + vector);

                foreach (Vector2Int vector in diagonals)
                    positions.Add(node.unitPosition + vector);

                foreach (Vector2Int position in positions)
                    if (FieldManager.IsInField(position) && FieldManager.GetTile(position).IsPositionable(agent))
                        neighbor.Add(new Node(position, node));

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

                // string path = "";
                // foreach (var position in totalPath)
                //     path += position + " > ";                    
                // Debug.Log("path : " + path);

                return totalPath;
            }
            static int Heuristic(Vector2Int from, Vector2Int to)
            {
                Vector2Int temp = from - to;
                return Mathf.Abs(temp.x) + Mathf.Abs(temp.y);
            }
        }

        /// <summary>
        /// 목적지에서 가까운 갈수있는 타일을 찾는다.
        /// </summary>
        /// <param name="unit">목적지에 가는 주체</param>
        /// <param name="dest">목적지</param>
        /// <returns></returns>
        public static Tile GetClosestReachableDest(Unit unit, Vector2Int dest)
        {
            Tile curTile = FieldManager.GetTileClamp(dest);

            List<Tile> frontier = new List<Tile>() {curTile};
            List<Tile> visited = new List<Tile>();

            Vector2Int [] directions = {Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left};

            int loop_count = 0;

            while (true)
            {
                InfiniteLoopDebug.Run("Pathfind", "GetClosestReachableDest", 140);
                // 무한루프 방지용
                loop_count++;
                if (frontier.Count == 0 || loop_count > 1000)
                {
                    Debug.Log("갈수 없거나 계산이 너무 오래걸립니다!");
                    return null;
                }

                curTile = frontier[0];
                visited.Add(curTile);
                frontier.RemoveAt(0);

                // if (curTile.IsPositionable(unit))
                if (PathFindAlgorithm(/*unit,*/ unit.MoveSkill, unit.Position, curTile.position) != null)
                    return curTile;

                foreach (Vector2Int direction in directions)
                {
                    Tile tile = FieldManager.GetTile(curTile.position + direction);
                    if (tile != null && !visited.Contains(tile))
                        frontier.Add(tile);
                }
            }
        }


        /// <summary>
        /// 스킬을 사용한 길찾기 알고리즘
        /// </summary>
        /// <param name="agent">길을 찾을 유닛</param>
        /// <param name="moveSkill">유닛이 사용할 이동스킬</param>
        /// <param name="from">시작위치</param>
        /// <param name="to">목적지</param>
        /// <returns></returns>
        public static List<Vector2Int> PathFindAlgorithm(/*Model.Unit agent,*/ MoveSkill moveSkill, Vector2Int from, Vector2Int to)
        {            
            if (FieldManager.GetTile(to) == null || FieldManager.GetTile(to).HasUnit())
            {
                // Debug.LogWarning("길찾기 알고리즘 오류");
                return null;
            }

            Node node = new Node(from, to);
            List<Node> frontier = new List<Node>(); // priority queue ordered by Path-Cost, with node as the only element
            List<Node> explored = new List<Node>(); // an empty set

            frontier.Add(node);

            while (true)
            {
                InfiniteLoopDebug.Run("Pathfind", "GetClosestReachableDest", 190);
                if (frontier.Count == 0)
                {
                    // Debug.Log("목적지에 갈수 있는 길이 존재하지 않습니다.");
                    return null; // 답이 없음.
                }

                node = Node.PopSmallestCostNode(frontier);
                frontier.Remove(node);

                if (node.unitPosition.Equals(to)) // goal test
                    return Node.RebuildPath(node);

                explored.Add(node); // add node.State to explored

                foreach (var neighbor in Node.GetAvilableNeighbor(/*agent,*/ moveSkill, node))
                {
                    bool isExplored = false;
                    foreach (var item in explored)
                        if (item.unitPosition == neighbor.unitPosition)
                            isExplored = true;
                    if (isExplored.Equals(true))
                        continue;

                    bool isFrontiered = false;

                    for (int i = frontier.Count -1; i >= 0; i--)
                        if (frontier[i].unitPosition.Equals(neighbor.unitPosition))
                        {
                            isFrontiered = true;
                            if (neighbor.unitPosition == frontier[i].unitPosition && 
                                neighbor.evaluationCost < frontier[i].evaluationCost)
                            {
                                frontier.Remove(frontier[i]);
                                frontier.Add(neighbor);
                            }
                        }

                    if (isFrontiered.Equals(false))
                        frontier.Add(neighbor);
                }
            }
        }

        /// <summary>
        /// 유닛의 이동경로를 찾는 알고리즘.
        /// </summary>
        /// <param name="unit">이동 유닛</param>
        /// <param name="from">출발 위치</param>
        /// <param name="to">도착 위치</param>
        /// <returns></returns>
        public static List<Vector2Int> PathFindAlgorithm(Model.Unit agent, Vector2Int from, Vector2Int to)
        {
            if (FieldManager.GetTile(to) == null || FieldManager.GetTile(to).HasUnit())
            {
                Debug.LogWarning("길찾기 알고리즘 오류");
                return null;
            }

            Node node = new Node(from, to);
            List<Node> frontier = new List<Node>(); // priority queue ordered by Path-Cost, with node as the only element
            List<Node> explored = new List<Node>(); // an empty set

            frontier.Add(node);

            while (true)
            {

                InfiniteLoopDebug.Run("Pathfind", "GetClosestReachableDest", 258);
                if (frontier.Count == 0)
                {
                    Debug.Log("목적지에 갈수 있는 길이 존재하지 않습니다.");
                    return null; // 답이 없음.
                }

                node = Node.PopSmallestCostNode(frontier);
                frontier.Remove(node);

                if (node.unitPosition.Equals(to)) // goal test
                    return Node.RebuildPath(node);

                explored.Add(node); // add node.State to explored

                foreach (var child in Node.GetAvilableNeighbor(agent, node))
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
                            if (child.unitPosition == frontier[i].unitPosition && 
                                child.evaluationCost < frontier[i].evaluationCost)
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