using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class SegmentData
    {
        public Vector3Int Position;
        public Quaternion Rotation;
        public StaticSegmentData StaticSegmentData;

        public IEnumerable<Vector3Int> GetConnectionPoints() =>
            StaticSegmentData.ConnectionPoints
                .AsVector3Ints()
                .Select(TransformDirection)
                .Select(direction => Position + direction);

        private Vector3Int TransformDirection(Vector3Int direction)
        {
            return Vector3Int.RoundToInt(Rotation * direction);
        }
    }
}