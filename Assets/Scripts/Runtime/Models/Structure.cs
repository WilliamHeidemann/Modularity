using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Runtime.Models
{
    [Serializable]
    public class Structure
    {
        private readonly TextMeshProUGUI _bloodText;
        private readonly TextMeshProUGUI _energyText;
        private readonly TextMeshProUGUI _mechanicalText;
        [SerializeField] private List<Segment> _segments = new();
        
        public Structure(TextMeshProUGUI bloodText, TextMeshProUGUI energyText, TextMeshProUGUI mechanicalText)
        {
            _bloodText = bloodText;
            _energyText = energyText;
            _mechanicalText = mechanicalText;
        }
        
        public bool IsAvailable(Position position) => 
            _segments.All(segment => segment.Position != position);
        public void AddSegment(Segment segment)
        {
            _segments.Add(segment);
            _bloodText.text = $"Blood: {Blood.ToString()}";
            _energyText.text = $"Energy: {Energy.ToString()}";
            _mechanicalText.text = $"Mechanical: {Mechanical.ToString()}";
        }

        public bool HasResources(SegmentData data)
        {
            return Blood >= -data.Blood &&
                   Energy >= -data.Energy &&
                   Mechanical >= -data.Mechanical;
        }

        public int Blood => _segments.Sum(segment => segment.Data.Blood);
        public int Energy => _segments.Sum(segment => segment.Data.Energy);
        public int Mechanical => _segments.Sum(segment => segment.Data.Mechanical);
    }
}