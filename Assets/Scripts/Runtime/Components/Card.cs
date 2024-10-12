using System;
using Runtime.Models;
using TMPro;
using UnityEngine;

namespace Runtime.Components
{
    public class Card : MonoBehaviour
    {
        public Kind Kind;
        [SerializeField] private TextMeshProUGUI _blood;
        [SerializeField] private TextMeshProUGUI _energy;
        [SerializeField] private TextMeshProUGUI _mechanical;

        private Segment _segment;
        
        private void Start()
        {
            _segment = Segment.Default(Kind);
            _blood.text = $"Blood: {_segment.Stats.Blood.ToString()}";
            _energy.text = $"Energy: {_segment.Stats.Energy.ToString()}";
            _mechanical.text = $"Mechanical: {_segment.Stats.Mechanical.ToString()}";
        }

        public void Select()
        {
            SegmentFactory.Instance.Select(_segment);
        }
    }
}
