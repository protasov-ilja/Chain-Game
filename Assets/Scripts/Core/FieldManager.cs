using System.Collections.Generic;
using ProjectName.Core.Enums;
using UnityEngine;
using Zenject;

namespace ProjectName.Core
{
    public class FieldManager : MonoBehaviour
    {
        [Inject] private Config.Config _config;

        [SerializeField] private Transform _cornerPoint;
        
        private List<WayPoint> _horizontalGrid = new List<WayPoint>();
        private List<WayPoint> _verticalGrid = new List<WayPoint>();
        
        private void Awake()
        {
            CreateHorizontalGrid();
            CreateVerticalGrid();
        }

        private void CreateHorizontalGrid()
        {
            _horizontalGrid = new List<WayPoint>();
            var pos = _cornerPoint.position;
            var blockTransform = _config.ChainBlockPrefab.Rect.rect;
            var startXOffset = blockTransform.width / 2;
            var startYOffset = blockTransform.height / 2;
            var cellSize = blockTransform.height;
            for (var i = 0; i < 4; ++i)
            {
                for (var j = 0; j < 3; ++j)
                {
                    var point = Instantiate(_config.WayPointPrefab, transform);
                    point.transform.position = new Vector3(startXOffset + pos.x + cellSize * j,
                        startYOffset + pos.y + cellSize * i, pos.z);
                    point.Initialize(new Vector2(j, i), DirectionType.Horizontal);
                    
                    _horizontalGrid.Add(point);
                }
            }
        }

        private void CreateVerticalGrid()
        {
            _verticalGrid = new List<WayPoint>();
            var pos = _cornerPoint.position;
            
            var blockTransform = _config.ChainBlockPrefab.Rect.rect;
            Debug.Log(blockTransform.width);
            var startXOffset = blockTransform.width / 4;
            var startYOffset = blockTransform.height;
            var cellSize = blockTransform.height;
            for (var i = 0; i < 3; ++i)
            {
                for (var j = 0; j < 4; ++j)
                {
                    var point = Instantiate(_config.WayPointPrefab, transform);
                    point.transform.position = new Vector3(startXOffset + pos.x + cellSize * j, startYOffset + pos.y + cellSize * i,
                        pos.z);
                    
                    point.Initialize(new Vector2(j, i), DirectionType.Vertical);

                    _verticalGrid.Add(point);
                }
            }
        }

        public Vector3 FindNearestPoint(Vector3 target, DirectionType direction)
        {
            var list = (direction == DirectionType.Horizontal) ? _horizontalGrid : _verticalGrid;
            var minDistance = float.MaxValue;
            Vector3 nearestPosition = Vector3.positiveInfinity;
            for (var i = 0; i < list.Count; ++i)
            {
                var distance = Vector3.Distance(list[i].transform.position, target);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPosition = list[i].transform.position;
                }
            }

            return nearestPosition;
        }

        public void ConnectToBlock(ChainBlock block)
        {
            var x = 3;
            var y = 4;
            if (block.Direction == DirectionType.Horizontal)
            {
                for (var i = 0; i < _horizontalGrid.Count; ++i)
                {
                    if (_horizontalGrid[i].IsEmpty)
                    {
                        var pos = _horizontalGrid[i].transform.position;
                        Debug.Log(pos);
                        _horizontalGrid[i].Block = block;
                        block.transform.position = new Vector3(pos.x, pos.y, block.transform.position.z);
                        break;
                    }
                }
            }
            else if (block.Direction == DirectionType.Vertical)
            {
                for (var i = 0; i < _horizontalGrid.Count; ++i)
                {
                    if (_verticalGrid[i].IsEmpty)
                    {
                        var pos = _verticalGrid[i].transform.position;
                        Debug.Log(pos);
                        _verticalGrid[i].Block = block;
                        block.transform.position = new Vector3(pos.x, pos.y, block.transform.position.z);
                        break;
                    }
                }
            }
        }
    }
}