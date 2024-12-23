using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class SegmentData
    {
        public Vector3Int Position;
        public Quaternion Rotation;
        public StaticSegmentData StaticSegmentData;
        public bool IsActivated;

        public IEnumerable<Vector3Int> GetConnectionPoints() =>
            StaticSegmentData.ConnectionPoints
                .AsVector3Ints()
                .Select(TransformDirection)
                .Select(direction => Position + direction);

        public IEnumerable<(Vector3Int, ConnectionType)> GetConnectionPointsPlus()
        {
            return StaticSegmentData.ConnectionPoints.GetDirectionData()
                .Select(pair => (TransformDirection(pair.Item1) + Position, pair.Item2));
        }

        private Vector3Int TransformDirection(Vector3Int direction)
        {
            return Vector3Int.RoundToInt(Rotation * direction);
        }
    }
}