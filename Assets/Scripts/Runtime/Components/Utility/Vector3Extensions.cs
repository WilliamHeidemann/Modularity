using System;
using UnityEngine;

namespace Runtime.Components.Utility
{
    public static class Vector3Extensions
    {
        public static Vector3Int AsVector3Int(this Vector3 vector3) =>
            new(Mathf.RoundToInt(vector3.x), 
                Mathf.RoundToInt(vector3.y),
                Mathf.RoundToInt(vector3.z));
    }
}