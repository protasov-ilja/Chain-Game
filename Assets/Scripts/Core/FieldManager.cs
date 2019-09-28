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
        
        private List<List<WayPoint>> _horizontalGrid = new List<List<WayPoint>>();
        private List<List<WayPoint>> _verticalGrid = new List<List<WayPoint>>();
        
        private void Awake()
        {
            CreateHorizontalGrid();
            CreateVerticalGrid();
        }

        private void CreateHorizontalGrid()
        {
            _horizontalGrid = new List<List<WayPoint>>();
            var pos = _cornerPoint.position;
            var blockTransform = _config.ChainBlockPrefab.Rect.rect;
            var startXOffset = blockTransform.width / 2;
            var startYOffset = blockTransform.height / 2;
            var cellSize = blockTransform.height;
            for (var i = 0; i < 4; ++i)
            {
                var row = new List<WayPoint>();
                for (var j = 0; j < 3; ++j)
                {
                    var point = Instantiate(_config.WayPointPrefab, transform);
                    point.transform.position = new Vector3(startXOffset + pos.x + cellSize * j,
                        startYOffset + pos.y + cellSize * i, pos.z);
                    point.Initialize(new Vector2Int(j, i), DirectionType.Horizontal);
                    
                    row.Add(point);
                }
                
                _horizontalGrid.Add(row);
            }
        }

        private void CreateVerticalGrid()
        {
            _verticalGrid = new List<List<WayPoint>>();
            var pos = _cornerPoint.position;
            
            var blockTransform = _config.ChainBlockPrefab.Rect.rect;
            Debug.Log(blockTransform.width);
            var startXOffset = blockTransform.width / 4;
            var startYOffset = blockTransform.height;
            var cellSize = blockTransform.height;
            for (var i = 0; i < 3; ++i)
            {
                var row = new List<WayPoint>();
                for (var j = 0; j < 4; ++j)
                {
                    var point = Instantiate(_config.WayPointPrefab, transform);
                    point.transform.position = new Vector3(startXOffset + pos.x + cellSize * j, startYOffset + pos.y + cellSize * i,
                        pos.z);
                    
                    point.Initialize(new Vector2Int(j, i), DirectionType.Vertical);

                    row.Add(point);
                }
                
                _verticalGrid.Add(row);
            }
        }

