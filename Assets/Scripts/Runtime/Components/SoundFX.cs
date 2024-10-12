using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Runtime.Components
{
    public class SoundFX : MonoSingleton<SoundFX>
    {
        [SerializeField] private AudioSource _audioSource;
    }
}
