using System;
using System.Collections;
using Runtime.Scriptable_Objects;
using UnityEditor;
using UnityEngine;
using UtilityToolkit.Runtime;
using Random = UnityEngine.Random;

namespace Runtime.Components.Utility
{
    public class SoundFXPlayer : MonoSingleton<SoundFXPlayer>
    {
        [SerializeField] private AudioSource _SFXAudioSource;
        [SerializeField] private AudioSource _musicAudioSource1, _musicAudioSource2;
        private AudioSource activeMusicSource;
        public float transitionTime = 5f;

        [Header("SoundFX")]
        [SerializeField] private AudioClip _fleshConnectorPlacement;
        [SerializeField] private AudioClip _fleshReceiverPlacement;
        [SerializeField] private AudioClip _steamConnectorPlacement;
        [SerializeField] private AudioClip _steamReceiverPlacement;
        [SerializeField] private AudioClip _mixConnectorPlacement;
        [SerializeField] private AudioClip _mixReceiverPlacement;
        [SerializeField] private AudioClip _income;
        [SerializeField] private AudioClip _cardMouseOver;
        [SerializeField] private AudioClip _cardSelection;
        [SerializeField] private AudioClip _orbSpawn;
        [SerializeField] private AudioClip _orbCollected;
        [SerializeField] private AudioClip _invalidPlacement;

        private float _volumeModifier = 0.5f;

        private void OnEnable()
        {
            OptionMenuController.OnSoundChange += SetVolume;
            MainMenuController.OnGameStart += ChangeBackgroundMusic;
            activeMusicSource = _musicAudioSource1;
        }

        private void OnDisable()
        {
            OptionMenuController.OnSoundChange -= SetVolume;
            MainMenuController.OnGameStart -= ChangeBackgroundMusic;
        }

        public void SetVolume(float musicVolume, float SFXVolume)
        {
            _volumeModifier = SFXVolume;
            _SFXAudioSource.volume = SFXVolume;
            if(activeMusicSource == _musicAudioSource1)
                _musicAudioSource1.volume = musicVolume * 0.7f;
            else
                _musicAudioSource2.volume = musicVolume;
        }

        public void Play(SoundFX soundFX, float volume = 1f)
        {
            var audioClip = soundFX switch
            {
                SoundFX.FleshConnectorPlacement => _fleshConnectorPlacement,
                SoundFX.FleshReceiverPlacement => _fleshReceiverPlacement,
                SoundFX.SteamConnectorPlacement => _steamConnectorPlacement,
                SoundFX.SteamReceiverPlacement => _steamReceiverPlacement,
                SoundFX.MixConnectorPlacement => _mixConnectorPlacement,
                SoundFX.MixReceiverPlacement => _mixReceiverPlacement,
                SoundFX.Income => _income,
                SoundFX.CardSelection => _cardSelection,
                SoundFX.CardMouseOver => _cardMouseOver,
                SoundFX.OrbSpawn => _orbSpawn,
                SoundFX.OrbCollected => _orbCollected,
                SoundFX.InvalidPlacement => _invalidPlacement,
                _ => throw new ArgumentOutOfRangeException(nameof(soundFX), soundFX, null)
            };

            _SFXAudioSource.PlayOneShot(audioClip, volume * _volumeModifier);
        }

        private void ChangeBackgroundMusic()
        {
            activeMusicSource = _musicAudioSource2;
            _musicAudioSource2.volume = 0;
            _musicAudioSource2.Play();
            StartCoroutine(MixAudioSources());
        }

        IEnumerator MixAudioSources()
        {
            float t = 0;
            while (t < transitionTime)
            {
                t += Time.deltaTime / transitionTime;
                _musicAudioSource1.volume = Mathf.Lerp(_musicAudioSource1.volume, 0, t / transitionTime);
                _musicAudioSource2.volume = Mathf.Lerp(_musicAudioSource2.volume, PlayerPrefs.GetFloat("MusicVolume"), t / transitionTime);
                yield return null;
            }
            _musicAudioSource1.volume = 0;
            _musicAudioSource1.Stop();
        }
    }

    public enum SoundFX
    {
        FleshConnectorPlacement,
        FleshReceiverPlacement,
        SteamConnectorPlacement,
        SteamReceiverPlacement,
        MixConnectorPlacement,
        MixReceiverPlacement,
        Income,
        CardSelection,
        CardMouseOver,
        OrbSpawn,
        OrbCollected,
        InvalidPlacement
    }
}