using System.Collections.Generic;
using ProjectName.Core;
using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.UI
{
    public class BlocksPanel : MonoBehaviour, IConnector
    {
        [SerializeField] private ChainBlock _verticalBlockPrefab;
        [SerializeField] private ChainBlock _horizontalBlockPrefab;
        
        private List<ChainBlock> _blocks = new List<ChainBlock>();

        public Transform Transform => transform;
        
        public void AddBlock(ChainBlock block)
        {
            Connect(block);
        }

        public ChainBlock Block { get; set; }
        public void Connect(ChainBlock block)
        {
            block.Connector = this;
            block.ConnectionType = ConnectionType.ToPanel;
        }

        public void Disconnect(ChainBlock block)
        {
        }
    }
}