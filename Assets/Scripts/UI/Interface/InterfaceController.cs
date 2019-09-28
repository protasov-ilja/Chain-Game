using Assets.Scripts.UI.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class InterfaceController : MonoBehaviour, IInterface
{
	#region Editor Fields
	[SerializeField] private GameObject _pausePanel;
	[SerializeField] private GameObject _wonPanel;
	[SerializeField] private GameObject _losePanel;
	[SerializeField] private TextMeshProUGUI _currentCount;
	[SerializeField] private TextMeshProUGUI _maxCount;
	[SerializeField] private TextMeshProUGUI _currentLevelText;
	[Header("Win Panel")]
	[SerializeField] private List<GameObject> _activeStars;
	[SerializeField] private TextMeshProUGUI _scoreText;

	[Header("Lose Panel")]
	[SerializeField] private TextMeshProUGUI _maxScoreText;
	[SerializeField] private TextMeshProUGUI _bsetScoreText;
	[SerializeField] private GameObject _bsetScorePanel;

	#endregion

	#region Private Fields
	#endregion

	public event Action PousePressed;
	public event Action RestartGamePressed;
	public event Action EndGamePressed;
	public event Action PouseReleased;
	public event Action LoadNextLevelPressed;

	#region Unity Methods
	#endregion

	#region Public Methods

	public void PressLoadNextLevel()
	{
		LoadNextLevelPressed?.Invoke();
		_wonPanel.SetActive(false);
		_losePanel.SetActive(false);
		_pausePanel.SetActive(false);
	}

	public void PressRestartButton()
	{
		RestartGamePressed?.Invoke();
		_wonPanel.SetActive(false);
		_losePanel.SetActive(false);
		_pausePanel.SetActive(false);
	}

	public void PressEndGameButton()
	{
		EndGamePressed?.Invoke();
		_wonPanel.SetActive(false);
		_losePanel.SetActive(false);
		_pausePanel.SetActive(false);
		gameObject.SetActive(false);
	}

	public void PressReturnToGameButton()
	{
		PouseReleased?.Invoke();
		_pausePanel.SetActive(false);
	}

	public void PressPauseButton()
	{
		PousePressed?.Invoke();
		_pausePanel.SetActive(true);
	}

	public void ShowGameOverPanel(int score, int bestScore)
	{
		_losePanel.SetActive(true);

		_bsetScorePanel.SetActive(score > bestScore);

		_bsetScoreText.text = bestScore.ToString();
		_maxScoreText.text = score.ToString();
	}

	public void ShowWonPanel(int score)
	{
		Thread.Sleep(800);
		_wonPanel.SetActive(true);
		_scoreText.text = score.ToString();

		for (var i = 0; i < _activeStars.Count; ++i)
		{
			_activeStars[i].SetActive(i < score);
		}
	}

	public void UpdateStepsCounter(int maxCount, int currentCount)
	{
		_currentCount.text = currentCount.ToString();
		_maxCount.text = maxCount.ToString();
	}

	public void SetCurrentLevel(int levelNumber)
	{
		_currentLevelText.text = levelNumber.ToString();
	}
	#endregion

	#region Private Methods
	#endregion
}
