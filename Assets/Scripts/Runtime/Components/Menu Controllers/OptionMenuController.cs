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
        [SerializeField] private Slider _dragSlider;
        [SerializeField] private Slider _lookSlider;
        [SerializeField] private Slider _zoomSlider;
        [SerializeField] private Image _musicCrossout;
        [SerializeField] private Image _SFXCrossout;
        [SerializeField] AudioSource _testingSFXAudioSource;
        
        public delegate void SoundChange(float musicVolume, float SFXVolume);
        public static event SoundChange OnSoundChange;

        private void Start()
        {
            GameStartupSettings();
        }

        public void GameStartupSettings()
        {
            //makes sure there always is a value for the playerpref of music and sfx volume
            if (!PlayerPrefs.HasKey("MusicVolume") && !PlayerPrefs.HasKey("SFXVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
                PlayerPrefs.SetFloat("SFXVolume", _SFXSlider.value);
                PlayerPrefs.SetFloat("DragIntensity", _dragSlider.value);
                PlayerPrefs.SetFloat("LookIntensity", _lookSlider.value);
                PlayerPrefs.SetFloat("ZoomIntensity", _zoomSlider.value);
            }
            else
            {
                _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
                _SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
                _dragSlider.value = PlayerPrefs.GetFloat("DragIntensity");
                _lookSlider.value = PlayerPrefs.GetFloat("LookIntensity");
                _zoomSlider.value = PlayerPrefs.GetFloat("ZoomIntensity");
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
            if (this.gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
            }

            OnSoundChange?.Invoke(_musicSlider.value, _SFXSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", _SFXSlider.value);
            _testingSFXAudioSource.volume = _SFXSlider.value;

            if (_SFXSlider.value == 0)
            {
                _SFXCrossout.gameObject.SetActive(true);
            }
            else
            {
                if (this.gameObject.activeInHierarchy)
                {
                    StartCoroutine(PlaySound());
                }
                _SFXCrossout.gameObject.SetActive(false);
            }
        }

        public void OnDragIntensityChaged()
        {
            PlayerPrefs.SetFloat("DragIntensity", _dragSlider.value);

            if (this.gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(PlaySound());
            }
        }

        public void OnLookIntensityChanged()
        {
            PlayerPrefs.SetFloat("LookIntensity", _lookSlider.value);

            if (this.gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(PlaySound());
            }
        }

        public void OnZoomIntensityChanged()
        {
            PlayerPrefs.SetFloat("ZoomIntensity", _zoomSlider.value);

            if(this.gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(PlaySound());
            }
        }

        IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(0.1f);
            _testingSFXAudioSource.Play();
        }
    }
}
