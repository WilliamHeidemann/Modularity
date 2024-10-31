using System;
using System.Linq;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class SegmentData
    {
        public Vector3Int Position;
        public Quaternion Rotation;
        public StaticSegmentData StaticSegmentData;
    }
}