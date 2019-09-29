﻿using System;
using System.Net.Mime;
using ProjectName.UI;
using ProjectName.UI.WinScreenUI;
using ProjectName.Utils;
using TMPro;
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
        [SerializeField] private LooseScreen _looseScreen;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private TextMeshProUGUI _bestTimeText;
        
        private LevelData _levelData;
        private int _currentLevel;
        private double _levelTime;
        private float _bestTime;

        private void Start()
        {
            _currentLevel = _gameManager.GetCurrentLevel();
            _bestTime = _gameManager.GetLevelBestTime(_currentLevel);
            _bestTimeText.text = GetTime(_bestTime);
            _levelData = DataSaver.LoadLevel(_currentLevel);
            _timerView.Initialize(_timer);
            LoadCurrentLevel(_levelData);
            _timer.OnTimerEnd(ShowLooseScreen);
            _timer.OnTimerStop(SaveTime);
        }

        private string GetTime(float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            var minute = LeadingZero( timeSpan.Minutes);
            var second = LeadingZero(timeSpan.Seconds + 1);
            
            return $"{ minute }:{ second }";
        }
        
        private static string LeadingZero(int number)
        {
            if (number < 0) number = 0;
            
            return number.ToString().PadLeft(2, '0');
        }
        
        private void LoadCurrentLevel(LevelData data)
        {
            foreach (var blockData in data.InitialBlocks)
            {
                var block = GetNewChainBlock(blockData, true);
                _field.AddInitialBlock(block, blockData.Position);
            }

            foreach (var blockData in data.BlocksOnPanel)
            {
                var block = GetNewChainBlock(blockData, false);
                _panel.AddBlock(block);
            }
            
            _timer.StartTimer(data.TimeLimit);
        }

        private ChainBlock GetNewChainBlock(BlockDataDTO data, bool isInitial)
        {
            var block = Instantiate(_config.ChainBlockPrefab, _config.ChainBlockPrefab.transform.position, Quaternion.identity);
            block.Direction = data.Direction;
            block.IsInitial = isInitial;
            block.SetConnectorsState(data.ConnectorsState);
            block.BlocksPanel = _panel;
            block.FieldManager = _field;
            block.OnStateChange += OnFieldStateChanged;

            return block;
        }

        private void SaveTime(TimerVO timerVo)
        {
            _levelTime = timerVo.Time.TotalSeconds;
            _gameManager.SaveLevelBestTime(_currentLevel, (float)_levelTime);
        }
        
        private void OnFieldStateChanged()
        {
            if (_field.CheckForWin(_levelData.WinDataHorizontal, _levelData.WinDataVertical))
            {
                _timer.StopTimer();
                ShowWinScreen();
            }
        }

        private void ShowLooseScreen()
        {
            _looseScreen.Activate();
        }

        private void ShowWinScreen()
        {
            _winScreen.Activate((float)_levelTime, _gameManager.GetLevelBestTime(_currentLevel), 3); // TODO: paste here logic of score calculation
        }

        public void ResumeGame()
        {
            _timer.ResumeTimer();
        }

        public void ShowPauseScreen()
        {
            _timer.PauseTimer();

            _pauseScreen.Activate(this);
        }
    }
}