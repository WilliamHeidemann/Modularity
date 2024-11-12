using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        [SerializeField] private List<SegmentData> _graphData = new();
        [SerializeField] private Currency _currency;

        public List<SegmentData> GraphData
        {
            get { return _graphData; }
        }

        public void AddSegment(SegmentData segmentData)
        {
            _graphData.Add(segmentData);
        }

        public bool ConnectsToNeighbors(SegmentData segmentData)
        {
            var neighbors = segmentData.GetConnectionPoints()
                .Where(point => _graphData.Any(data => data.Position == point))
                .Select(point => _graphData.First(data => data.Position == point))
                .ToList();
            
            return neighbors.Any() && neighbors.All(segment => CanConnect(segmentData, segment));
        }

        public bool ConnectsEverywhere(SegmentData segmentData)
        {
            var everythingConnects = segmentData.GetConnectionPoints()
                .All(point => _graphData.Any(data => data.Position == point));

            return everythingConnects && ConnectsToNeighbors(segmentData);
        }

        private bool CanConnect(SegmentData segmentData1, SegmentData segmentData2)
        {
            foreach (var connectionPoint in segmentData1.GetConnectionPointsPlus())
            {
                if (connectionPoint.Item1 == segmentData2.Position 
                    && segmentData2.GetConnectionPointsPlus().Contains((segmentData1.Position, connectionPoint.Item2))) return true;
            }
            return false;
        }


        public IEnumerable<SegmentData> GetLinks(SegmentData segmentData)
        {
            var connectionsOneWay = _graphData.Where(data => segmentData.GetConnectionPoints().Contains(data.Position));
            return connectionsOneWay.Where(data => CanConnect(data, segmentData));
        }

        public bool IsEmpty => _graphData.Count == 0;
        public bool IsOpenPosition(Vector3Int position) => _graphData.All(data => data.Position != position);

        public void Clear()
        {
            _graphData.Clear();
        }

    }
}