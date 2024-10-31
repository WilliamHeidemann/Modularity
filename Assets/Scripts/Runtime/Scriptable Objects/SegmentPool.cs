using Runtime.Components.Segments;
using System;
using UnityEngine;
using System.Collections.Generic;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class SegmentPool : ScriptableObject
    {
        [SerializeField] private List<Segment> _segmentPool;

        public Segment[] GetSegmentPool()
        {
            return _segmentPool.ToArray();
        }

        public Segment GetRandomSegment()
        {
            return _segmentPool.RandomElement();
        }
    }
}