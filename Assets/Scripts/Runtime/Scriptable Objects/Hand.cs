using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Runtime.Components.Systems;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        public delegate void DrawHand();
        public event DrawHand OnDrawHand;

        [SerializeField] private Selection _selection;
        [SerializeField] private SegmentPool _pool;

        //the segments that the player can choose from
        public List<Segment> SegmentsOptions;

        private int _optionsCount = 3;

        public void Initialize()
        {
            GenerateHand();
        }

        public void SelectBlueprint(int chosenSegment)
        {
            _selection.Prefab = Option<Segment>.Some(SegmentsOptions[chosenSegment]);
            _selection.Price = SegmentsOptions[chosenSegment].StaticSegmentData.ConnectionPoints.OpenConnectionPoints();
        }

        public void GenerateHand()
        {
            SegmentsOptions = new List<Segment>();

            for (int i = 0; i < _optionsCount; i++)
            {
                var segment = _pool.GetRandomSegment();
                int failsafe = 0;

                while (SegmentsOptions.Contains(segment))
                {
                    segment = _pool.GetRandomSegment();
                    failsafe++;

                    if (failsafe > 10)
                    {
                        Debug.LogError("Failsafe triggered, breaking loop");
                        break;
                    }
                };

                SegmentsOptions.Add(segment);
            }
            OnDrawHand?.Invoke();
        }
    }
}