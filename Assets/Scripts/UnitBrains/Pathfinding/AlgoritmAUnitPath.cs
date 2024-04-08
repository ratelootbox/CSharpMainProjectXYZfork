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
        private int[] dx = {-1, 0, 1, 0};
        private int[] dy = {0, 1, 0, -1};

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

                    Vector2Int newPos = new Vector2Int(newX, newY);

                    if (!runtimeModel.IsTileWalkable(newPos) && newPos != endPoint)
                        continue;
                   
                    Node neighbor = new Node(newPos);

                    if (closedList.Contains(neighbor))
                        continue;

                    neighbor.Parent = currentNode;
                    neighbor.CalculateEstimate(targetNode.Pos);
                    neighbor.CalculateValue();

                    openList.Add(neighbor);
                    
                }
            }

            return null;
        }
    }
}
