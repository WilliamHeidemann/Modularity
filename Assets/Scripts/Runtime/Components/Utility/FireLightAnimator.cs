using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FireLightAnimator : MonoBehaviour
{
    public bool isActive = false;

    [SerializeField] private Light[] _fireLights;
    [SerializeField] private float _centralLightIntensity = 1;
    private LocalSoundSystem _localSoundSystem;
    private bool _isPlaying = false;

    private void Start()
    {
        _localSoundSystem = transform.GetComponent<LocalSoundSystem>();

        if (_localSoundSystem == null)
        {
            Debug.LogError("No LocalSoundSystem found on the object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            PlayFireAnimation();
            _localSoundSystem.PlaySound();
        }
        else
        {
            foreach (var light in _fireLights)
            {
                light.DOIntensity(0, 0.5f).SetEase(Ease.OutQuad);
            }
            _localSoundSystem.StopSound();
        }
    }

    private void PlayFireAnimation()
    {
        if (_isPlaying) return;
        _isPlaying = true;
        var sequence = DOTween.Sequence();

        foreach (var light in _fireLights)
        {
            float randomAnimationTime = Random.Range(0.3f, 1f);
            float randomIntensity = Random.Range(-0.75f, 0.75f);
            var changeIntensity = light.DOIntensity(_centralLightIntensity + randomIntensity, randomAnimationTime).SetEase(Ease.InQuad);

            sequence.Join(changeIntensity);
        }

        sequence.OnComplete(() => _isPlaying = false);
    }
}
