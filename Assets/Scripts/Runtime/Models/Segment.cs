using System;
using UnityEngine;

namespace Runtime.Models
{
    [Serializable]
    public class Segment
    {
        [SerializeField] private Position _position;
        [SerializeField] private Kind _kind;
        [SerializeField] private SegmentStats _stats;

        public Segment(Position position, Kind kind, SegmentStats stats)
        {
            _position = position;
            _kind = kind;
            _stats = stats;
        }

        public Position Position => _position;
        public Kind Kind => _kind;
        public SegmentStats Stats => _stats;

        public void SetPosition(Position position) => _position = position;
        
        public override string ToString()
        {
            return $"Position: {Position}, Kind: {Kind}";
        }
    }
}