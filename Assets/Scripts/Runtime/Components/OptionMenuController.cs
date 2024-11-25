using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _SFXSlider;

    public void Return()
    {
        _mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnMusicVolumeChanged()
    {
        Debug.Log($"Music volume changed to {_musicSlider.value}");
    }

    public void OnSFXVolumeChanged()
    {
        Debug.Log($"SFX volume changed to {_SFXSlider.value}");
    }
}
