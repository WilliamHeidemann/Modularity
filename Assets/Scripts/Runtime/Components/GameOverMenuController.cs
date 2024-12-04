using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Runtime.Scriptable_Objects;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private ScoreTracker _scoreTracker;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _heartsConnectedText;
    [SerializeField] private TextMeshProUGUI _furnacesConnectedText;
    [SerializeField] private TextMeshProUGUI _brainsActivatedText;
    [SerializeField] private TextMeshProUGUI _enginesActivatedText;
    [SerializeField] private TextMeshProUGUI _hybridsActivatedText;
    [SerializeField] private TextMeshProUGUI _energySpheresCollectedText;

    private Animator _animator;
    private void OnEnable()
    {
        _scoreText.text = _scoreTracker.GetScore().ToString();
        _highScoreText.text = _scoreTracker.GetHighScore().ToString();

        _heartsConnectedText.text = _scoreTracker.hearthConnections.ToString();
        _furnacesConnectedText.text = _scoreTracker.furnaceConnections.ToString();
        _brainsActivatedText.text = _scoreTracker.brainActivations.ToString();
        _enginesActivatedText.text = _scoreTracker.engineActivations.ToString();
        _hybridsActivatedText.text = _scoreTracker.hybridActivations.ToString();
        _energySpheresCollectedText.text = _scoreTracker.energySpheresCollected.ToString();

        _animator = GetComponent<Animator>();
        _animator.Play("Base Layer.Game Over Fade Animation");
        _gameUI.SetActive(false);

    }

    public void MainMenu()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
