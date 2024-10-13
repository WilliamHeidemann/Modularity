using System;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components.Gameplay
{
    public class SupplyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bloodText;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private TextMeshProUGUI _mechanicalText;

        private void Start()
        {
            Structure.OnSupplyChanged += UpdateTexts;
        }

        private void OnDisable()
        {
            Structure.OnSupplyChanged -= UpdateTexts;
        }

        private void UpdateTexts(int blood, int energy, int mechanical)
        {
            _bloodText.text = blood.ToString();
            _energyText.text = energy.ToString();
            _mechanicalText.text = mechanical.ToString();
        }
    }
}