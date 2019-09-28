using System;
using UnityEngine;

namespace Assets.Scripts.UI.MainMenuUI
{
	public sealed class MainMenu : MonoBehaviour
	{
		#region Editor Fields
		#endregion

		#region Private Fields
		#endregion

		#region Public Fields
		public event Action StartGameButtonPressed;
		public event Action OptionsButtonPressed;
		public event Action LevelsMenuButtonPressed;
		public event Action ExitButtonPressed;
		#endregion

		#region Unity Methods
		#endregion

		#region Public Methods
		public void PressStartGameButton()
		{
			StartGameButtonPressed?.Invoke();
		}

		public void PressOptionsButton()
		{
			OptionsButtonPressed?.Invoke();
		}

		public void PressLevelsMenuButton()
		{
			LevelsMenuButtonPressed?.Invoke();
		}

		public void PressExitButton()
		{
			ExitButtonPressed?.Invoke();
		}
		#endregion

		#region Private Methods
		#endregion
	}
}
