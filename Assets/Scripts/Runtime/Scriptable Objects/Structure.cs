using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        public readonly HashSet<Vector3Int> SlotPositions = new();
        private readonly HashSet<SegmentData> _graphData = new();

        public void AddSegment(Segment segment)
        {
            var segmentData = new SegmentData
            {
                Position = segment.transform.position.AsVector3Int(),
                Rotation = segment.transform.rotation,
                StaticSegmentData = segment.StaticSegmentData,
            };

            _graphData.Add(segmentData);
        }
        
        public bool ConnectsToSomething(Vector3Int position)
        {
            if (_graphData.Count == 0)
            {
                return true;
            }
            
            return _graphData.Any(data => data.ConnectsTo(position));
        }

        public bool IsOpenPosition(Vector3Int position)
        {
            return _graphData.All(data => data.Position != position);
        }
        
        public void Clear()
        {
            SlotPositions.Clear();
            _graphData.Clear();
        }
    }

    public class SegmentData
    {
        public Vector3Int Position;
        public Quaternion Rotation;
        public StaticSegmentData StaticSegmentData;
        
        public bool ConnectsTo(Vector3Int position)
        {
            return StaticSegmentData.ConnectionPoints.AsVector3Ints().Contains(position - Position);
        }
    }
}