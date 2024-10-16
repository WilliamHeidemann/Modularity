using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Models
{
    [Serializable]
    public struct Position
    {
        [field: SerializeField] public int X { get; private set; }
        [field: SerializeField] public int Y { get; private set; }
        [field: SerializeField] public int Z { get; private set; }
        
        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public Vector3Int AsVector3 => new(X, Y, Z);

        public static readonly Position Forward = new(0, 0, 1);
        public static readonly Position Back = new(0, 0, -1);
        public static readonly Position Right = new(1, 0, 0);
        public static readonly Position Left = new(-1, 0, 0);
        public static readonly Position Up = new(0, 1, 0);
        public static readonly Position Down = new(0, -1, 0);

        public static Position operator +(Position a, Position b) =>
            new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static bool operator ==(Position left, Position right) => left.Equals(right);
        public static bool operator !=(Position left, Position right) => !left.Equals(right);
        public bool Equals(Position other) => X == other.X && Y == other.Y && Z == other.Z;
        public override bool Equals(object obj) => obj is Position other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}