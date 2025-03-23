using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Backend;
using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.DataLayer
{
    [Serializable]
    public struct ConnectionPoints
    {
        public ConnectionType Up;
        public ConnectionType Down;
        public ConnectionType Right;
        public ConnectionType Left;
        public ConnectionType Forward;
        public ConnectionType Back;

        public IEnumerable<Vector3Int> AsVector3Ints()
        {
            if (Up > 0) yield return Vector3Int.up;
            if (Down > 0) yield return Vector3Int.down;
            if (Forward > 0) yield return Vector3Int.forward;
            if (Back > 0) yield return Vector3Int.back;
            if (Right > 0) yield return Vector3Int.right;
            if (Left > 0) yield return Vector3Int.left;
        }

        public IEnumerable<(Vector3Int, ConnectionType)> GetDirectionData()
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

        public static readonly Vector3Int[] AllDirections =
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.forward,
            Vector3Int.back,
            Vector3Int.right,
            Vector3Int.left,
        };
        
        public IEnumerable<Quaternion> AsQuaternions()
        {
            if (Up > 0) yield return Quaternion.Euler(0, 0, 0);
            if (Down > 0) yield return Quaternion.Euler(180, 0, 0);
            if (Forward > 0) yield return Quaternion.Euler(90, 0, 0);
            if (Back > 0) yield return Quaternion.Euler(-90, 0, 0);
            if (Right > 0) yield return Quaternion.Euler(0, 0, 90);
            if (Left > 0) yield return Quaternion.Euler(0, 0, -90);
        }
        
        public IEnumerable<(Vector3Int position, Quaternion quaternion, ConnectionType type)> GetConnectionPointsData()
        {
            return LinqExtensions.ZipThree(AsVector3Ints(), AsQuaternions(), GetConnectionTypes());
        }
        
        private IEnumerable<ConnectionType> GetConnectionTypes()
        {
            if (Up > 0) yield return Up;
            if (Down > 0) yield return Down;
            if (Forward > 0) yield return Forward;
            if (Back > 0) yield return Back;
            if (Right > 0) yield return Right;
            if (Left > 0) yield return Left;
        }
    }
}