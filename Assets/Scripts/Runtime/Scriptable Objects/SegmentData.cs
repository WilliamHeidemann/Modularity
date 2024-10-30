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
        
        public bool ConnectsTo(Vector3Int position)
        {
            // check if Position + one of the connection points is equal to the position
            return true;
        }
    }
}