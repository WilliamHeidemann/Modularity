using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Components.Utility
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;

        private bool _isGameStarted = false;
        private bool _isGamePaused = false;

        private void OnEnable()
        {
            MainMenuController.OnGameStart += TogglePause;
            PauseMenuController.OnGamePause += TogglePause;
        }

        private void OnDisable()
        {
            MainMenuController.OnGameStart -= TogglePause;
            PauseMenuController.OnGamePause -= TogglePause;
        }

        public void TogglePause()
        {
            _isGameStarted = true;
            _isGamePaused = !_isGamePaused;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isGameStarted)
                {
                    return;
                }

                if (!_isGamePaused)
                {
                    _pauseMenu.SetActive(true);
                    _pauseMenu.GetComponent<PauseMenuController>().OnPause();
                }
                else
                {
                    _pauseMenu.GetComponent<PauseMenuController>().StartGame();
                }
            }
        }
    }
}