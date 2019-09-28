using System;
using System.Collections.Generic;
using ProjectName.Core.Enums;
using ProjectName.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace ProjectName.Core
{
    public class FieldManager : MonoBehaviour
    {
        [Inject] private Config.Config _config;

        [SerializeField] private Transform _cornerPoint;

        [Header("HelperForDataSaving")] [SerializeField]
        private TextMeshProUGUI _levelNumberText;

        [SerializeField] private TMP_InputField _input;
        
        private List<List<WayPoint>> _horizontalGrid = new List<List<WayPoint>>();
        private List<List<WayPoint>> _verticalGrid = new List<List<WayPoint>>();
        
        private void Awake()
        {
            CreateHorizontalGrid();
            CreateVerticalGrid();
        }

        public void AddInitialBlock(ChainBlock block, Vector2Int gridPosition)
        {
            if (block.Direction == DirectionType.Horizontal)
            {
                ConnectHorizontalBlock(block, gridPosition);
            }
            else if (block.Direction == DirectionType.Vertical)
            {
                ConnectVerticalBlock(block, gridPosition);
            }
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

                    point.OnDisconnect += DisconnectHorizontalBlock;
                    
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

                    point.OnDisconnect += DisconnectVerticalBlock;
                    
                    row.Add(point);
                }
                
                _verticalGrid.Add(row);
            }
        }

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

        #region SaveDataOnDisk

         public void SaveLevelData()
        {
            try 
            {
                Debug.Log($"Start ... {_levelNumberText.text}");
                Debug.Log("Start Saving...");
                var s = _input.text;
                DataSaver.DataSave((int)float.Parse(s), CreateLevelData());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private LevelData CreateLevelData()
        {
            Debug.Log("Creating Data...");
            var data = new LevelData();
            data.WinDataHorizontal = new List<BlockDataDTO>();
            data.WinDataVertical = new List<BlockDataDTO>();
            data.InitialBlocks = new List<BlockDataDTO>();
            data.BlocksOnPanel = new List<BlockDataDTO>();
            
            for (var i = 0; i < _horizontalGrid.Count; ++i)
            {
                for (var j = 0; j < _horizontalGrid[i].Count; ++j)
                {
                    if (_horizontalGrid[i][j].HasBlock)
                    {
                        var blockData = new BlockDataDTO();
                        var block = _horizontalGrid[i][j].Block;
                        var point = _horizontalGrid[i][j];
                        blockData.Direction = DirectionType.Horizontal;
                        blockData.Position = point.GridPosition;
                        blockData.ConnectorsState = new Vector2Int(block.FirstDirection, block.SecondDirection);
                        data.WinDataHorizontal.Add(blockData);
                        
                        if (block.IsInitial)
                        {
                            data.InitialBlocks.Add(blockData);
                        }
                        else
                        {
                            data.BlocksOnPanel.Add(blockData);
                        }
                    }
                }
            }
            
            for (var i = 0; i < _verticalGrid.Count; ++i)
            {
                for (var j = 0; j < _verticalGrid[i].Count; ++j)
                {
                    if (_verticalGrid[i][j].HasBlock)
                    {
                        var blockData = new BlockDataDTO();
                        var block = _verticalGrid[i][j].Block;
                        var point = _verticalGrid[i][j];
                        blockData.Direction = DirectionType.Vertical;
                        blockData.Position = point.GridPosition;
                        blockData.ConnectorsState = new Vector2Int(block.FirstDirection, block.SecondDirection);
                        data.WinDataVertical.Add(blockData);
                        
                        if (block.IsInitial)
                        {
                            data.InitialBlocks.Add(blockData);
                        }
                        else
                        {
                            data.BlocksOnPanel.Add(blockData);
                        }
                    }
                }
            }

            data.BlocksOnPanel = ShuffleList(data.BlocksOnPanel);

            return data;
        }
        
        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            System.Random r = new System.Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        #endregion

        private void DisconnectVerticalBlock(Vector2Int positionOnGrid)
        {
            var x = positionOnGrid.x;
            var y = positionOnGrid.y;
            
            if (IsVerticalCellExist(y + 1, x)) _verticalGrid[y + 1][x].ConnectionsCount--;
            if (IsVerticalCellExist(y - 1, x)) _verticalGrid[y - 1][x].ConnectionsCount--;
            if (IsHorizontalCellExist(y, x)) _horizontalGrid[y][x].ConnectionsCount--;
            if (IsHorizontalCellExist(y, x - 1)) _horizontalGrid[y][x - 1].ConnectionsCount--;
            if (IsHorizontalCellExist(y + 1, x)) _horizontalGrid[y + 1][x].ConnectionsCount--;
            if (IsHorizontalCellExist(y + 1, x - 1)) _horizontalGrid[y + 1][x - 1].ConnectionsCount--;
        }

        private void DisconnectHorizontalBlock(Vector2Int positionOnGrid)
        {
            var x = positionOnGrid.x;
            var y = positionOnGrid.y;
            
            if (IsHorizontalCellExist(y, x + 1)) _horizontalGrid[y][x + 1].ConnectionsCount--;
            if (IsHorizontalCellExist(y, x - 1)) _horizontalGrid[y][x - 1].ConnectionsCount--;
            if (IsVerticalCellExist(y, x)) _verticalGrid[y][x].ConnectionsCount--;
            if (IsVerticalCellExist(y, x + 1)) _verticalGrid[y][x + 1].ConnectionsCount--;
            if (IsVerticalCellExist(y - 1, x)) _verticalGrid[y - 1][x].ConnectionsCount--;
            if (IsVerticalCellExist(y - 1, x + 1)) _verticalGrid[y - 1][x + 1].ConnectionsCount--;
        }

        public bool ConnectToBlock(ChainBlock block)
        {
            if (block.Direction == DirectionType.Horizontal)
            {
                var nearestCell = GetNearestHorizontalCell(block);
                if (nearestCell.HasValue)
                {
                    var value = nearestCell.Value;
                    ConnectHorizontalBlock(block, value);
                    
                    return true;
                }
            }
            else if (block.Direction == DirectionType.Vertical)
            {
                var nearestCell = GetNearestVerticalCell(block);
                if (nearestCell.HasValue)
                {
                    var value = nearestCell.Value;
                    ConnectVerticalBlock(block, value);
                    
                    return true;
                }
            }

            return false;
        }

        private void ConnectHorizontalBlock(ChainBlock block, Vector2Int value)
        {
            var x = value.x;
            var y = value.y;
                    
            if (IsHorizontalCellExist(y, x + 1)) _horizontalGrid[y][x + 1].ConnectionsCount++;
            if (IsHorizontalCellExist(y, x - 1)) _horizontalGrid[y][x - 1].ConnectionsCount++;
            if (IsVerticalCellExist(y, x)) _verticalGrid[y][x].ConnectionsCount++;
            if (IsVerticalCellExist(y, x + 1)) _verticalGrid[y][x + 1].ConnectionsCount++;
            if (IsVerticalCellExist(y - 1, x)) _verticalGrid[y - 1][x].ConnectionsCount++;
            if (IsVerticalCellExist(y - 1, x + 1)) _verticalGrid[y - 1][x + 1].ConnectionsCount++;
                    
            Debug.Log($"<color=red>{ _horizontalGrid[y][x].GridPosition }</color>");
            var pos = _horizontalGrid[y][x].transform.position;
            _horizontalGrid[y][x].Connect(block);
                    
            block.transform.position = new Vector3(pos.x, pos.y, block.transform.position.z);
        }

        private void ConnectVerticalBlock(ChainBlock block, Vector2Int value)
        {
            var x = value.x;
            var y = value.y;
                    
            if (IsVerticalCellExist(y + 1, x)) _verticalGrid[y + 1][x].ConnectionsCount++;
            if (IsVerticalCellExist(y - 1, x)) _verticalGrid[y - 1][x].ConnectionsCount++;
            if (IsHorizontalCellExist(y, x)) _horizontalGrid[y][x].ConnectionsCount++;
            if (IsHorizontalCellExist(y, x - 1)) _horizontalGrid[y][x - 1].ConnectionsCount++;
            if (IsHorizontalCellExist(y + 1, x)) _horizontalGrid[y + 1][x].ConnectionsCount++;
            if (IsHorizontalCellExist(y + 1, x - 1)) _horizontalGrid[y + 1][x - 1].ConnectionsCount++;

            Debug.Log($"<color=red>{ _verticalGrid[y][x].GridPosition }</color>");
            var pos = _verticalGrid[y][x].transform.position;
            _verticalGrid[y][x].Connect(block);
                    
            block.transform.position = new Vector3(pos.x, pos.y, block.transform.position.z);
        }

        private Vector2Int? GetNearestVerticalCell(ChainBlock block)
        {
            var minDistance = float.MaxValue;
            Vector2Int? minDistanceCell = null;
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
                        var currentDistance = Vector3.Distance(block.transform.position,
                            _verticalGrid[i][j].transform.position);
                        if (currentDistance < minDistance && block.Rect.rect.width > currentDistance)
                        {
                            minDistance = currentDistance;
                            minDistanceCell = new Vector2Int(j, i);
                        }
                    }
                }
            }

            return minDistanceCell;
        }

        public bool CheckForWin(List<BlockDataDTO> winHorizontal, List<BlockDataDTO> winVertical)
        {
            foreach (var data in winHorizontal)
            {
                var x = data.Position.x;
                var y = data.Position.y;
                if (!_horizontalGrid[y][x].IsEmpty)
                {
                    if (!(_horizontalGrid[y][x].Block.SecondDirection == data.ConnectorsState.y
                        && _horizontalGrid[y][x].Block.FirstDirection == data.ConnectorsState.x))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            
            foreach (var data in winVertical)
            {
                var x = data.Position.x;
                var y = data.Position.y;
                if (!_verticalGrid[y][x].IsEmpty)
                {
                    if (!(_verticalGrid[y][x].Block.SecondDirection == data.ConnectorsState.y
                          && _verticalGrid[y][x].Block.FirstDirection == data.ConnectorsState.x))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private Vector2Int? GetNearestHorizontalCell(ChainBlock block)
        {
            var minDistance = float.MaxValue;
            Vector2Int? minDistanceCell = null;
            for (var i = 0; i < _horizontalGrid.Count; ++i)
            {
                for (var j = 0; j < _horizontalGrid[i].Count; ++j)
                {
                    if (_horizontalGrid[i][j].IsEmpty &&
                        IsFree(_horizontalGrid[0][0].Direction, i ,j + 1) && 
                        IsFree(_horizontalGrid[0][0].Direction, i, j - 1) &&
                        IsFree(_verticalGrid[0][0].Direction, i, j) &&
                        IsFree(_verticalGrid[0][0].Direction, i, j + 1) &&
                        IsFree(_verticalGrid[0][0].Direction, i - 1, j) &&
                        IsFree(_verticalGrid[0][0].Direction, i - 1, j + 1))
                    {
                        var currentDistance = Vector3.Distance(block.transform.position,
                            _horizontalGrid[i][j].transform.position);
                        if (currentDistance < minDistance)
                        {
                            minDistance = currentDistance;
                            minDistanceCell = new Vector2Int(j, i);
                        }
                    }
                }
            }

            return minDistanceCell;
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
    }
}