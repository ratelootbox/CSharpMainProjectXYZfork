using Model;
using Model.Runtime.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnitBrains;
using UnitBrains.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.UnitBrains.Pathfinding
{
    public class AlgoritmAUnitPath: BaseUnitPath
    {
        /* (-1;1)  # (0;1)  # (1;1)
         * #########################
         * (-1;0)  # (0;0)  # (1:0)
         * #########################
         * (-1;-1) # (0;-1) # (1;-1)
         * 
         * 2 # 3 # 4
         * #########
         * 1 # x # 5
         * #########
         * 8 # 7 # 6
         * 
         * 2,4,6,8 диагональные шаги
         */

        private int[] dx = {-1, -1, 0, 1, 1, 1, 0, -1};
        private int[] dy = {0, 1, 1, 1, 0, -1, -1, -1};

        public AlgoritmAUnitPath(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPoint, Vector2Int endPoint) :
            base(runtimeModel, startPoint, endPoint)
        {
        }

        protected override void Calculate()
        {
            path = FindPath().ToArray();

            if (path == null)
                path = new Vector2Int[] { startPoint };

        }

        public List<Vector2Int> FindPath()
        {
            Node startNode = new Node(startPoint);
            Node targetNode = new Node(endPoint);

            List<Node> openList = new List<Node>() { startNode };
            List<Node> closedList = new List<Node>();

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                foreach (var node in openList)
                {
                    if (node.Value < currentNode.Value)
                        currentNode = node;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (!IsNodeReachable(targetNode))
                {
                    IEnumerable<IReadOnlyUnit> enemyUnits = runtimeModel.RoBotUnits;

                    for (int i = 0; i < enemyUnits.Count() - 1; i++)
                    {
                        Node temp = new Node(enemyUnits.ToArray()[i].Pos);
                        if (IsNodeReachable(temp)) 
                        {
                            targetNode = temp;
                            break;
                        }
                    }
                }

                if (currentNode.Pos.x == targetNode.Pos.x && currentNode.Pos.y == targetNode.Pos.y)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    while (currentNode != null)
                    {
                        path.Add(currentNode.Pos);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path;
                }

                for (int i = 0; i < dx.Length; i++)
                {
                    int newX = currentNode.Pos.x + dx[i];
                    int newY = currentNode.Pos.y + dy[i];
                    bool isDiagonalMove = (i + 1) % 2 == 0;

                    Vector2Int newPos = new Vector2Int(newX, newY);

                    if (!runtimeModel.IsTileWalkable(newPos) && newPos != endPoint)
                        continue;
                   
                    Node neighbor = new Node(newPos);

                    if (closedList.Contains(neighbor))
                        continue;

                    neighbor.Parent = currentNode;
                    neighbor.CalculateEstimate(targetNode.Pos);
                    neighbor.CalculateValue(isDiagonalMove);

                    openList.Add(neighbor);
                    
                }
            }

            return null;
        }

        private bool IsNodeReachable(Node node)
        {
            bool b = false;
            for (int i = 0; i < dx.Length; i++)
            {
                int newX = node.Pos.x + dx[i];
                int newY = node.Pos.y + dy[i];

                Vector2Int newPos = new Vector2Int(newX, newY);

                b = runtimeModel.IsTileWalkable(newPos);
                if (b)
                    break;
            }

            return b;
        }
    }
}
