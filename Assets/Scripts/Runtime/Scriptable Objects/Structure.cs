using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using TMPro;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Structure")]
    public class Structure : ScriptableObject
    {
        [SerializeField] private List<Segment> _segments = new();
        public bool IsAvailable(Position position) =>
            _segments.All(segment => segment.Position != position);

        public void AddSegment(Segment segment)
        {
            _segments.Add(segment);
        }
        
        public IEnumerable<Segment> AllSegments()
        {
            return _segments;
        }
        
        public void Clear()
        {
            _segments.Clear();
        }
    }
}