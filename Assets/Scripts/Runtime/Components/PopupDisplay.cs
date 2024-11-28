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

        [SerializeField] private TMP_Text bloodText;
        [SerializeField] private TMP_Text steamText;
        [SerializeField] private Canvas canvas;

        [SerializeField] private float AnimationTime = 2f;

        private void Start()
        {
            var textToDisplay = new List<TMP_Text>();
            if (_staticSegmentData.BloodReward > 0)
            {
                bloodText.gameObject.SetActive(true);
                textToDisplay.Add(bloodText);
                bloodText.text = "+" + _staticSegmentData.BloodReward.ToString();
            }

            if(_staticSegmentData.SteamReward > 0)
            {
                steamText.gameObject.SetActive(true);
                textToDisplay.Add(steamText);
                steamText.text = "+" + _staticSegmentData.SteamReward.ToString();
            }

            PlayRewardAnimation(textToDisplay.ToArray());
        }

        private void PlayRewardAnimation(TMP_Text[] textToDisplay)
        {
            foreach (var text in textToDisplay)
            {
                var fade = text.DOFade(0, AnimationTime);
                text.GetComponentInChildren<Image>().DOFade(0, AnimationTime);
            }
        
            var startingScale = canvas.transform.localScale;
            var scaleUp = canvas.transform.DOScale(startingScale * 1.5f, AnimationTime * 0.25f).SetEase(Ease.InQuad);
            var scaleDown = canvas.transform.DOScale(startingScale, AnimationTime * 0.75f).SetEase(Ease.OutQuad);

            var sequence = DOTween.Sequence();
            sequence.Append(scaleUp);
            sequence.Append(scaleDown);
            sequence.OnComplete(() => Destroy(this.gameObject));
        }
    }
}
