using System;
using ProjectName.Utils;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.UI.MainMenuUI
{
	public sealed class MainMenu : MonoBehaviour
	{
		[Inject] private GenericSceneManager _sceneManger;
		
		[SerializeField] private LevelsMenu _levelsMenu;
		
		#region Public Methods
		public void PressStartGameButton()
		{
			_sceneManger.LoadSceneAsync("SampleScene");
		}

		public void PressLevelsMenuButton()
		{
			_levelsMenu.Activate(this);
			gameObject.SetActive(false);
		}

		public void PressExitButton()
		{
			
		}
		#endregion
	}
}
