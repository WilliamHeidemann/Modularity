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
                    return;
                }

                if (!_isGamePaused)
                {
                    _pauseMenu.gameObject.SetActive(true);
                    _pauseMenu.OnPause();
                }
                else
                {
                    _pauseMenu.StartGame();
                }
            }
        }
    }
}