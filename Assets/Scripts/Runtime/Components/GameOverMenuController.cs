using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Runtime.Scriptable_Objects;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private ScoreTracker _scoreTracker;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scoreText.text = _scoreTracker.GetTotalScore().ToString();
        _highScoreText.text = _scoreTracker.GetTotalScore().ToString();
    }

    public void MainMenu()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
