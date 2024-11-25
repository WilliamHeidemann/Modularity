using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _optionsPanel;

    public void StartGame()
    {
        _gameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Options()
    {
        _optionsPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
