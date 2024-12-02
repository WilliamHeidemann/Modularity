using System.Collections.Generic;
using DG.Tweening;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Components
{
    public class PopupDisplay : MonoBehaviour
    {
        public StaticSegmentData _staticSegmentData;
        public bool isCurrencyGained;

        [SerializeField] private TMP_Text bloodText;
        [SerializeField] private TMP_Text steamText;

        [SerializeField] private float AnimationTime = 2f;

        private void Start()
        {
            var textToDisplay = new List<TMP_Text>();
            if (_staticSegmentData.BloodReward > 0 && isCurrencyGained)
            {
                bloodText.gameObject.SetActive(true);
                textToDisplay.Add(bloodText);
                bloodText.text = "+" + _staticSegmentData.BloodReward.ToString();
            }

            if(_staticSegmentData.SteamReward > 0 && isCurrencyGained)
            {
                steamText.gameObject.SetActive(true);
                textToDisplay.Add(steamText);
                steamText.text = "+" + _staticSegmentData.SteamReward.ToString();
            }

            if (_staticSegmentData.BloodCost > 0 && !isCurrencyGained)
            {
                bloodText.gameObject.SetActive(true);
                textToDisplay.Add(bloodText);
                bloodText.text = "-" + _staticSegmentData.BloodCost.ToString();
            }

            if (_staticSegmentData.SteamCost > 0 && !isCurrencyGained)
            {
                steamText.gameObject.SetActive(true);
                textToDisplay.Add(steamText);
                steamText.text = "-" + _staticSegmentData.SteamCost.ToString();
            }

            PlayAnimation(textToDisplay.ToArray());
        }

        private void PlayAnimation(TMP_Text[] textToDisplay)
        {
            foreach (var text in textToDisplay)
            {
                var fade = text.DOFade(0, AnimationTime * 3f);
                text.GetComponentInChildren<Image>().DOFade(0, AnimationTime * 3f);
            }

            var startingScale = transform.localScale;
            var scaleUp = transform.DOScale(startingScale * 1.5f, AnimationTime * 0.25f).SetEase(Ease.InQuad);
            var scaleDown = transform.DOScale(startingScale * 0.5f, AnimationTime * 3f).SetEase(Ease.OutQuad);

            var sequence = DOTween.Sequence();
            sequence.Append(scaleUp);
            sequence.Append(scaleDown);
            sequence.OnComplete(() => Destroy(this.gameObject));
        }
    }
}
