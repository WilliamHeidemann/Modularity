using System;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Models
{
    [Serializable]
    public struct Segment
    {
        [SerializeField] private Position _position;
        [SerializeField] private Model _model;
        [SerializeField] private Supply _supply;

        public Segment(Position position, Card card)
        {
            _position = position;
            _model = card.Model;
            _supply = card.Supply;
        }

        public Position Position => _position;
        public Model Model => _model;
        public Supply Supply => _supply;
        
        public override string ToString()
        {
            return $"Position: {Position}, Model: {Model}";
        }
    }
}