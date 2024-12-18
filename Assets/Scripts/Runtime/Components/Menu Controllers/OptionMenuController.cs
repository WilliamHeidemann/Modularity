using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class OptionMenuController : MonoBehaviour
    {
        [HideInInspector] public GameObject _menuToReturnTo;
        [SerializeField] private GameObject _creditsOverlay;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _SFXSlider;
        [SerializeField] private Image _musicCrossout;
        [SerializeField] private Image _SFXCrossout;
        private AudioSource _testingSFXAudioSource;
        
        public delegate void SoundChange(float musicVolume, float SFXVolume);
        public static event SoundChange OnSoundChange;

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            }
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                _SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            }
        }

        public void GameStartupSettings()
        {
            _testingSFXAudioSource = GetComponent<AudioSource>();

            if (!PlayerPrefs.HasKey("MusicVolume") && !PlayerPrefs.HasKey("SFXVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
                PlayerPrefs.SetFloat("SFXVolume", _SFXSlider.value);
            }

            OnSoundChange?.Invoke(PlayerPrefs.GetFloat("MusicVolume"), PlayerPrefs.GetFloat("SFXVolume"));
            _testingSFXAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
        }

        public void Credits()
        {
            _creditsOverlay.SetActive(true);
        }

        public void Return()
        {
            _menuToReturnTo.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void OnMusicVolumeChanged()
        {
            OnSoundChange?.Invoke(_musicSlider.value, _SFXSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);

            if (_musicSlider.value == 0)
            {
                _musicCrossout.gameObject.SetActive(true);
            }
            else
            {
                _musicCrossout.gameObject.SetActive(false);
            }
        }

        public void OnSFXVolumeChanged()
        {
            StopAllCoroutines();
            OnSoundChange?.Invoke(_musicSlider.value, _SFXSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", _SFXSlider.value);
            _testingSFXAudioSource.volume = _SFXSlider.value;

            if (_SFXSlider.value == 0)
            {
                _SFXCrossout.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(PlaySound());
                _SFXCrossout.gameObject.SetActive(false);
            }
        }

        IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(0.1f);
            _testingSFXAudioSource.Play();
        }
    }
}
