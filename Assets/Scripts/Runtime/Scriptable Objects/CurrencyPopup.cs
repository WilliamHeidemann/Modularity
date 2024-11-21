using log4net.Util;
using System;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class CurrencyPopup : ScriptableObject
    {
        private StaticSegmentData _staticSegmentData;
        [SerializeField] private Currency _currency;
        [SerializeField] private GameObject _displayPopupPrefab;

        public void Activate(Vector3 popupPosition, StaticSegmentData segmentData)
        {
            var popup = Instantiate(_displayPopupPrefab, popupPosition + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            popup.GetComponent<PopupDisplay>()._staticSegmentData = segmentData;
            _staticSegmentData = segmentData;
            GrantReward();
        }

        private void GrantReward()
        {
            _currency.Add(_staticSegmentData.BloodReward, _staticSegmentData.SteamReward);
        }
    }
}