using ProjectName.Core;
using ProjectName.Utils;
using UnityEngine;
using Zenject;

namespace ProjectName.UI.WinScreenUI
{
    public class PauseScreen : MonoBehaviour
    {
        [Inject] private GenericSceneManager _sceneManager;
        
        private LevelController _controller;

        public void Activate(LevelController controller)
        {
            _controller = controller;
            gameObject.SetActive(true);
        }

        public void OnResumeButtonPressed()
        {
            _controller.ResumeGame();
            gameObject.SetActive(false);
        }

        public void OnRetryButtonPressed()
        {
            _sceneManager.LoadSceneAsync("SampleScene");
        }

        public void OnMenuButtonPressed()
        {
            _sceneManager.LoadSceneAsync("MenuScene");
        }
    }
}