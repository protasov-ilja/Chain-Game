using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.Core
{
    public class WayPoint : MonoBehaviour
    {
        private RectTransform _rect => (RectTransform) transform;
        
        public DirectionType Direction { get; private set; }
        public Vector2 GridPosition { get; private set; }

        public ChainBlock Block { get; set; }

        private bool _isEmpty = true;

        public bool IsEmpty
        {
            get => Block == null && _isEmpty;
            set => _isEmpty = value;
        }

        public void Initialize(Vector2 gridPosition, DirectionType direction)
        {
            Direction = direction;
            GridPosition = gridPosition;
        }

        private void OnDrawGizmos()
        {
            var color = Direction == DirectionType.Horizontal ? Color.red : Color.blue;
            Gizmos.color = color;
            Gizmos.DrawSphere(_rect.position, _rect.rect.width / 4);
        }
    }
}