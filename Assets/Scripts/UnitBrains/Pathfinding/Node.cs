using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UnitBrains.Pathfinding
{
    public class Node
    {
        public Vector2Int Pos;
        public static float BaseCost = 10f;
        public float DiaginalCost = BaseCost * 1.4f;
        public float Estimate;
        public float Value;
        public Node Parent;

        public Node(Vector2Int position)
        {
            Pos = position;
        }

        public void CalculateValue(bool isDiagonalMove)
        {
            Value = (isDiagonalMove ? DiaginalCost : BaseCost) + Estimate;
        }

        public void CalculateEstimate(Vector2Int targetPos)
        {
            Estimate = Math.Abs(Pos.x - targetPos.x) + Math.Abs(Pos.y - targetPos.y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Node node)
                return false;

            return Pos.x == node.Pos.x && Pos.y == node.Pos.y;
        }
    }
}
