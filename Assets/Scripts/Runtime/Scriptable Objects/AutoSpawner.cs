using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Backend;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.DataLayer;
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
        [SerializeField] private float _distanceConstant;
        [SerializeField] private float _distancePercentage;
        [SerializeField] private Collectable _collectablePrefab;
        private readonly List<Collectable> _collectables = new();

        public delegate void CollectedCollectables(int collectedAmount);

        public static event CollectedCollectables OnCollectedCollectables;

        [Space]
        [Header("FIXED SOURCE SPAWN POSITIONS")]
        [SerializeField] private bool ShouldSpawnInPredefinedPosition;
        [Header("Heart")]
        [SerializeField] private Vector3Int HeartSpawnPosition;
        [SerializeField] private Vector3 HeartRotation;
        [Header("Furnace")]
        [SerializeField] private Vector3Int FurnaceSpawnPosition;
        [SerializeField] private Vector3 FurnaceRotation;

        public void Clear()
        {
            _collectables.Clear();
        }

        public void CheckForCollectables()
        {
            var segmentPositions = _structure.Segments.Select(s => s.Position).ToHashSet();
            var collectables = _collectables
                .Where(collectable => segmentPositions.Contains(collectable.Position))
                .ToList();

            if (collectables.Count > 0)
            {
                SoundFXPlayer.Instance.Play(SoundFX.OrbCollected);
            }

            _questFactory.CollectableCollected(collectables.Count);
            OnCollectedCollectables?.Invoke(collectables.Count);
            collectables.ForEach(c => _currencyPopup.GainCurrency(c.Position, c.StaticSegmentData));
            collectables.ForEach(c => _collectables.Remove(c));
            collectables.ForEach(c => Destroy(c.gameObject));
        }

        public void SpawnBloodSource()
        {
            if (ShouldSpawnInPredefinedPosition)
            {
                SpawnSource(_bloodSource, HeartSpawnPosition, Quaternion.Euler(HeartRotation));
            }
            else
            {
                SpawnRandomSource(_bloodSource);
            }
        }

        public void SpawnSteamSource()
        {
            if (ShouldSpawnInPredefinedPosition)
            {
                SpawnSource(_steamSource, FurnaceSpawnPosition, Quaternion.Euler(FurnaceRotation));
            }
            else
            {
                SpawnRandomSource(_steamSource);
            }
        }

        private void SpawnRandomSource(Segment source)
        {
            Vector3Int spawnPosition = SpawnUtility.Get(GetSpawnPosition, _structure.IsValidSourcePlacement);
            Quaternion spawnRotation = GetRandomRotation();
            SpawnSource(source, spawnPosition, spawnRotation);
        }

        private void SpawnSource(Segment source, Vector3Int spawnPosition, Quaternion rotation)
        {
            _selection.Reset();
            _selection.Prefab = Option<Segment>.Some(source);
            _builder.Build(spawnPosition, rotation, true);
            _selection.Reset();
        }

        private Vector3Int GetSpawnPosition()
        {
            if (!_structure.Segments.Any())
            {
                return SpawnUtility.GetRandomSpawnPosition();
            }

            var positions = _structure.Segments.Select(segment => segment.Position).ToList();
            return SpawnUtility.GetWeightedSpawnPosition(positions, _distanceConstant, _distancePercentage);
        }

        private Quaternion GetRandomRotation() => RotationUtility.AllRotations().RandomElement();

        public void SpawnCollectable()
        {
            var spawnPosition = SpawnUtility.Get(GetSpawnPosition, _structure.IsValidSourcePlacement);
            var collectable = Instantiate(_collectablePrefab, spawnPosition, Quaternion.identity);
            collectable.Position = spawnPosition;
            _collectables.Add(collectable);
            SoundFXPlayer.Instance.Play(SoundFX.OrbSpawn);
        }
    }
}