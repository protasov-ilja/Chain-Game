using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Interface
{
	public sealed class UICell : MonoBehaviour
	{
		[SerializeField] private Image _image;

		//public CellState State { get; set; }

		public Color ImgaeColor
		{
			get => _image.color;
			set => _image.color = value;
		}
	}
}
