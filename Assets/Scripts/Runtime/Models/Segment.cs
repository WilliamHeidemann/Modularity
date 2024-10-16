using System;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Models
{
    [Serializable]
    public struct Segment
    {
        [field: SerializeField] public Position Position { get; private set; }
        [field: SerializeField] public Model Model { get; private set; }
        [field: SerializeField] public Supply Supply { get; private set; }

        public Segment(Position position, Card card)
        {
            Position = position;
            Model = card.Model;
            Supply = card.Supply.WithVariation();
        }
        
        public override string ToString()
        {
            return $"Position: {Position}, Model: {Model}";
        }
    }
}