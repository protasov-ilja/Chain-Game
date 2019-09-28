using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.LevelMenuUI
{
	public sealed class LevelPanel : MonoBehaviour
	{
		const string ProgressKey = "LevelProgress"; 

		[SerializeField] private int _levelNumber;
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private List<GameObject> _activeStars;

		public event Action<int> ChooseLevelButtonPressed;

		public void Initilize(int levelNumber)
		{
			_levelNumber = levelNumber;
			var progress = PlayerPrefs.GetInt($"{ProgressKey}{levelNumber}", 0);
			for (var i = 0; i < _activeStars.Count; ++i)
			{
				_activeStars[i].SetActive(i < progress);
			}
		}

		public void UpdatePanel(int levelNumber)
		{
			_levelText.text = levelNumber.ToString();
		}

		public void PressStartThisLevel()
		{
			ChooseLevelButtonPressed?.Invoke(_levelNumber);
		}

		public void UpdateState()
		{
			var progress = PlayerPrefs.GetInt($"{ProgressKey}{_levelNumber}", 0);
			for (var i = 0; i < _activeStars.Count; ++i)
			{
				_activeStars[i].SetActive(i < progress);
			}
		}
	}
}
