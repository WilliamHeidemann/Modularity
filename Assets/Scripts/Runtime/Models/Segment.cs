using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Models
{
    [Serializable]
    public class Segment
    {
        [SerializeField] private Position _position;
        [SerializeField] private Model _model;
        [SerializeField] private SegmentData _data;

        public Segment(Position position, Card card)
        {
            _position = position;
            _model = card.Model;
            _data = card.SegmentData;
        }

        public Position Position => _position;
        public Model Model => _model;
        public SegmentData Data => _data;

        public void SetPosition(Position position) => _position = position;
        
        public override string ToString()
        {
            return $"Position: {Position}, Model: {Model}";
        }

        public static Segment StartingSegment(Position position) => 
            new(position, Card.StartingCard);
    }
}