using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Gameplay
{
    public class SegmentFactory : MonoSingleton<SegmentFactory>
    {
        [Header("Prefabs")] [SerializeField] private MonoSegment _connectorBoxPrefab;
        [SerializeField] private MonoSegment _cogsPrefab;
        [SerializeField] private MonoSegment _pipesPrefab;
        [SerializeField] private MonoSegment _heartPrefab;
        [SerializeField] private MonoSegment _eyesPrefab;
        [SerializeField] private MonoSegment _tentaclePrefab;
        [SerializeField] private MonoSegment _wingsPrefab;
        [SerializeField] private ConnectionSlot _connectionSlotPrefab;

        private void Start()
        {
            StructureManager.OnSegmentAdded += SpawnSegment;
        }

        private void OnDisable()
        {
            StructureManager.OnSegmentAdded -= SpawnSegment;
        }

        private void SpawnSegment(Segment segment)
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