using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Backend
{
    public static class SpawnUtility
    {
        public static Vector3Int GetRandomSpawnPosition()
        {
            var spherePosition = Random.onUnitSphere * 2f;
            spherePosition.y = Mathf.Abs(spherePosition.y) + 1f;
            return spherePosition.AsVector3Int();
        }
        
        public static Vector3Int GetWeightedSpawnPosition(List<Vector3Int> positions, float distanceConstant)
        {
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
                (maxDistX - xCenter) +
                (maxDistY - yCenter) +
                (maxDistZ - zCenter)) / 3;

            var radius = averageDistToRelativeCenter + distanceConstant;

            var unitSpherePosition = Random.onUnitSphere;
            unitSpherePosition.y = Mathf.Abs(unitSpherePosition.y);

            var spawnPosition = unitSpherePosition * radius + offset;
            return spawnPosition.AsVector3Int();
        }
    }
}