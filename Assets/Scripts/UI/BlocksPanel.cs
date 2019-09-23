using System.Collections.Generic;
using ProjectName.Core;
using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.UI
{
    public class BlocksPanel : MonoBehaviour
    {
        [SerializeField] private ChainBlock _verticalBlockPrefab;
        [SerializeField] private ChainBlock _horizontalBlockPrefab;
        
        private List<ChainBlock> _blocks = new List<ChainBlock>();

        public void GenerateBlocks(IEnumerable<ChainBlock> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.Direction == DirectionType.Horizontal)
                {
                    _blocks.Add(Instantiate(_horizontalBlockPrefab, transform));
                }
                else if (block.Direction == DirectionType.Vertical)
                {
                    _blocks.Add(Instantiate(_verticalBlockPrefab, transform));
                }
            }
        }
    }
}