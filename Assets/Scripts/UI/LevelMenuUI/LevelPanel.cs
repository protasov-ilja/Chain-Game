using System;
using System.Collections.Generic;
using ProjectName.Core;
using ProjectName.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.UI.LevelMenuUI
{
	public sealed class LevelPanel : MonoBehaviour
	{
		[Inject] private GameManager _gameManager;
		[Inject] private GenericSceneManager _sceneManager;

		[SerializeField] private int _levelNumber;
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private List<GameObject> _activeStars;

		public void Initilize(int levelNumber)
		{
			UpdatePanel(levelNumber);
			_levelNumber = levelNumber;
			var progress = _gameManager.GetLevelScore(levelNumber);
			for (var i = 0; i < _activeStars.Count; ++i)
			{
				_activeStars[i].SetActive(i < progress);
			}
		}

		private void UpdatePanel(int levelNumber)
		{
			_levelText.text = (levelNumber + 1).ToString();
		}

		public void PressStartThisLevel()
		{
			_gameManager.SetActiveLevel(_levelNumber);
			_sceneManager.LoadSceneAsync("SampleScene");
		}
	}
}
