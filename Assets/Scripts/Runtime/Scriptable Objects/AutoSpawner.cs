using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private CurrencyPopup _currencyPopup;
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private Segment _bloodSource;
        [SerializeField] private Segment _steamSource;
        [SerializeField] private float DistanceConstant;
        [SerializeField] private Collectable _collectablePrefab;
        private readonly List<Collectable> _collectables = new();
        private bool _shouldSpawnCollectables;

        public void Clear()
        {
            _collectables.Clear();
            _shouldSpawnCollectables = false;
        }

        public void StartSpawningCollectables() => _shouldSpawnCollectables = true;


        public void CheckForCollectables()
        {
            var segmentPositions = _structure.Segments.Select(s => s.Position).ToHashSet();
            var collectables = _collectables
                .Where(collectable => segmentPositions.Contains(collectable.Position))
                .ToList();

            _questFactory.CollectableCollected(collectables.Count);
            collectables.ForEach(c => _currencyPopup.Activate(c.Position, c.StaticSegmentData));
            collectables.ForEach(c => _collectables.Remove(c));
            collectables.ForEach(c => Destroy(c.gameObject));

            CheckToSpawnCollectables();
        }

        private void CheckToSpawnCollectables()
        {
            if (!_shouldSpawnCollectables)
            {
                return;
            }

            if (_collectables.Any())
            {
                return;
            }

            SpawnCollectable();
            SpawnCollectable();
            SpawnBloodSource();
            SpawnSteamSource();
        }

        public void SpawnBloodSource() => SpawnRandomSource(_bloodSource);
        public void SpawnSteamSource() => SpawnRandomSource(_steamSource);

        private void SpawnRandomSource(Segment source)
        {
            var spawnPosition = SpawnUtility.Get(GetSpawnPosition, _structure.IsValidSourcePlacement);
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
            var spawnPosition = SpawnUtility.Get(GetSpawnPosition, _structure.IsValidSourcePlacement);
            var collectable = Instantiate(_collectablePrefab, spawnPosition, Quaternion.identity);
            collectable.Position = spawnPosition;
            _collectables.Add(collectable);
        }
    }
}