using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class RewardActivator : SegmentActivator
    {
        [SerializeField] private StaticSegmentData _staticSegmentData;
        [SerializeField] private Currency _currency;
        [SerializeField] private GameObject _rewardDisplayPrefab;
        private const float AnimationTime = 2f;

        public override void Activate()
        {
            if (IsActive)
            {
                return;
            }

            GrantReward();
            PlayRewardAnimation();
            IsActive = true;
        }

        private void PlayRewardAnimation()
        {
            var canvas = Instantiate(_rewardDisplayPrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            var text = canvas.GetComponentInChildren<TextMeshProUGUI>();
            text.text = (_staticSegmentData.BloodReward + _staticSegmentData.SteamReward).ToString();

            var fade = text.DOFade(0, AnimationTime);
            var startingScale = canvas.transform.localScale;
            var scaleUp = canvas.transform.DOScale(startingScale * 1.5f, AnimationTime * 0.25f).SetEase(Ease.InQuad);
            var scaleDown = canvas.transform.DOScale(startingScale, AnimationTime * 0.75f).SetEase(Ease.OutQuad);
            
            var sequence = DOTween.Sequence();
            sequence.Append(scaleUp);
            sequence.Append(scaleDown);
            sequence.OnComplete(() => Destroy(canvas.gameObject));
        }

        private void GrantReward()
        {
            _currency.Add(_staticSegmentData.BloodReward,_staticSegmentData.SteamReward);
        }
    }
}