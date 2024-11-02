using Runtime.Scriptable_Objects;
using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private Currency _currency;
    [SerializeField] private TextMeshProUGUI _gearText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gearText.text = _currency.GetGearAmount().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
