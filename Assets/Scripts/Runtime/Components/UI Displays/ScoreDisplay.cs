using UnityEngine;
using TMPro;
using Runtime.Scriptable_Objects;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreTracker _scoreTracker;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        Builder.OnSegmentPlaced += UpdateScore;
    }

    private void OnDisable()
    {
        Builder.OnSegmentPlaced -= UpdateScore;
    }

    private void UpdateScore()
    {
        _scoreText.text = _scoreTracker.GetScore().ToString();
    }
}
