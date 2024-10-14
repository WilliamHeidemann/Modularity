using System;
using Runtime.Components.Segments;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Segment Factory")]
    public class SegmentFactory : ScriptableObject
    {
        [Header("Prefabs")] [SerializeField] private MonoSegment _connectorBoxPrefab;
        [SerializeField] private MonoSegment _cogsPrefab;
        [SerializeField] private MonoSegment _pipesPrefab;
        [SerializeField] private MonoSegment _heartPrefab;
        [SerializeField] private MonoSegment _eyesPrefab;
        [SerializeField] private MonoSegment _tentaclePrefab;
        [SerializeField] private MonoSegment _wingsPrefab;
        [SerializeField] private ConnectionSlot _connectionSlotPrefab;

        public void SpawnSegment(Segment segment)
        {
            var prefab = segment.Model switch
            {
                Model.ConnectorBox => _connectorBoxPrefab,
                Model.ConnectionSlot => _connectionSlotPrefab,
                Model.Cogs => _cogsPrefab,
                Model.Pipes => _pipesPrefab,
                Model.Heart => _heartPrefab,
                Model.Eyes => _eyesPrefab,
                Model.Tentacle => _tentaclePrefab,
                Model.Wings => _wingsPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var monoSegment = Instantiate(prefab, segment.Position.AsVector3, Quaternion.identity);
            monoSegment.Segment = segment;
        }
    }
}