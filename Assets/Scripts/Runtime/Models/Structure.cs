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
        public void AddSegment(Segment segment) => _segments.Add(segment);
    }
}