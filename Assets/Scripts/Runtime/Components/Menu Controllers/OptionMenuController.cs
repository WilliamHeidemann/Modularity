using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class OptionMenuController : MonoBehaviour
    {
        [HideInInspector] public GameObject _menuToReturnTo;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _SFXSlider;
        [SerializeField] private Image _musicCrossout;
        [SerializeField] private Image _SFXCrossout;
        [SerializeField] private AudioSource _testingAudioSource;

        public delegate void SoundChange(float musicVolume, float SFXVolume);
        public static event SoundChange OnSoundChange;

        private void Start()
        {
            _testingAudioSource = GetComponent<AudioSource>();
            _testingAudioSource.volume = _SFXSlider.value;
        }

        public void Return()
        {
            _menuToReturnTo.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void OnMusicVolumeChanged()
        {
            OnSoundChange?.Invoke(_musicSlider.value, _SFXSlider.value);

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
            _testingAudioSource.volume = _SFXSlider.value;

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
            _testingAudioSource.Play();
        }
    }
}
