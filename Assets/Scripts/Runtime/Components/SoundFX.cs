using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components
{
    public class SoundFX : MonoSingleton<SoundFX>
    {
        [SerializeField] private AudioSource audioSource;
    }
}
