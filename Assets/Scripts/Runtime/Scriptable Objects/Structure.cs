using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        [SerializeField] private List<SegmentData> _graphData = new();
        [SerializeField] private Currency _currency;

        public IEnumerable<SegmentData> Sources => _graphData.Where(data => data.StaticSegmentData.IsSource);
        public IEnumerable<SegmentData> Receivers => _graphData.Where(data => data.StaticSegmentData.IsReceiver);
        public void AddSegment(SegmentData segmentData) => _graphData.Add(segmentData);
        
        public bool ConnectsToNeighbors(SegmentData segmentData)
        {
            var neighbors = segmentData.GetConnectionPoints()
                .Where(point => _graphData.Any(data => data.Position == point))
                .Select(point => _graphData.First(data => data.Position == point))
                .ToList();

            return neighbors.Any();
        }

        public bool ConnectsEverywhere(SegmentData segmentData)
        {
            var everythingConnects = segmentData.GetConnectionPoints()
                .All(point => _graphData.Any(data => data.Position == point));

            return everythingConnects && ConnectsToNeighbors(segmentData);
        }

        private bool CanConnect(SegmentData segmentData1, SegmentData segmentData2)
        {
            if (!segmentData1.StaticSegmentData.IsConnector && !segmentData2.StaticSegmentData.IsConnector)
            {
                return false;
            }
            
            foreach (var connectionPoint in segmentData1.GetConnectionPointsPlus())
            {
                if (connectionPoint.Item1 == segmentData2.Position
                    && segmentData2.GetConnectionPointsPlus().Contains((segmentData1.Position, connectionPoint.Item2)))
                    return true;
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

        public void Clear() => _graphData.Clear();


        public IEnumerable<SegmentData> GetInputSegments(SegmentData segmentData) =>
            _graphData.Where(data => data.GetConnectionPoints().Contains(segmentData.Position));
        
        public IEnumerable<SegmentData> GetOutputSegments(SegmentData segmentData) =>
            _graphData.Where(data => segmentData.GetConnectionPoints().Contains(data.Position));

        public IEnumerable<ConnectionType> GetInputs(SegmentData segmentData) => GetInputs(segmentData.Position);
        public IEnumerable<ConnectionType> GetInputs(Vector3Int position) => 
            _graphData.SelectMany(segment => segment.GetConnectionPointsPlus())
                .Where(connection => connection.Item1 == position)
                .Select(connection => connection.Item2);
        
        
        private IEnumerable<SegmentData> Neighbors(SegmentData segmentData)
        {
            return ConnectionPoints.AllDirections()
                .Select(direction => segmentData.Position + direction)
                .Where(position => !IsOpenPosition(position))
                .Select(position => _graphData.First(data => data.Position == position));
        }

        private IEnumerable<Vector3Int> NeighborPositions(SegmentData segmentData)
        {
            return ConnectionPoints.AllDirections().Select(direction => segmentData.Position + direction);
        }

    }
}