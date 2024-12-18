using UnityEngine;
using TMPro;
using Runtime.Scriptable_Objects;
using DG.Tweening;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreTracker _scoreTracker;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _scoreUpdateText;

    [SerializeField] private float _animationTime = 1.5f;
    private float _moveUpAmount = Screen.height / 20;

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
        int scoreDifference = _scoreTracker.GetScore() - int.Parse(_scoreText.text);
        _scoreText.text = _scoreTracker.GetScore().ToString();

        if(scoreDifference == 0)
        {
            return;
        }

        GameObject scoreUpdateTextClone = Instantiate(_scoreUpdateText, transform);
        TMP_Text scoreText = scoreUpdateTextClone.GetComponent<TMP_Text>();
        scoreText.text = "+" + scoreDifference.ToString();
        var textFade = scoreText.DOFade(0, _animationTime).SetEase(Ease.InQuad);
        var moveUp = scoreUpdateTextClone.transform.DOMoveY(scoreUpdateTextClone.transform.position.y + _moveUpAmount, _animationTime).SetEase(Ease.OutQuad);

        var sequence = DOTween.Sequence();
        sequence.Join(textFade);
        sequence.Join(moveUp);
        sequence.OnComplete(() => Destroy(scoreUpdateTextClone));
    }
}
