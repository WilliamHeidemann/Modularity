using UnityEngine;

public class HeartAnimationController : MonoBehaviour
{
    public bool isActive = false;
    public bool addDelay = false;

    private Animator _heartAnimator;
    private LocalSoundSystem _localSoundSystem;
    private bool _isPlaying = false;
    private float _randomStartDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _heartAnimator = GetComponent<Animator>();
        if (_heartAnimator == null)
        {
            Debug.LogError("No Animator found on the object.");
        }
        _localSoundSystem = GetComponent<LocalSoundSystem>();
        if (_localSoundSystem == null)
        {
            Debug.LogError("No LocalSoundSystem found on the object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (!_isPlaying)
            {
                _randomStartDelay = 0;

                if (addDelay)
                {
                    _randomStartDelay = Random.Range(0.0f, 1f);
                }

                _isPlaying = true;
                Invoke(nameof(PlayHeartAnimation), _randomStartDelay);
            }
        }
        else
        {
            StopHeartAnimation();
            _localSoundSystem.StopSound();
        }
    }

    private void PlayHeartAnimation()
    {
        _localSoundSystem.PlaySound();
        _heartAnimator.SetBool("isActive", true);
    }

    private void StopHeartAnimation()
    {
        if (!_isPlaying) return;
        _isPlaying = false;
        _heartAnimator.SetBool("isActive", false);
    }
}
