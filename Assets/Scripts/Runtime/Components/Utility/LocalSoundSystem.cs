using Runtime.Components;
using UnityEngine;

public class LocalSoundSystem : MonoBehaviour
{
    [SerializeField] float _volumeMinimum;
    [SerializeField] float _volumeMaximum;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        OptionMenuController.OnSoundChange += SetVolume;
    }

    private void OnDisable()
    {
        OptionMenuController.OnSoundChange -= SetVolume;
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = Mathf.Clamp(PlayerPrefs.GetFloat("SFXVolume"), _volumeMinimum, _volumeMaximum);
    }

    public void SetVolume(float musicVolume, float SFXVolume)
    {
        _audioSource.volume = Mathf.Clamp(SFXVolume, _volumeMinimum, _volumeMaximum);
    }

    public void PlaySound()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }

        _audioSource.Play();
    }
}
