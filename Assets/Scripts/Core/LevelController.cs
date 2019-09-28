using ProjectName.UI;
using ProjectName.Utils;
using UnityEngine;
using Zenject;

namespace ProjectName.Core
{
    public class LevelController : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        [Inject] private Config.Config _config;

        [SerializeField] private FieldManager _field;
        [SerializeField] private BlocksPanel _panel;
        [SerializeField] private Timer _timer;
        [SerializeField] private DigitalTimer _timerView;

        private int _currentLevel;

        private void Start()
        {
            _currentLevel = _gameManager.GetCurrentLevel();
            var data = DataSaver.LoadLevel(_currentLevel);
            _timerView.Initialize(_timer);
            LoadCurrentLevel(data);
        }
        
        public void LoadCurrentLevel(LevelData data)
        {
            foreach (var blockData in data.InitialBlocks)
            {
                var block = Instantiate(_config.ChainBlockPrefab, _config.ChainBlockPrefab.transform.position, Quaternion.identity);
                block.Direction = blockData.Direction;
                block.IsInitial = true;
                block.SetConnectorsState(blockData.ConnectorsState);
                block.BlocksPanel = _panel;
                block.FieldManager = _field;
                
                _field.AddInitialBlock(block, blockData.Position);
            }

            foreach (var blockData in data.BlocksOnPanel)
            {
                var block = Instantiate(_config.ChainBlockPrefab, _config.ChainBlockPrefab.transform.position, Quaternion.identity);
                block.Direction = blockData.Direction;
                block.IsInitial = false;
                block.SetConnectorsState(blockData.ConnectorsState);
                block.BlocksPanel = _panel;
                block.FieldManager = _field;

                _panel.AddBlock(block);
            }
            
            _timer.StartTimer(data.TimeLimit);
        }
    }
}