//        public Vector3 FindNearestPoint(Vector3 target, DirectionType direction)
//        {
//            var list = (direction == DirectionType.Horizontal) ? _horizontalGrid : _verticalGrid;
//            var minDistance = float.MaxValue;
//            Vector3 nearestPosition = Vector3.positiveInfinity;
//            for (var i = 0; i < list.Count; ++i)
//            {
//                var distance = Vector3.Distance(list[i].transform.position, target);
//                if (distance < minDistance)
//                {
//                    minDistance = distance;
//                    nearestPosition = list[i].transform.position;
//                }
//            }
//
//            return nearestPosition;
//        }

        private bool IsFree(DirectionType type, int yPosition, int xPosition)
        {
            switch (type)
            {
                case DirectionType.Horizontal:
                    if (IsHorizontalCellExist(yPosition, xPosition))
                    {
                        return !_horizontalGrid[yPosition][xPosition].HasBlock;
                    };

                    return true;
                case DirectionType.Vertical:
                    if (IsVerticalCellExist(yPosition, xPosition))
                    {
                        return !_verticalGrid[yPosition][xPosition].HasBlock;
                    }

                    return true;
            }
            
            return false;
        }

        private bool IsHorizontalCellExist(int yPosition, int xPosition)
        {
            return ((yPosition >= 0 && yPosition < _horizontalGrid.Count) &&
                    (xPosition >= 0 && xPosition < _horizontalGrid[yPosition].Count));
        }
        
        private bool IsVerticalCellExist(int yPosition, int xPosition)
        {
            return ((yPosition >= 0 && yPosition < _verticalGrid.Count) &&
                    (xPosition >= 0 && xPosition < _verticalGrid[yPosition].Count));
        }
        
        public void ConnectToBlock(ChainBlock block)
        {
            if (block.Direction == DirectionType.Horizontal)
            {
                for (var i = 0; i < _horizontalGrid.Count; ++i)
                {
                    for (var j = 0; j < _horizontalGrid[i].Count; ++j)
                    {
                        if (_horizontalGrid[i][j].IsEmpty &&
                            IsFree(_horizontalGrid[0][0].Direction, i ,j + 1) && 
                            IsFree(_horizontalGrid[0][0].Direction, i, j - 1) &&
                            IsFree(_verticalGrid[0][0].Direction, i, j + 1) &&
                            IsFree(_verticalGrid[0][0].Direction, i, j) &&
                            IsFree(_verticalGrid[0][0].Direction, i - 1, j) &&
                            IsFree(_verticalGrid[0][0].Direction, i - 1, j + 1))
                        {
                            if (IsHorizontalCellExist(i, j + 1))
                            {
                                Debug.Log("H :" + _horizontalGrid[i][j + 1].GridPosition);
                                _horizontalGrid[i][j + 1].IsEmpty = false;
                            }
                            
                            if (IsHorizontalCellExist(i, j - 1))
                            {
                                Debug.Log("H :" + _horizontalGrid[i][j - 1].GridPosition);
                                _horizontalGrid[i][j - 1].IsEmpty = false;
                            }
                            
                            if (IsVerticalCellExist(i, j + 1))
                            {
                                Debug.Log("V :" + _verticalGrid[i][j + 1].GridPosition);
                                _verticalGrid[i][j + 1].IsEmpty = false;
                            }

                            if  (IsVerticalCellExist(i, j))
                            {
                                Debug.Log("V :" + _verticalGrid[i][j].GridPosition);
                                _verticalGrid[i][j].IsEmpty = false;
                            }

                            if (IsVerticalCellExist(i - 1, j))
                            {
                                Debug.Log("V :" + _verticalGrid[i - 1][j].GridPosition);
                                _verticalGrid[i - 1][j].IsEmpty = false;
                            }
                            
                            if (IsVerticalCellExist(i - 1, j + 1))
                            {
                                Debug.Log("V :" + _verticalGrid[i - 1][j + 1].GridPosition);
                                _verticalGrid[i - 1][j + 1].IsEmpty = false;
                            }
                            
                            Debug.Log($"<color=red>{ _horizontalGrid[i][j].GridPosition }</color>");
                            var pos = _horizontalGrid[i][j].transform.position;
                            _horizontalGrid[i][j].Block = block;
                            block.transform.position = new Vector3(pos.x, pos.y, block.transform.position.z);

                            return;
                        }
                    }
                }
            }
            else if (block.Direction == DirectionType.Vertical)
            {
                for (var i = 0; i < _verticalGrid.Count; ++i)
                {
                    for (var j = 0; j < _verticalGrid[i].Count; ++j)
                    {
                        if (_verticalGrid[i][j].IsEmpty &&
                            IsFree(_verticalGrid[0][0].Direction, i + 1 ,j) && 
                            IsFree(_verticalGrid[0][0].Direction, i - 1, j) &&
                            IsFree(_horizontalGrid[0][0].Direction, i, j) &&
                            IsFree(_horizontalGrid[0][0].Direction, i, j - 1) &&
                            IsFree(_horizontalGrid[0][0].Direction, i + 1, j) &&
                            IsFree(_horizontalGrid[0][0].Direction, i + 1, j - 1))
                        {
                            if (IsVerticalCellExist(i + 1, j))
                            {
                                Debug.Log("H :" + _verticalGrid[i + 1][j].GridPosition);
                                _verticalGrid[i + 1][j].IsEmpty = false;
                            }
                            
                            if (IsVerticalCellExist(i - 1, j))
                            {
                                Debug.Log("H :" + _verticalGrid[i - 1][j].GridPosition);
                                _verticalGrid[i - 1][j].IsEmpty = false;
                            }
                            
                            if (IsHorizontalCellExist(i, j))
                            {
                                Debug.Log("V :" + _horizontalGrid[i][j].GridPosition);
                                _horizontalGrid[i][j].IsEmpty = false;
                            }

                            if (IsHorizontalCellExist(i, j - 1))
                            {
                                Debug.Log("V :" + _horizontalGrid[i][j - 1].GridPosition);
                                _horizontalGrid[i][j - 1].IsEmpty = false;
                            }

                            if (IsHorizontalCellExist(i + 1, j))
                            {
                                Debug.Log("V :" + _horizontalGrid[i + 1][j].GridPosition);
                                _horizontalGrid[i + 1][j].IsEmpty = false;
                            }
                            
                            if (IsHorizontalCellExist(i + 1, j - 1))
                            {
                                Debug.Log("V :" + _horizontalGrid[i + 1][j - 1].GridPosition);
                                _horizontalGrid[i + 1][j - 1].IsEmpty = false;
                            }
                            
                            Debug.Log($"<color=red>{ _verticalGrid[i][j].GridPosition }</color>");
                            var pos = _verticalGrid[i][j].transform.position;
                            _verticalGrid[i][j].Block = block;
                            block.transform.position = new Vector3(pos.x, pos.y, block.transform.position.z);

                            return;
                        }
                    }
                }
            }
        }
    }
}