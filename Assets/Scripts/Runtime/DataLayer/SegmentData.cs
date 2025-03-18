using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.DataLayer
{
    [Serializable]
    public class SegmentData
    {
        public Vector3Int Position;
        public Quaternion Rotation;
        public StaticSegmentData StaticSegmentData;
        public bool IsActivated;

        public IEnumerable<Vector3Int> GetConnectionPoints() =>
            StaticSegmentData
                .GetConnectionPointPositions()
                .Select(TransformDirection)
                .Select(direction => Position + direction);

        public IEnumerable<(Vector3Int position, ConnectionType type)> GetConnectionPointsPlus()
        {
            return StaticSegmentData.GetConnectionPointData()
                .Select(pair => (TransformDirection(pair.Item1) + Position, pair.Item2));
        }

        private Vector3Int TransformDirection(Vector3Int direction)
        {
            return Vector3Int.RoundToInt(Rotation * direction);
        }
    }
}