using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.Core
{
    public class WayPoint : MonoBehaviour
    {
        private RectTransform _rect => (RectTransform) transform;
        
        public DirectionType Direction { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        public ChainBlock Block
        {
            get => _block;
            set
            {
                IsEmpty = false;
                _block = value;
            }
        }

        private ChainBlock _block;
        private bool _isEmpty = true;

        public bool IsEmpty
        {
            get => _isEmpty;
            set => _isEmpty = value;
        }

        public bool HasBlock => Block != null;

        public void Initialize(Vector2Int gridPosition, DirectionType direction)
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