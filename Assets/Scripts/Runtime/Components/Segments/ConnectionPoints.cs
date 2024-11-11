using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Components.Segments
{
    [Serializable]
    public struct ConnectionPoints
    {
        /*
        * ConnectionPoints are ints:
        * 0 = no connection
        * 1 = Blood
        * 2 = Steam
        */
        public int Up;
        public int Down;
        public int Right;
        public int Left;
        public int Forward;
        public int Back;

        public IEnumerable<Vector3Int> AsVector3Ints()
        {
            if (Up > 0) yield return Vector3Int.up;
            if (Down > 0) yield return Vector3Int.down;
            if (Forward > 0) yield return Vector3Int.forward;
            if (Back > 0) yield return Vector3Int.back;
            if (Right > 0) yield return Vector3Int.right;
            if (Left > 0) yield return Vector3Int.left;
        }

        public IEnumerable<(Vector3Int, int)> GetDirectionData()
        {
            if (Up > 0) yield return (Vector3Int.up, Up);
            if (Down > 0) yield return (Vector3Int.down, Down);
            if (Forward > 0) yield return (Vector3Int.forward, Forward);
            if (Back > 0) yield return (Vector3Int.back, Back);
            if (Right > 0) yield return (Vector3Int.right, Right);
            if (Left > 0) yield return (Vector3Int.left, Left);
        }

        public int OpenConnectionPoints()
        {
            int count = 0;
            if (Up > 0) count++;
            if (Down > 0) count++;
            if (Forward > 0) count++;
            if (Back > 0) count++;
            if (Right > 0) count++;
            if (Left > 0) count++;
            return count;
        }
    }
}