using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ProjectName.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int _levelsAmount;

        private int _currentLevel;

        public void SetNewLevel()
        {
            _currentLevel = PlayerPrefs.GetInt($"CurrentLevel", 0);
            _currentLevel++;
            if (_currentLevel >= _levelsAmount)
            {
                _currentLevel = 0;
            }
            
            PlayerPrefs.SetInt($"CurrentLevel", _currentLevel);
        }
        
        public int GetCurrentLevel()
        {
            return 2;PlayerPrefs.GetInt($"CurrentLevel", 0);
        }

        public void SaveLevelScore(int level, int score)
        {
            PlayerPrefs.SetInt($"Score_{level}", score);
        }

        public void SaveLevelBestTime(int level, float time)
        {
            var currentBestTime = GetLevelBestTime(level);
            if (time < currentBestTime)
            {
                PlayerPrefs.SetFloat($"BestTime_{level}", time);
            }
        }

        public float GetLevelBestTime(int level)
        {
            return PlayerPrefs.GetFloat($"BestTime_{level}", 10000);
        }
    }
}