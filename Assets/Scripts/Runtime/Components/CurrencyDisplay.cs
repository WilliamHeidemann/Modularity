using System;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private Currency _currency;

        private void Start()
        {
            _currencyText.text = _currency.Amount.ToString();
            _currency.OnGearsChanged += UpdateCurrencyText;
        }

        private void UpdateCurrencyText(int newAmount)
        {
            _currencyText.text = newAmount.ToString();
        }
    }
}
