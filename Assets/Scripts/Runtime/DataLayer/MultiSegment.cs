using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.DataLayer
{
    [CreateAssetMenu(menuName = "MultiSegment")]
    public class MultiSegment : StaticSegmentData
    {
        // serialized dictionary
        [SerializeField] private List<Vector3Int> _positions = new();
        [SerializeField] private List<StaticSegmentData> _segments = new();

        public override IEnumerable<Vector3Int> GetConnectionPointPositions()
        {
            var zip = _positions.Zip(_segments, 
                (position, segment) => (position, segment));
            return zip.SelectMany(pair => pair.segment.GetConnectionPointPositions()
                .Select(direction => pair.position + direction));
        }

        public override IEnumerable<(Vector3Int, ConnectionType)> GetConnectionPointData()
        {
            var zip = _positions.Zip(_segments, 
                (position, segment) => (position, segment));
            return zip.SelectMany(pair => pair.segment.GetConnectionPointData()
                .Select(data => (data.Item1 + pair.position, data.Item2)));
        }
    }
}