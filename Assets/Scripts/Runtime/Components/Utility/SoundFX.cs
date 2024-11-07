using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Utility
{
    public class SoundFX : MonoSingleton<SoundFX>
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _blood1;
        [SerializeField] private AudioClip _blood2;
        [SerializeField] private AudioClip _steam1;
        [SerializeField] private AudioClip _steam2;
        public void PlayBlood1()
        {
            _audioSource.PlayOneShot(_blood1, 1f);
        }
        public void PlayBlood2()
        {
            _audioSource.PlayOneShot(_blood2, 1f);
        }
        public void PlaySteam1()
        {
            _audioSource.PlayOneShot(_steam1, 0.3f);
        }
        public void PlaySteam2()
        {
            _audioSource.PlayOneShot(_steam2, 0.3f);
        }

        public void PlaySoundEffect(StaticSegmentData staticSegmentData)
        {
            if (staticSegmentData.Blood)
            {
                if (Random.value < 0.5f)
                {
                    PlayBlood1();
                }
                else
                {
                    PlayBlood2();
                }
            } 
            else if (staticSegmentData.Steam)
            {
                if (Random.value < 0.5f)
                {
                    PlaySteam1();
                }
                else
                {
                    PlaySteam2();
                }
            }
        }
    }
}