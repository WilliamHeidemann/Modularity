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
        public void AddSegment(SegmentData segmentData) => _graphData.Add(segmentData);
        public void Clear() => _graphData.Clear();

        public IEnumerable<SegmentData> Segments => _graphData;
        public IEnumerable<SegmentData> Sources => _graphData.Where(data => data.StaticSegmentData.IsSource);
        public IEnumerable<SegmentData> Receivers => _graphData.Where(data => data.StaticSegmentData.IsReceiver);
        public IEnumerable<SegmentData> Connectors => _graphData.Where(data => data.StaticSegmentData.IsConnector);

        private bool ConnectsToAtLeastOneNeighbor(SegmentData segmentData) =>
            Neighbors(segmentData).Any(neighbor => CanConnect(segmentData, neighbor));

        public bool ConnectsEverywhere(SegmentData segmentData) =>
            Neighbors(segmentData).All(neighbor => CanConnect(segmentData, neighbor));

        private bool CanConnect(SegmentData segmentData1, SegmentData segmentData2)
        {
            if (!segmentData1.StaticSegmentData.IsConnector && !segmentData2.StaticSegmentData.IsConnector)
            {
                return false;
            }

            var connection1Option = segmentData1.GetConnectionPointsPlus().FirstOption(point =>
                point.Item1 == segmentData2.Position);
            var connection2Option = segmentData2.GetConnectionPointsPlus().FirstOption(point =>
                point.Item1 == segmentData1.Position);

            if (!connection1Option.IsSome(out var connection1) ||
                !connection2Option.IsSome(out var connection2))
            {
                return false;
            }

            return connection1.Item2 == connection2.Item2;
        }

        public IEnumerable<SegmentData> GetValidConnections(SegmentData segmentData) =>
            GetOutputSegments(segmentData).Where(data => CanConnect(data, segmentData));

        public bool IsOpenPosition(Vector3Int position) => _graphData.All(data => data.Position != position);

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

        public bool IsValidPlacement(SegmentData segmentData)
        {
            var connectsToWrongType = GetOutputSegments(segmentData).Any(neighbor => !CanConnect(segmentData, neighbor));
            
            if (connectsToWrongType)
            {
                return false;
            }
            
            var atLeastOneConnect = ConnectsToAtLeastOneNeighbor(segmentData);

            if (!atLeastOneConnect)
            {
                return false;
            }
            
            if (segmentData.StaticSegmentData.IsConnector)
            {
                return true;
            }
            
            return GetOutputSegments(segmentData).All(link => link.StaticSegmentData.IsConnector);
        }

        public bool IsValidSourcePlacement(Vector3Int position)
        {
            return !GetInputs(position).Any() && IsOpenPosition(position);
        }
    }
}