using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Components.Gameplay
{
    public class Connectable : MonoSegment
    {
        [SerializeField] private ConnectionSlot _connectionSlot;
        
        [SerializeField] private bool _connectsUp;
        [SerializeField] private bool _connectsDown;
        [SerializeField] private bool _connectsForward;
        [SerializeField] private bool _connectsBack;
        [SerializeField] private bool _connectsRight;
        [SerializeField] private bool _connectsLeft;
        
        private void Start()
        {
            SpawnConnections();
        }

        private void SpawnConnections()
        {
            var placeholderPositions = PlaceholderPositions().Select(p => p + Segment.Position);
            SegmentFactory.Instance.SpawnPlaceholders(placeholderPositions);
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
