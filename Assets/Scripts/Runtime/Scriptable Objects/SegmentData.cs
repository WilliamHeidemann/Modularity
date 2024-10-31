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
            var rotation = Rotation.eulerAngles;
            var x = direction.x * Mathf.Cos(rotation.y) - direction.z * Mathf.Sin(rotation.y);
            var z = direction.x * Mathf.Sin(rotation.y) + direction.z * Mathf.Cos(rotation.y);
            return new Vector3Int((int) x, direction.y, (int) z);
        }
    }
}