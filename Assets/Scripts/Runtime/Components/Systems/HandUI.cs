using Runtime.Scriptable_Objects;
using UnityEngine;
using System.Collections;
using Runtime.Components.Segments;

namespace Runtime.Components.Systems
{
    public class HandUI : MonoBehaviour
    {
        [SerializeField] private Blueprint[] _blueprintOptions;
        [SerializeField] private Hand _hand;

        void OnEnable()
        {
            Hand.OnDrawHand += DisplayHand;
            DisplayHand();
        }

        void OnDisable()
        {
            Hand.OnDrawHand -= DisplayHand;
        }

        void DisplayHand()
        {
            var segments = _hand._segmentsOptions;

            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                _blueprintOptions[i].SetName(segments[i].name);
                _blueprintOptions[i].SetCost(segments[i].ConnectionPoints.OpenConnectionPoints().ToString());
            }
        }
    }
}
