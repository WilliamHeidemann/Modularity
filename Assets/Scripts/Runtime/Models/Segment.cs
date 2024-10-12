using System;
using UnityEngine;

namespace Runtime.Models
{
    [Serializable]
    public class Segment
    {
        [SerializeField] private Position _position;
        [SerializeField] private Rotation _rotation;
        [SerializeField] private Kind _kind;

        public Segment(Position position, Rotation rotation, Kind kind)
        {
            _position = position;
            _rotation = rotation;
            _kind = kind;
        }

        public Position Position => _position;
        public Rotation Rotation => _rotation;
        public Kind Kind => _kind;
        
        public override string ToString()
        {
            return $"Position: {Position}, Rotation: {Rotation}, Kind: {Kind}";
        }
    }
}