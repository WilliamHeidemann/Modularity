using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Systems
{
    public class HandUI : MonoBehaviour
    {
        [SerializeField] private Blueprint[] _blueprintOptions;
        [SerializeField] private Hand _hand;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            _hand.OnDrawHand += DisplayHand;
        }

        private void OnDestroy()
        {
            _hand.OnDrawHand -= DisplayHand;
        }

        private void DisplayHand()
        {
            var segments = _hand.SegmentsOptions;

            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                _blueprintOptions[i].SetName(segments[i].name);
                _blueprintOptions[i].SetCost(segments[i].StaticSegmentData.ConnectionPoints.OpenConnectionPoints().ToString());
            }
        }
    }
}
