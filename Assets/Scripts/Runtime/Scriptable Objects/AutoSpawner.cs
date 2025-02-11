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
        [SerializeField] private GameObject _collectablePickupFX;
        private readonly List<Collectable> _collectables = new();
        public delegate void CollectedCollectables(int collectedAmount);
        public static event CollectedCollectables OnCollectedCollectables;

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
            collectables.ForEach(c => Instantiate(_collectablePickupFX, c.Position, Quaternion.identity));
            collectables.ForEach(c => Destroy(c.gameObject));
        }

        public void SpawnBloodSource() => SpawnRandomSource(_bloodSource);
        public void SpawnSteamSource() => SpawnRandomSource(_steamSource);

        private void SpawnRandomSource(Segment source)
        {
            var spawnPosition = SpawnUtility.Get(GetSpawnPosition, _structure.IsValidSourcePlacement);
            _selection.Reset();
            _selection.Prefab = Option<Segment>.Some(source);
            _builder.Build(spawnPosition, GetRandomRotation(), true);
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