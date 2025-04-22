using Runtime.Components;
using UnityEngine;

public class LocalSoundSystem : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float _volumeMaximum = 1;
    [HideInInspector] public AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on the object.");
            return;
        }

        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * _volumeMaximum;
    }

    public void SetVolume(float musicVolume, float SFXVolume)
    {
        audioSource.volume = SFXVolume * _volumeMaximum;
    }

    public void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
