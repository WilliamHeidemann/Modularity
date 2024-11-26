using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Components.Utility
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private PauseMenuController _pauseMenu;

        private bool _isGameStarted = false;
        private bool _isGamePaused = false;

        private void OnEnable()
        {
            MainMenuController.OnGameStart += GameStarted;
            PauseMenuController.OnGamePause += TogglePause;
        }

        private void OnDisable()
        {
            MainMenuController.OnGameStart -= GameStarted;
            PauseMenuController.OnGamePause -= TogglePause;
        }

        private void GameStarted()
        {
            _isGameStarted = true;
        }

        private void TogglePause()
        {
            _isGamePaused = !_isGamePaused;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isGameStarted)
                {
                    Debug.LogWarning("Game has not started yet.");
                    return;
                }

                if (!_isGamePaused)
                {
                    Debug.Log("Game paused.");
                    _pauseMenu.gameObject.SetActive(true);
                    _pauseMenu.OnPause();
                }
                else
                {
                    Debug.Log("Game resumed.");
                    _pauseMenu.StartGame();
                }
            }
        }
    }
}