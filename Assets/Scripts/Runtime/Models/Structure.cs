using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Models
{
    public class Structure
    {
        [SerializeField] private List<Segment> _segments = new();
        public bool IsAvailable(Position position) => 
            _segments.All(segment => segment.Position != position);
        public void AddSegment(Segment segment)
        {
            _segments.Add(segment);
            Debug.Log($"Blood: {Blood}");
            Debug.Log($"Energy: {Energy}");
            Debug.Log($"Mechanical: {Mechanical}");
        }

        public bool HasResources(SegmentStats stats)
        {
            return Blood >= -stats.Blood &&
                   Energy >= -stats.Energy &&
                   Mechanical >= -stats.Mechanical;
        }

        public int Blood => _segments.Sum(segment => segment.Stats.Blood);
        public int Energy => _segments.Sum(segment => segment.Stats.Energy);
        public int Mechanical => _segments.Sum(segment => segment.Stats.Mechanical);
    }
}