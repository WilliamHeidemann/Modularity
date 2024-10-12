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
        [SerializeField] private Structure _structure = new();
        [SerializeField] private Segment _toBuild = new Segment(new Position(), Kind.ConnectorBox, SegmentStats.ConnectorBox);

        [Header("Prefabs")]
        [SerializeField] private MonoSegment _connectorBoxPrefab;
        [SerializeField] private MonoSegment _cogsPrefab;
        [SerializeField] private MonoSegment _pipesPrefab;
        [SerializeField] private ConnectionSlot _connectionSlotPrefab;
        
        private void Start()
        {
            PlaceSegment(new Segment(new Position(), Kind.ConnectorBox, SegmentStats.None));
        }

        public void Select(Segment segment)
        {
            _toBuild = segment;
        }
        
        public void TryBuild(Position position)
        {
            _toBuild.SetPosition(position);
            PlaceSegment(_toBuild);
        }
        
        private void PlaceSegment(Segment segment)
        {
            if (!_structure.HasResources(segment.Stats))
            {
                print("Not enough resources");
                return;
            }
            
            var prefab = segment.Kind switch
            {
                Kind.ConnectorBox => _connectorBoxPrefab,
                Kind.ConnectionSlot => _connectionSlotPrefab,
                Kind.Cogs => _cogsPrefab,
                Kind.Pipes => _pipesPrefab,
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
                var segment = new Segment(position, Kind.ConnectionSlot, SegmentStats.None);
                PlaceSegment(segment);
            }
        }
    }
}