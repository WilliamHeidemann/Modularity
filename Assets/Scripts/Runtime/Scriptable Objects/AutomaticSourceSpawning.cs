using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net.DateFormatter;
using NUnit.Framework;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UtilityToolkit.Runtime;
using Random = UnityEngine.Random;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class AutomaticSourceSpawning : ScriptableObject
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private Builder _builder;
        [SerializeField] private Selection _selection;
        [SerializeField] private Segment _bloodSource;
        [SerializeField] private Segment _steamSource;
        [SerializeField] private float DistanceConstant;
        [SerializeField] private List<Collectable> _collectables = new();
        [SerializeField] private  Collectable _whisp;

        public void Clear() => _collectables.Clear();

        public void SpawnRandomSource()
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
            
            
            var source = Random.value < 0.5f ? _bloodSource : _steamSource;

            _selection.Reset();
            _selection.Prefab = Option<Segment>.Some(source);
            _builder.Build(spawnPosition, Quaternion.identity, true);
            _selection.Reset();
        }

        private Vector3Int GetSpawnPosition()
        {
            var positions = _structure.Segments.Select(segment => segment.Position).ToList();
            var minX = positions.Min(position => position.x);
            var maxX = positions.Max(position => position.x);
            var minY = positions.Min(position => position.y);
            var maxY = positions.Max(position => position.y);
            var minZ = positions.Min(position => position.z);
            var maxZ = positions.Max(position => position.z);

            var xCenter = (maxX + minX) / 2;
            var yCenter = (maxY + minY) / 2;
            var zCenter = (maxZ + minZ) / 2;
            var offset = new Vector3(xCenter, yCenter, zCenter);

            var maxDistX = Math.Max(maxX, Math.Abs(minX));
            var maxDistY = Math.Max(maxY, Math.Abs(minY));
            var maxDistZ = Math.Max(maxZ, Math.Abs(minZ));
            
            var averageDistToRelativeCenter = (
                (maxDistX - xCenter)+
                (maxDistY - yCenter)+
                (maxDistZ - zCenter)) /3;

            var radius = averageDistToRelativeCenter + DistanceConstant;

            var unitSpherePosition = Random.onUnitSphere;
            unitSpherePosition.y = Mathf.Abs(unitSpherePosition.y);
            
            var spawnPosition = unitSpherePosition * radius + offset;
            return spawnPosition.AsVector3Int();
        }

        public void SpawnRandomWhisp()
        {
            var spawnPosition = GetSpawnPosition();
            //clears throat
            for (int i = 0; i < 10; i++)
            {
                if (_structure.IsValidSourcePlacement(spawnPosition))
                {
                    break;
                }

                spawnPosition = GetSpawnPosition();
            }

            var whisp = Instantiate(_whisp, spawnPosition, Quaternion.identity);
            whisp.position = spawnPosition;
            _collectables.Add(whisp);
        }

        public Collectable GetCollectable(Vector3Int position)
        {
            var collectable = _collectables.First(c => c.position == position);
            _collectables.Remove(collectable);
            return collectable;
        }
        public bool IsOnCollectable(Vector3Int position) => _collectables.Any(collectable => collectable.position == position);
        
    }
}