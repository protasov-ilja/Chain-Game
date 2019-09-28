using System;

namespace Assets.Scripts.UI.Interface
{
	public interface IInterface
	{
		void UpdateStepsCounter(int maxCount, int currentCount);
		void ShowWonPanel(int score);
		void ShowGameOverPanel(int score, int bestScore);
		void SetCurrentLevel(int levelNumber);
		event Action PousePressed;
		event Action PouseReleased;
		event Action RestartGamePressed;
		event Action EndGamePressed;
	}
}
