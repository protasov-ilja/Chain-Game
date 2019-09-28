using System;
using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.Core
{
    public class WayPoint : MonoBehaviour, IConnector
    {
        private RectTransform _rect => (RectTransform) transform;
        
        public DirectionType Direction { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        private int _connectionsCount;

        public event Action<Vector2Int> OnDisconnect;

        public int ConnectionsCount
        {
            get => _connectionsCount;
            set
            {
                _connectionsCount = value;
                IsEmpty = _connectionsCount <= 0;
            }
        }

        public ChainBlock Block
        {
            get => _block;
            set
            {
                IsEmpty = value == null;
                _block = value;
            }
        }

        public Transform Transform => transform;

        public void Connect(ChainBlock block)
        {
            if (Block != block)
            {
                block.Connector = this;
                Block = block;
                block.ConnectionType = ConnectionType.ToField;
            }
        }

        public void Disconnect(ChainBlock block)
        {
            Block = null;
            OnDisconnect?.Invoke(GridPosition);
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