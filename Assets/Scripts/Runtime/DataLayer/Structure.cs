using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.DataLayer
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

        public bool ConnectsToAtLeastOneNeighbor(SegmentData segmentData) =>
            Neighbors(segmentData).Any(segmentData.CanConnect);

        public bool ConnectsToAllNeighbors(SegmentData segmentData) =>
            Neighbors(segmentData).All(segmentData.CanConnect);

        public IEnumerable<SegmentData> GetValidConnections(SegmentData segmentData) =>
            GetPointedToSegments(segmentData).Where(segmentData.CanConnect);

        public bool IsOpenPosition(Vector3Int position) => _graphData.All(data => data.Position != position);

        public IEnumerable<SegmentData> GetPointedFromSegments(SegmentData segmentData) =>
            _graphData.Where(data => data.GetConnectionPoints().Contains(segmentData.Position));

        public IEnumerable<SegmentData> GetPointedToSegments(SegmentData segmentData) =>
            _graphData.Where(data => segmentData.GetConnectionPoints().Contains(data.Position));

        public IEnumerable<ConnectionType> GetPointedFromConnectionTypes(SegmentData segmentData) => GetPointedFromConnectionTypes(segmentData.Position);

        public IEnumerable<ConnectionType> GetPointedFromConnectionTypes(Vector3Int position) =>
            _graphData.SelectMany(segment => segment.GetConnectionPointsPlus())
                .Where(connection => connection.position == position)
                .Select(connection => connection.type);

        public IEnumerable<SegmentData> Neighbors(SegmentData segmentData)
        {
            return ConnectionPoints.AllDirections()
                .Select(direction => segmentData.Position + direction)
                .Where(position => !IsOpenPosition(position))
                .Select(position => _graphData.First(data => data.Position == position));
        }

        public IEnumerable<SegmentData> GetNeighborsConnectingDirectionally(SegmentData segmentData)
        {
            return Neighbors(segmentData)
                .Where(neighbor => segmentData.CanConnectDirectionally(neighbor, out _, out _));
        }

        public bool IsValidPlacement(SegmentData segmentData)
        {
            var connectsToWrongType =
                GetNeighborsConnectingDirectionally(segmentData).Any(neighbor => !segmentData.CanConnect(neighbor));

            if (connectsToWrongType)
            {
                return false;
            }
            
            if (!IsOpenPosition(segmentData.Position))
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
                    segmentData.CanConnectDirectionally(neighbor, out _, out _));

            return connectsToAtLeastOneNeighborDirectionally;
        }

        public bool IsValidSourcePlacement(Vector3Int position)
        {
            return !GetPointedFromConnectionTypes(position).Any() && IsOpenPosition(position);
        }
    }
}