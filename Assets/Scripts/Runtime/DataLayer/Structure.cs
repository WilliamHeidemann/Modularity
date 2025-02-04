using System;
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
        private readonly Dictionary<Vector3Int, SegmentData> _graphData = new();

        public void AddSegment(SegmentData segmentData) => _graphData.Add(segmentData.Position, segmentData);
        public void Clear() => _graphData.Clear();
        public int Count => _graphData.Count;

        public IEnumerable<SegmentData> Segments => _graphData.Values;
        public IEnumerable<SegmentData> Sources => _graphData.Values.Where(data => data.StaticSegmentData.IsSource);
        public IEnumerable<SegmentData> Receivers => _graphData.Values.Where(data => data.StaticSegmentData.IsReceiver);

        public IEnumerable<SegmentData> Connectors =>
            _graphData.Values.Where(data => data.StaticSegmentData.IsConnector);

        public bool ConnectsToAtLeastOneNeighbor(SegmentData segmentData) =>
            Neighbors(segmentData).Any(segmentData.CanConnect);

        public bool ConnectsToAllNeighbors(SegmentData segmentData) =>
            Neighbors(segmentData).All(segmentData.CanConnect);

        public IEnumerable<SegmentData> GetValidConnections(SegmentData segmentData) =>
            GetPointedToSegments(segmentData).Where(segmentData.CanConnect);

        public bool IsOpenPosition(Vector3Int position) => !_graphData.ContainsKey(position);

        public IEnumerable<SegmentData> GetPointedFromSegments(SegmentData segmentData)
        {
            return Neighbors(segmentData)
                .Where(neighbor => neighbor.GetConnectionPoints().Contains(segmentData.Position));
        }

        public IEnumerable<SegmentData> GetPointedToSegments(SegmentData segmentData)
        {
            var pointedToPositions = segmentData.GetConnectionPoints();
            var neighborPositions = Neighbors(segmentData).Select(neighbor => neighbor.Position);
            var intersection = pointedToPositions.Intersect(neighborPositions);
            foreach (Vector3Int position in intersection)
            {
                yield return _graphData[position];
            }
        }

        public IEnumerable<ConnectionType> GetPointedFromConnectionTypes(SegmentData segmentData)
        {
            return GetPointedFromConnectionTypes(segmentData.Position);
        }

        public IEnumerable<ConnectionType> GetPointedFromConnectionTypes(Vector3Int position)
        {
            return Neighbors(position)
                .SelectMany(neighbor => neighbor.GetConnectionPointsPlus())
                .Where(tuple => tuple.position == position)
                .Select(tuple => tuple.type);
        }

        public IEnumerable<SegmentData> Neighbors(Vector3Int position)
        {
            foreach (Vector3Int direction in ConnectionPoints.AllDirections)
            {
                Vector3Int neighborPosition = position + direction;
                if (_graphData.TryGetValue(neighborPosition, out SegmentData neighbor))
                {
                    yield return neighbor;
                }
            }
        }

        public IEnumerable<SegmentData> Neighbors(SegmentData segmentData) => Neighbors(segmentData.Position);

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

        public int OpenConnectionCount(bool excludeSources = false)
        {
            if (excludeSources)
            {
                return _graphData.Values
                    .Where(segmentData => !segmentData.StaticSegmentData.IsSource)
                    .SelectMany(segmentData => segmentData.GetConnectionPoints())
                    .ToHashSet()
                    .Count(position => IsOpenPosition(position));
            }

            return _graphData.Values
                .SelectMany(segmentData => segmentData.GetConnectionPoints())
                .ToHashSet()
                .Count(position => IsOpenPosition(position));
        }
    }
}