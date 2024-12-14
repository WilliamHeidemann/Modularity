using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Components
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _buttonsPanel;

        public delegate void GamePause();
        public static event GamePause OnGamePause;

        public void OnPause()
        {
            OnGamePause?.Invoke();
            _gameUI.SetActive(false);
        }

        public void StartGame()
        {
            _gameUI.SetActive(true);
            OnGamePause?.Invoke();
            _optionsPanel.SetActive(false);
            _buttonsPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void Options()
        {
            _optionsPanel.SetActive(true);
            _optionsPanel.GetComponent<OptionMenuController>()._menuToReturnTo = _buttonsPanel;
            _buttonsPanel.SetActive(false);
        }

        public void MainMenu()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}
