using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components.Systems
{
    public class SupplyUI : MonoBehaviour
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
            _bloodText.text = $"Blood: {blood.ToString()}";
            _energyText.text = $"Energy: {energy.ToString()}";
            _mechanicalText.text = $"Mechanical: {mechanical.ToString()}";
        }
    }
}