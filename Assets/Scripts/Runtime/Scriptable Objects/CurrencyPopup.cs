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
        [SerializeField] private GameObject _WorldSpaceCanvas;
        private GameObject _ActiveWorldSpaceCanvas;

        public void CheckForActiveCanvas()
        {
            if (_ActiveWorldSpaceCanvas == null)
            {
                var canvas = Instantiate(_WorldSpaceCanvas);
                if (Camera.main == null)
                {
                    throw new Exception("Main camera not found");
                }
                canvas.GetComponent<Canvas>().worldCamera = Camera.main.gameObject.GetComponentInChildren<Camera>();
                _ActiveWorldSpaceCanvas = canvas;
            }
        }

        public void SpendCurrency(Vector3 popupPosition, StaticSegmentData staticSegmentData)
        {
            CheckForActiveCanvas();

            var popup = Instantiate(_displayPopupPrefab, popupPosition, Quaternion.identity);
            popup._staticSegmentData = staticSegmentData;
            popup.isCurrencyGained = false;
            popup.transform.SetParent(_ActiveWorldSpaceCanvas.transform);
        }

        public void GainCurrency(Vector3 popupPosition, StaticSegmentData staticSegmentData)
        {
            CheckForActiveCanvas();

            var popup = Instantiate(_displayPopupPrefab, popupPosition + new Vector3(0, 0.5f, 0), Quaternion.identity);
            popup._staticSegmentData = staticSegmentData;
            popup.isCurrencyGained = true;
            popup.transform.SetParent(_ActiveWorldSpaceCanvas.transform);
            _currency.Add(staticSegmentData.BloodReward, staticSegmentData.SteamReward);
            SoundFXPlayer.Instance.Play(SoundFX.Income);
        }
    }
}