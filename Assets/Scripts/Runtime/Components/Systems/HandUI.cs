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
                _hand.GenerateHand();
            }
        }

        private void DisplayHand()
        {
            var segments = _hand.SegmentsOptions;

            for (int i = 0; i < _blueprintOptions.Length; i++)
            {
                _blueprintOptions[i].SetCost(segments[i].StaticSegmentData.ConnectionPoints.OpenConnectionPoints().ToString());
                _blueprintOptions[i].SetPreview(segments[i].Preview);
            }
        }
    }
}
