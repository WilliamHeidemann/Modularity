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
            var connectsDirectionally = CanConnectDirectionally(
                segmentData1, segmentData2,
                out var connection1,
                out var connection2);

            if (!connectsDirectionally)
            {
                return false;
            }

            return connection1.Item2 == connection2.Item2;
        }

        private bool CanConnectDirectionally(SegmentData segmentData1, SegmentData segmentData2,
            out (Vector3Int, ConnectionType) connection1, out (Vector3Int, ConnectionType) connection2)
        {
            var connection1Option = segmentData1.GetConnectionPointsPlus().FirstOption(point =>
                point.Item1 == segmentData2.Position);
            var connection2Option = segmentData2.GetConnectionPointsPlus().FirstOption(point =>
                point.Item1 == segmentData1.Position);

            var connectsOut = connection1Option.IsSome(out var c1);
            var connectsIn = connection2Option.IsSome(out var c2);
            connection1 = c1;
            connection2 = c2;
            return connectsOut && connectsIn;
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

        public IEnumerable<SegmentData> GetNeighborsFacingThis(SegmentData segmentData)
        {
            return Neighbors(segmentData)
                .Where(neighbor => CanConnectDirectionally(segmentData, neighbor, out _, out _));
        }

        public bool IsValidPlacement(SegmentData segmentData)
        {
            var connectsToWrongType =
                GetNeighborsFacingThis(segmentData).Any(neighbor => !CanConnect(segmentData, neighbor));

            if (connectsToWrongType)
            {
                return false;
            }

            var atLeastOneConnect = ConnectsToAtLeastOneNeighbor(segmentData);

            return atLeastOneConnect;
        }

        public bool IsDirectionallyValidPlacement(SegmentData segmentData)
        {
            var connectsToAtLeastOneNeighborDirectionally =
                Neighbors(segmentData).Any(neighbor =>
                    CanConnectDirectionally(segmentData, neighbor, out var _, out var _));

            return connectsToAtLeastOneNeighborDirectionally;
        }

        public bool IsValidSourcePlacement(Vector3Int position)
        {
            return !GetInputs(position).Any() && IsOpenPosition(position);
        }
    }
}