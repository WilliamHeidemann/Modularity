using System;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Runtime.Components
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _BloodCurrencyText;
        [SerializeField] private TextMeshProUGUI _SteamCurrencyText;
        [SerializeField] private Currency _currency;

        [SerializeField] private float _animationTime = 1f;
        [SerializeField] private float _moveUpAmount = 30f;
        [SerializeField] private Transform _bloodTextSpawnPos;
        [SerializeField] private Transform _steamTextSpawnPos;
        [SerializeField] private GameObject _bloodUpdateText;
        [SerializeField] private GameObject _steamUpdateText;

        private void Start()
        {
            _BloodCurrencyText.text = _currency.BloodAmount.ToString();
            _SteamCurrencyText.text = _currency.SteamAmount.ToString();
            _currency.OnCurrencyChanged += UpdateCurrencyText;
        }

        private void UpdateCurrencyText(int bloodAmount, int steamAmount)
        {
            int bloodDifference = bloodAmount - int.Parse(_BloodCurrencyText.text);
            int steamDifference = steamAmount - int.Parse(_SteamCurrencyText.text);

            _BloodCurrencyText.text = bloodAmount.ToString();
            _SteamCurrencyText.text = steamAmount.ToString();

            if (bloodDifference != 0 && steamDifference != 0)
            {
                UpdateTextAnimation(_bloodTextSpawnPos, _bloodUpdateText, bloodDifference);
                UpdateTextAnimation(_steamTextSpawnPos, _steamUpdateText, steamDifference);
            }
            else if (bloodDifference != 0)
            {
                UpdateTextAnimation(_bloodTextSpawnPos, _bloodUpdateText, bloodDifference);
            }
            else if (steamDifference != 0)
            {
                UpdateTextAnimation(_steamTextSpawnPos, _steamUpdateText, steamDifference);
            }
        }

        private void UpdateTextAnimation(Transform _spawnPosition, GameObject _updateText, int updateAmount)
        {
            GameObject updateTextClone = Instantiate(_updateText, _spawnPosition);
            updateTextClone.transform.parent = _spawnPosition;
            TMP_Text updateText = updateTextClone.GetComponent<TMP_Text>();
            float moveAmount = 0;

            if (updateAmount < 0)
            {
                updateText.text = updateAmount.ToString();
                moveAmount = -_moveUpAmount;
            }
            else
            {
                updateText.text = updateAmount.ToString();
                moveAmount = _moveUpAmount;
            }

            var textFade = updateText.DOFade(0, _animationTime);
            var moveUp = updateTextClone.transform.DOMoveY(updateTextClone.transform.position.y + moveAmount, _animationTime).SetEase(Ease.OutQuad);

            var sequence = DOTween.Sequence();
            sequence.Join(textFade);
            sequence.Join(moveUp);
            sequence.OnComplete(() => Destroy(updateTextClone));
        }
    }
}
