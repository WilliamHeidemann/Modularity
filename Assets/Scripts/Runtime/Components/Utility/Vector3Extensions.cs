using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Components.Utility
{
    public static class Vector3Extensions
    {
        public static Vector3Int AsVector3Int(this Vector3 vector3) =>
            new(Mathf.RoundToInt(vector3.x), 
                Mathf.RoundToInt(vector3.y),
                Mathf.RoundToInt(vector3.z));
        
        public static bool ContainsRotation(this IEnumerable<Quaternion> quaternions, Quaternion rotation)
        {
            return quaternions.Any(quaternion => Quaternion.Angle(quaternion, rotation) < 1);
        }

        public static int IndexOfRotation(this IEnumerable<Quaternion> quaternions, Quaternion rotation)
        {
            return quaternions.Select((quaternion, index) => (quaternion, index))
                .First(pair => Quaternion.Angle(pair.quaternion, rotation) < 1).index;
        }
    }
}