using log4net.Util;
using System;
using Runtime.Components;
using Runtime.Components.Utility;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class CurrencyPopup : ScriptableObject
    {
        [SerializeField] private Currency _currency;
        [SerializeField] private PopupDisplay _displayPopupPrefab;

        public void Activate(Vector3 popupPosition, StaticSegmentData staticSegmentData)
        {
            var popup = Instantiate(_displayPopupPrefab, popupPosition + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            popup._staticSegmentData = staticSegmentData;
            _currency.Add(staticSegmentData.BloodReward, staticSegmentData.SteamReward);
            SoundFXPlayer.Instance.Play(SoundFX.Income);
        }
    }
}