using System;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Currency : ScriptableObject
    {
        [SerializeField] private int _blood;
        [SerializeField] private int _steam;
        public event Action<int, int> OnCurrencyChanged;

        public void Add(int bloodAmount, int steamAmount)
        {
            _blood += bloodAmount;
            _steam += steamAmount;
            OnCurrencyChanged?.Invoke(_blood, _steam);
        }

        public void Pay(int bloodAmount, int steamAmount)
        {
            _blood -= bloodAmount;
            _steam -= steamAmount;
            OnCurrencyChanged?.Invoke(_blood, _steam);
        }

        public bool HasAtLeast(int bloodAmount, int steamAmount)
        {
            return _blood >= bloodAmount && _steam >= steamAmount;
        }

        public int BloodAmount => _blood;
        public int SteamAmount => _steam;

        public void Initialize(int startingCurrency)
        {
            _blood = startingCurrency;
            _steam = startingCurrency;
            OnCurrencyChanged?.Invoke(_blood, _steam);
        }
    }
}