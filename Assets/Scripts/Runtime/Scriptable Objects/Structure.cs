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
        public static event Action<int, int, int> OnSupplyChanged;
        
        public bool IsAvailable(Position position) =>
            _segments.All(segment => segment.Position != position);

        public void AddSegment(Segment segment)
        {
            _segments.Add(segment);
            OnSupplyChanged?.Invoke(Blood, Energy, Mechanical);
        }

        public bool HasResources(Supply supply)
        {
            return Blood >= -supply.Blood &&
                   Energy >= -supply.Energy &&
                   Mechanical >= -supply.Mechanical;
        }

        public IEnumerable<Segment> AllSegments()
        {
            return _segments;
        }
        
        public void Clear()
        {
            _segments.Clear();
            OnSupplyChanged?.Invoke(Blood, Energy, Mechanical);
        }

        public int Blood => _segments.Sum(segment => segment.Supply.Blood);
        public int Energy => _segments.Sum(segment => segment.Supply.Energy);
        public int Mechanical => _segments.Sum(segment => segment.Supply.Mechanical);
    }
}