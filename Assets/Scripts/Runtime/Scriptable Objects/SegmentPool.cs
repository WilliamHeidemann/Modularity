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
        [SerializeField] [Range(0f, 10f)] private float _ultraRareDropChance;
        [SerializeField] [Range(0f, 10f)] private float _rareDropChance;
        [SerializeField] [Range(0f, 10f)] private float _commonDropChance;
        
        [SerializeField] private List<Segment> _ultraRareSegments;
        [SerializeField] private List<Segment> _rareSegments;
        [SerializeField] private List<Segment> _commonSegments;

        [Header("Tutorial Segment Lists")]
        [SerializeField] private List<Segment> _bloodConnectors;
        [SerializeField] private List<Segment> _bloodReceivers;
        [SerializeField] private List<Segment> _steamConnectors;
        [SerializeField] private List<Segment> _steamReceivers;
        [SerializeField] private List<Segment> _hybridSegments;

        public Segment GetRandomSegment()
        {
            var maxValue = _ultraRareDropChance + _rareDropChance + _commonDropChance;
            var value = Random.Range(0f, maxValue);
            
            if (value <= _ultraRareDropChance && _ultraRareSegments.Count > 0)
            {
                return _ultraRareSegments.RandomElement();
            } 
            if (value <= _ultraRareDropChance + _rareDropChance && _rareSegments.Count > 0)
            {
                return _rareSegments.RandomElement();
            }

            return _commonSegments.RandomElement();
        }
    }
}