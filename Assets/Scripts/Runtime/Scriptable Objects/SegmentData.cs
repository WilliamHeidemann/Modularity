using System;
using System.Collections.Generic;
using System.Linq;
using GluonGui.Dialog;
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

        public HashSet<(Vector3Int, int)> GetConnectionPointsPlus()
        {
            HashSet<(Vector3Int,int)> result = new ();
            foreach (var pair in StaticSegmentData.ConnectionPoints.GetDirectionData())
            {
                result.Add((TransformDirection(pair.Item1) + Position, pair.Item2));
            }
            return result;
        }

        private Vector3Int TransformDirection(Vector3Int direction)
        {
            return Vector3Int.RoundToInt(Rotation * direction);
        }
    }
}