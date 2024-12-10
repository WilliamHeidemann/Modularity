using System;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _BloodCurrencyText;
        [SerializeField] private TextMeshProUGUI _SteamCurrencyText;
        [SerializeField] private Currency _currency;

        private void Start()
        {
            _BloodCurrencyText.text = _currency.BloodAmount.ToString();
            _SteamCurrencyText.text = _currency.SteamAmount.ToString();
            _currency.OnCurrencyChanged += UpdateCurrencyText;
        }

        private void UpdateCurrencyText(int bloodAmount, int steamAmount)
        {
            _BloodCurrencyText.text = bloodAmount.ToString();
            _SteamCurrencyText.text = steamAmount.ToString();
        }
    }
}
