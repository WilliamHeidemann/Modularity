using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Utility
{
    public class SoundFX : MonoSingleton<SoundFX>
    {
        [SerializeField] private AudioSource _audioSource;
    }
}
