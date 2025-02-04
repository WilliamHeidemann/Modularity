using Runtime.Components;
using UnityEngine;

public class LocalSoundSystem : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float _volumeMaximum = 1;
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
        _audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * _volumeMaximum;
    }

    public void SetVolume(float musicVolume, float SFXVolume)
    {
        _audioSource.volume = SFXVolume * _volumeMaximum;
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
