using Assets.Scripts.UI.LevelMenuUI;
using System.Collections.Generic;
using Assets.Scripts.UI.MainMenuUI;
using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private List<LevelPanel> _panels;
	#endregion

	private MainMenu _menu;

	#region Public Fields
	public void Activate(MainMenu menu)
	{
		gameObject.SetActive(true);
		_menu = menu;
		for (var i = 0; i < _panels.Count; ++i)
		{
			_panels[i].Initilize(i);
		}
	}

	public void OnBackButtonPressed()
	{
		_menu.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
	#endregion
}
