using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net.DateFormatter;
using NUnit.Framework;
using Runtime.Backend;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;
using Random = UnityEngine.Random;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class AutoSpawner : ScriptableObject
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private Builder _builder;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _bloodSource;
        [SerializeField] private Segment _steamSource;
        [SerializeField] private float DistanceConstant;
        [SerializeField] private Collectable _collectablePrefab;
        private readonly List<Collectable> _collectables = new();

        public void Clear() => _collectables.Clear();
        public List<Collectable> Collectables => _collectables;


        public void SpawnBloodSource() => SpawnRandomSource(_bloodSource);
        public void SpawnSteamSource() => SpawnRandomSource(_steamSource);

        private void SpawnRandomSource(Segment source)
        {
            var spawnPosition = GetSpawnPosition();
            for (int i = 0; i < 10; i++)
            {
                if (_structure.IsValidSourcePlacement(spawnPosition))
                {
                    break;
                }

                spawnPosition = GetSpawnPosition();
            }


            _selection.Reset();
            _selection.Prefab = Option<Segment>.Some(source);
            _builder.Build(spawnPosition, Quaternion.identity, true);
            _selection.Reset();
        }

        private Vector3Int GetSpawnPosition()
        {
            if (!_structure.Segments.Any())
            {
                return SpawnUtility.GetRandomSpawnPosition();
            }

            var positions = _structure.Segments.Select(segment => segment.Position).ToList();
            return SpawnUtility.GetWeightedSpawnPosition(positions, DistanceConstant);
        }

        public void SpawnCollectable()
        {
            var spawnPosition = GetSpawnPosition();
            for (int i = 0; i < 10; i++)
            {
                if (_structure.IsValidSourcePlacement(spawnPosition))
                {
                    break;
                }

                spawnPosition = GetSpawnPosition();
            }

            var collectable = Instantiate(_collectablePrefab, spawnPosition, Quaternion.identity);
            collectable.Position = spawnPosition;
            _collectables.Add(collectable);
        }
    }
}