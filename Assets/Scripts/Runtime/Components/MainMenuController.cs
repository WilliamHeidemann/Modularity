using UnityEngine;

namespace Runtime.Components
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _buttonsPanel;

        public delegate void GameStart();
        public static event GameStart OnGameStart;

        public void StartGame()
        {
            _gameUI.SetActive(true);
            OnGameStart?.Invoke();
            this.gameObject.SetActive(false);
        }

        public void Options()
        {
            _optionsPanel.SetActive(true);
            _optionsPanel.GetComponent<OptionMenuController>()._menuToReturnTo = _buttonsPanel;
            _buttonsPanel.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
