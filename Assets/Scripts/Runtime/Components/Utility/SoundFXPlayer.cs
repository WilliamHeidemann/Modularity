using System;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;
using Random = UnityEngine.Random;

namespace Runtime.Components.Utility
{
    public class SoundFXPlayer : MonoSingleton<SoundFXPlayer>
    {
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private AudioClip _fleshConnectorPlacement;
        [SerializeField] private AudioClip _fleshReceiverPlacement;
        [SerializeField] private AudioClip _bloodIncome;
        [SerializeField] private AudioClip _steamConnectorPlacement;
        [SerializeField] private AudioClip _steamReceiverPlacement;
        [SerializeField] private AudioClip _steamIncome;
        [SerializeField] private AudioClip _mixConnectorPlacement;
        [SerializeField] private AudioClip _mixReceiverPlacement;
        [SerializeField] private AudioClip _mixIncome;
        [SerializeField] private AudioClip _cardMouseOver;
        [SerializeField] private AudioClip _cardSelection;
            
        public void Play(SoundFX soundFX, float volume = 1f)
        {
            var audioClip = soundFX switch
            {
                SoundFX.FleshConnectorPlacement => _fleshConnectorPlacement,
                SoundFX.FleshReceiverPlacement => _fleshReceiverPlacement,
                SoundFX.BloodIncome => _bloodIncome,
                SoundFX.SteamConnectorPlacement => _steamConnectorPlacement,
                SoundFX.SteamReceiverPlacement => _steamReceiverPlacement,
                SoundFX.SteamIncome => _steamIncome,
                SoundFX.MixConnectorPlacement => _mixConnectorPlacement,
                SoundFX.MixReceiverPlacement => _mixReceiverPlacement,
                SoundFX.MixIncome => _mixIncome,
                SoundFX.CardSelection => _cardSelection,
                SoundFX.CardMouseOver => _cardMouseOver,
                _ => throw new ArgumentOutOfRangeException(nameof(soundFX), soundFX, null)
            };
            
            _audioSource.PlayOneShot(audioClip, volume);
        }
    }


    public enum SoundFX
    {
        FleshConnectorPlacement,
        FleshReceiverPlacement,
        BloodIncome,
        SteamConnectorPlacement,
        SteamReceiverPlacement,
        SteamIncome,
        MixConnectorPlacement,
        MixReceiverPlacement,
        MixIncome,
        CardSelection,
        CardMouseOver
    }
}