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

        private void Update()
        {
            
        }

        private void CreateHorizontalGrid()
        {
            _horizontalGrid = new List<WayPoint>();
            var pos = _cornerPoint.position;
            var blockTransform = _config.ChainBlockPrefab.transform;
            var startXOffset = blockTransform.lossyScale.x / 2;
            var startYOffset = blockTransform.lossyScale.y / 2;
            var cellSize = blockTransform.lossyScale.y;
            for (var i = 0; i < 4; ++i)
            {
                for (var j = 0; j < 3; ++j)
                {
                    var point = Instantiate(_config.WayPointPrefab, new Vector3(startXOffset + pos.x + cellSize * j, startYOffset + pos.y + cellSize * i, pos.z), Quaternion.identity);
                    point.Initialize(new Vector2(j, i), DirectionType.Horizontal);
                    _horizontalGrid.Add(point);
                }
            }
        }

        private void CreateVerticalGrid()
        {
            _verticalGrid = new List<WayPoint>();
            var pos = _cornerPoint.position;
            var blockTransform = _config.ChainBlockPrefab.transform;
            var startXOffset = blockTransform.lossyScale.x / 4;
            var startYOffset = blockTransform.lossyScale.y;
            var cellSize = blockTransform.lossyScale.y;
            for (var i = 0; i < 3; ++i)
            {
                for (var j = 0; j < 4; ++j)
                {
                    var point = Instantiate(_config.WayPointPrefab, new Vector3(startXOffset + pos.x + cellSize * j, startYOffset + pos.y + cellSize * i, pos.z), Quaternion.identity);
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
    }
}