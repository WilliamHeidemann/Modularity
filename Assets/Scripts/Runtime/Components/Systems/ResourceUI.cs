using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private Resources _resources;
    [SerializeField] private TextMeshProUGUI _gearText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_gearText.text = _resources.GetGearAmount().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
