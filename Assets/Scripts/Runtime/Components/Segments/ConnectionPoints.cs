using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Components.Segments
{
    [Serializable]
    public struct ConnectionPoints
    {
        public bool Up;
        public bool Down;
        public bool Right;
        public bool Left;
        public bool Forward;
        public bool Back;

        public IEnumerable<Vector3Int> AsVector3Ints()
        {
            if (Up) yield return Vector3Int.up;
            if (Down) yield return Vector3Int.down;
            if (Forward) yield return Vector3Int.forward;
            if (Back) yield return Vector3Int.back;
            if (Right) yield return Vector3Int.right;
            if (Left) yield return Vector3Int.left;
        }

        public int OpenConnectionPoints()
        {
            int count = 0;
            if (Up) count++;
            if (Down) count++;
            if (Forward) count++;
            if (Back) count++;
            if (Right) count++;
            if (Left) count++;
            return count;
        }
    }
}