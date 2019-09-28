using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectName.Utils
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private List<BlockDataDTO> _winDataVertical;
        [SerializeField] private List<BlockDataDTO> _winDataHorizontal;
        [SerializeField] private List<BlockDataDTO> _initialBlocks;
        [SerializeField] private List<BlockDataDTO> _blocksOnPanel;
        [SerializeField] private float _timeLimit;
        
        public List<BlockDataDTO> WinDataVertical
        {
            get => _winDataVertical;
            set => _winDataVertical = value;
        }
        public List<BlockDataDTO> WinDataHorizontal
        {
            get => _winDataHorizontal;
            set => _winDataHorizontal = value;
        }
        
        public List<BlockDataDTO> InitialBlocks
        {
            get => _initialBlocks;
            set => _initialBlocks = value;
        }
        
        public List<BlockDataDTO> BlocksOnPanel
        {
            get => _blocksOnPanel;
            set => _blocksOnPanel = value;
        }

        public float TimeLimit
        {
            get => _timeLimit;
            set => _timeLimit = value;
        }
    }
}