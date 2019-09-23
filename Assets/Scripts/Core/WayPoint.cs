using System;
using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.Core
{
    public class WayPoint : MonoBehaviour
    {
        public DirectionType Direction { get; private set; }
        public Vector2 GridPosition { get; private set; }

        public ChainBlock Block { get; set; }

        public bool IsEmpty => Block != null;
        
        public void Initialize(Vector2 gridPosition, DirectionType direction)
        {
            Direction = direction;
            GridPosition = gridPosition;
        }

        private void OnDrawGizmos()
        {
            var color = Direction == DirectionType.Horizontal ? Color.red : Color.blue;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}