using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Editor;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        public List<Vector3Int> SlotPositions = new();
        [SerializeField] private List<SegmentData> _graphData = new();

        public void AddSegment(SegmentData segmentData) => _graphData.Add(segmentData);
        public bool ConnectsToSomething(SegmentData segmentData) => _graphData.Any(data => true);
        public bool IsEmpty => _graphData.Count == 0;
        public bool IsOpenPosition(Vector3Int position) => _graphData.All(data => data.Position != position);

        public void Clear()
        {
            SlotPositions.Clear();
            _graphData.Clear();
        }
    }
}