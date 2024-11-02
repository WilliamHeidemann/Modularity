using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Systems
{
    public class HandUI : MonoBehaviour
    {
        [SerializeField] private Blueprint[] _blueprintOptions;
        [SerializeField] private Hand _hand;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void OnEnable()
        {
            _hand.OnDrawHand += DisplayHand;
            DisplayHand();
        }

        void OnDisable()
        {
            _hand.OnDrawHand -= DisplayHand;
        }

        void DisplayHand()
        {
            var segments = _hand._segmentsOptions;

            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                _blueprintOptions[i].SetName(segments[i].name);
                _blueprintOptions[i].SetCost(segments[i].StaticSegmentData.ConnectionPoints.OpenConnectionPoints().ToString());
            }
        }
    }
}
