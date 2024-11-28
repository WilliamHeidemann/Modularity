using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class OptionMenuController : MonoBehaviour
    {
        [HideInInspector] public GameObject _menuToReturnTo;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _SFXSlider;

        public delegate void SoundChange(float musicVolume, float SFXVolume);
        public static event SoundChange OnSoundChange;

        public void Return()
        {
            _menuToReturnTo.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void OnMusicVolumeChanged()
        {
            OnSoundChange?.Invoke(_musicSlider.value, _SFXSlider.value);
        }

        public void OnSFXVolumeChanged()
        {
            OnSoundChange?.Invoke(_musicSlider.value, _SFXSlider.value);
        }
    }
}
