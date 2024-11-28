using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FireLightAnimator : MonoBehaviour
{
    [SerializeField] private Light[] _fireLights;
    [SerializeField] private float _centralLightIntensity = 1;
    private bool _isAnimating = false;

    // Update is called once per frame
    void Update()
    {
        if(!_isAnimating)
        {
            _isAnimating = true;
            PlayFireAnimation();
        }
    }

    private void PlayFireAnimation()
    {
        var sequence = DOTween.Sequence();

        foreach (var light in _fireLights)
        {
            float randomAnimationTime = Random.Range(0.3f, 1f);
            float randomIntensity = Random.Range(-0.75f, 0.75f);
            var changeIntensity = light.DOIntensity(_centralLightIntensity + randomIntensity, randomAnimationTime).SetEase(Ease.InQuad);

            sequence.Join(changeIntensity);
        }

        sequence.OnComplete(() => _isAnimating = false);
    }
}
