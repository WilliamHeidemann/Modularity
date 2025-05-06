using Runtime.Components.Segments;
using System;
using UnityEngine;
using System.Collections.Generic;
using UtilityToolkit.Runtime;
using Random = UnityEngine.Random;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class SegmentPool : ScriptableObject
    {
        [SerializeField] [Range(0f, 10f)] private float _rareDropChance;
        [SerializeField] [Range(0f, 10f)] private float _uncommonDropChance;
        [SerializeField] [Range(0f, 10f)] private float _commonDropChance;
        [SerializeField] [Range(0f, 10f)] private float _basicSourceDropChance;
        
        [SerializeField] private List<Segment> _rareSegments;
        [SerializeField] private List<Segment> _uncommonSegments;
        [SerializeField] private List<Segment> _commonSegments;
        [SerializeField] private List<Segment> _basicSourceSegments;

        [Header("Tutorial Segment Lists")]
        [SerializeField] private List<Segment> _bloodConnectors;
        [SerializeField] private List<Segment> _bloodReceivers;
        [SerializeField] private List<Segment> _steamConnectors;
        [SerializeField] private List<Segment> _steamReceivers;
        [SerializeField] private List<Segment> _hybridSegments;

        public Segment GetRandomSegment()
        {
            var maxValue = _rareDropChance + _commonDropChance + _uncommonDropChance;
            var value = Random.Range(0f, maxValue);
            
            if (value <= _rareDropChance && _rareSegments.Count > 0)
            {
                return _rareSegments.RandomElement();
            } 
            if (value <= _uncommonDropChance + _rareDropChance && _uncommonSegments.Count > 0)
            {
                return _uncommonSegments.RandomElement();
            }
            if (value <= _basicSourceDropChance + _uncommonDropChance + _rareDropChance && _basicSourceSegments.Count > 0)
            {
                return _basicSourceSegments.RandomElement();
            }

            return _commonSegments.RandomElement();
        }
    }
}