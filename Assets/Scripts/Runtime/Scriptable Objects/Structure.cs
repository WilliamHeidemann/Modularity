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
        [SerializeField] private List<SegmentData> _graphData = new();

        public void AddSegment(SegmentData segmentData) => _graphData.Add(segmentData);

        public bool ConnectsToSomething(SegmentData segmentData)
        {
            return _graphData.Any(data => CanConnect(segmentData, data));
        }

        public bool CanConnect(SegmentData segmentData1, SegmentData segmentData2)
        {
            var from1To2 = segmentData1.GetConnectionPoints().Contains(segmentData2.Position);
            var from2To1 = segmentData2.GetConnectionPoints().Contains(segmentData1.Position);
            return from1To2 && from2To1;
        }
        
        public IEnumerable<SegmentData> GetLinks(SegmentData segmentData)
        {
            var connectionsOneWay = _graphData.Where(data => segmentData.GetConnectionPoints().Contains(data.Position));
            return connectionsOneWay.Where(data => data.GetConnectionPoints().Contains(segmentData.Position));
        }
        
        public bool IsEmpty => _graphData.Count == 0;
        public bool IsOpenPosition(Vector3Int position) => _graphData.All(data => data.Position != position);

        public void Clear()
        {
            _graphData.Clear();
        }

        public bool IsOpenSlotPosition(Vector3Int position)
        {
            var isOpenPosition = IsOpenPosition(position);
            var isSlotPosition = _graphData.Any(data => data.GetConnectionPoints().Contains(position));
            return isOpenPosition && !isSlotPosition;
        }
    }
}