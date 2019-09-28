using System;
using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.Utils
{
    [Serializable]
    public class BlockDataDTO
    {
        [SerializeField] private DirectionType _direction;
        [SerializeField] private Vector2Int _position;
        [SerializeField] private Vector2Int _connectorsState;

        public DirectionType Direction
        {
            get => _direction;
            set => _direction = value;
        }
        
        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }
        
        public Vector2Int ConnectorsState
        {
            get => _connectorsState;
            set => _connectorsState = value;
        }
    }
}