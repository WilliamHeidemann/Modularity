using Runtime.Scriptable_Objects;
using System.Collections;
using UnityEngine;
using UnityUtils;
using Selection = Runtime.Scriptable_Objects.Selection;

namespace Runtime.Components.Systems
{
    public class HandUI : MonoBehaviour
    {
        [SerializeField] private Blueprint[] _blueprintOptions;
        [SerializeField] private Hand _hand;
        [SerializeField] private Currency _currency;
        [SerializeField] private Selection _selection;
        [SerializeField] private int _rerollCostBlood;
        [SerializeField] private int _rerollCostSteam;
        [SerializeField] private EndGame _endGame;

        private void Awake()
        {
            _hand.OnDrawHand += DisplayHand;
            _hand.OnDiscardHand += DiscardHand;
        }

        private void OnDestroy()
        {
            _hand.OnDrawHand -= DisplayHand;
            _hand.OnDiscardHand -= DiscardHand;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _hand.SelectBlueprint(0);
                ChangeGlow(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _hand.SelectBlueprint(1);
                ChangeGlow(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _hand.SelectBlueprint(2);
                ChangeGlow(2);
            }
        }

        public void ReRoll()
        {
            if (_currency.HasAtLeast(_rerollCostBlood, _rerollCostSteam))
            {
                _currency.Pay(_rerollCostBlood, _rerollCostSteam);
                _hand.DrawHand();
                _selection.Reset();
            }
        }

        private void DisplayHand()
        {
            StartCoroutine(DisplayHandCycle());
        }

        private IEnumerator DisplayHandCycle()
        {
            yield return new WaitForSeconds(0.6f);
            var segments = _hand.SegmentsOptions;

            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                _blueprintOptions[i].SetCardValues(segments[i].StaticSegmentData.BloodCost, segments[i].StaticSegmentData.SteamCost,
                    segments[i].StaticSegmentData.BloodReward, segments[i].StaticSegmentData.SteamReward);
                _blueprintOptions[i].SetPreview(segments[i].Preview);
                _blueprintOptions[i].GlowState(false);
                _blueprintOptions[i].PlayCardIntroAnimation();

                yield return new WaitForSeconds(0.2f);
            }
            // _hand.SelectBlueprint(1);
            // ChangeGlow(1);
            _endGame.CheckHand(segments, _rerollCostBlood, _rerollCostSteam);
        }

        private void DiscardHand(int selectedIndex)
        {
            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                if( i == selectedIndex)
                {
                    _blueprintOptions[i].PlayPickCardAnimation();
                }
                else
                {
                    _blueprintOptions[i].PlayCardOutroAnimation();
                }
            }
        }

        public void ChangeGlow(int chosenBlueprint)
        {
            foreach (var blueprint in _blueprintOptions)
            {
                if (blueprint == _blueprintOptions[chosenBlueprint])
                {
                    _blueprintOptions[chosenBlueprint].GlowState(true);
                }
                else
                {
                    blueprint.GlowState(false);
                }
            }
        }

        public void SetCardsVisible(int count)
        {
            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                _blueprintOptions[i].gameObject.SetActive(i < count);
            }
        }

        public void SetCostVisible(bool isVisible)
        {
            foreach (var blueprint in _blueprintOptions)
            {
                blueprint.SetCostVisible(isVisible);
            }
        }
    }
}