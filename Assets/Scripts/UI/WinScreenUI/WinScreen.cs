using System;
using System.Collections.Generic;
using System.Threading;
using ProjectName.Core;
using ProjectName.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace ProjectName.UI.WinScreenUI
{
    public class WinScreen : MonoBehaviour
    {
        private const int FAULT = 1;
        
        [Inject] private GenericSceneManager _sceneManager;
        [Inject] private GameManager _gameManager;
        
        [SerializeField] private TextMeshProUGUI _currentTime;
        [SerializeField] private TextMeshProUGUI _bestTime;
        [SerializeField] private List<GameObject> _activeStars;
        
        public void Activate(float currentTime, float bestTime, int score)
        {
            Thread.Sleep(800);
            gameObject.SetActive(true);

            _currentTime.text = GetTime(currentTime);
            _bestTime.text = GetTime(bestTime);
            
            for (var i = 0; i < _activeStars.Count; ++i)
            {
                _activeStars[i].SetActive(i < score);
            }
        }

        private string GetTime(float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            var minute = LeadingZero( timeSpan.Minutes);
            var second = LeadingZero(timeSpan.Seconds + FAULT);
            
            return $"{ minute }:{ second }";
        }
        
        private static string LeadingZero(int number)
        {
            if (number < 0) number = 0;
            
            return number.ToString().PadLeft(2, '0');
        }

        public void OnNextLevelButtonPressed()
        {
            _gameManager.SetNewLevel();
            _sceneManager.LoadSceneAsync("SampleScene");
        }

        public void OnRetryLevelButtonPressed()
        {
            _sceneManager.LoadSceneAsync("SampleScene");
        }
    }
}