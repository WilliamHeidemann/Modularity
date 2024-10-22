using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components.Segments
{
    public class Connector : MonoSegment
    {
        [SerializeField] private bool _connectsUp;
        [SerializeField] private bool _connectsDown;
        [SerializeField] private bool _connectsForward;
        [SerializeField] private bool _connectsBack;
        [SerializeField] private bool _connectsRight;
        [SerializeField] private bool _connectsLeft;

        public static event Action<IEnumerable<Position>> OnSpawnSlots; 
        
        private void Start()
        {
            SpawnConnections();
        }

        private void SpawnConnections()
        {
            var slots = PlaceholderPositions().Select(p => p + Segment.Position);
            OnSpawnSlots?.Invoke(slots);
        }

        private IEnumerable<Position> PlaceholderPositions()
        {
            if (_connectsUp) yield return Position.Up;
            if (_connectsDown) yield return Position.Down;
            if (_connectsForward) yield return Position.Forward;
            if (_connectsBack) yield return Position.Back;
            if (_connectsRight) yield return Position.Right;
            if (_connectsLeft) yield return Position.Left;
        }
    }
}
