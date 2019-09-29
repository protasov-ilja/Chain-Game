using ProjectName.Utils;
using UnityEngine;
using Zenject;

namespace ProjectName.UI.WinScreenUI
{
    public class LooseScreen : MonoBehaviour
    {
        [Inject] private GenericSceneManager _sceneManager;
        
        public void Activate()
        {
            gameObject.SetActive(true);
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