using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Runtime.Components
{
    public class SegmentFactory : MonoSingleton<SegmentFactory>
    {
        [SerializeField] private MonoSegment _connectorBoxPrefab;
        [SerializeField] private ConnectionSlot _connectionSlotPrefab;
        [SerializeField] private Structure _structure = new();

        private void Start()
        {
            PlaceSegment(new Segment(new Position(), Rotation.Forward, Kind.ConnectorBox));
        }

        public void PlaceSegment(Segment segment)
        {
            var prefab = segment.Kind switch
            {
                Kind.ConnectorBox => _connectorBoxPrefab,
                Kind.Placeholder => _connectionSlotPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var monoSegment = Instantiate(prefab, segment.Position.AsVector3, Quaternion.identity);
            monoSegment.Segment = segment;
            _structure.AddSegment(segment);
        }

        public void SpawnPlaceholders(IEnumerable<Position> positions)
        {
            foreach (var position in positions.Where(_structure.IsAvailable))
            {
                var segment = new Segment(position, Rotation.Forward, Kind.Placeholder);
                PlaceSegment(segment);
            }
        }
    }
}