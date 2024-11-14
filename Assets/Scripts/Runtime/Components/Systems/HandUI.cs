using System;
using System.Threading.Tasks;
using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEditor;
using UnityEngine;

namespace Runtime.Components.Systems
{
    public class HandUI : MonoBehaviour
    {
        [SerializeField] private Blueprint[] _blueprintOptions;
        [SerializeField] private Hand _hand;
        [SerializeField] private Currency _currency;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            _hand.OnDrawHand += DisplayHand;
        }

        private void OnDestroy()
        {
            _hand.OnDrawHand -= DisplayHand;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ReRoll();
            }
        }

        public void ReRoll()
        {
            if (_currency.HasAtLeast(1, 1))
            {
                _currency.Pay(1, 1);
                _hand.GenerateHand();
            }
        }

        private void DisplayHand()
        {
            var segments = _hand.SegmentsOptions;

            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                var cost = segments[i].StaticSegmentData.BloodCost + segments[i].StaticSegmentData.SteamCost;
                _blueprintOptions[i].SetCost(cost.ToString(), segments[i].StaticSegmentData.BloodCost, segments[i].StaticSegmentData.SteamCost);
                _blueprintOptions[i].SetPreview(segments[i].Preview);
                _blueprintOptions[i].GlowState(false);
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
    }
}
