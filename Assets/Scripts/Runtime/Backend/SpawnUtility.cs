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
        public static T Get<T>(Func<T> create, Predicate<T> predicate)
        {
            var item = create();
            for (int i = 0; i < 30; i++)
            {
                if (predicate(item))
                {
                    return item;
                }

                item = create();
            }

            return item;
        }
        
        public static Vector3Int GetRandomSpawnPosition()
        {
            var spherePosition = Random.onUnitSphere * 2f;
            spherePosition.y = Mathf.Abs(spherePosition.y) + 1f;
            return spherePosition.AsVector3Int();
        }

        public static Vector3Int GetWeightedSpawnPosition(List<Vector3Int> positions, float distanceConstant, float distancePercentage)
        {
            var offset = GetCenter(positions);
            var radius = GetRadius(positions, distanceConstant, distancePercentage);
            
            var unitSpherePosition = Random.onUnitSphere;
            unitSpherePosition.y = Mathf.Abs(unitSpherePosition.y);

            var spawnPosition = unitSpherePosition * radius + offset;
            return spawnPosition.AsVector3Int();
        }

        public static Vector3 GetCenter(List<Vector3Int> positions)
        {
            var minX = positions.Min(position => position.x);
            var maxX = positions.Max(position => position.x);
            var minY = positions.Min(position => position.y);
            var maxY = positions.Max(position => position.y);
            var minZ = positions.Min(position => position.z);
            var maxZ = positions.Max(position => position.z);

            var xCenter = (maxX + minX) / 2f;
            var yCenter = (maxY + minY) / 2f;
            var zCenter = (maxZ + minZ) / 2f;
            return new Vector3(xCenter, yCenter, zCenter);
        }

        public static float GetRadius(List<Vector3Int> positions, float distanceConstant, float distancePercentage)
        {
            var minX = positions.Min(position => position.x);
            var maxX = positions.Max(position => position.x);
            var minY = positions.Min(position => position.y);
            var maxY = positions.Max(position => position.y);
            var minZ = positions.Min(position => position.z);
            var maxZ = positions.Max(position => position.z);

            var xCenter = (maxX + minX) / 2f;
            var yCenter = (maxY + minY) / 2f;
            var zCenter = (maxZ + minZ) / 2f;
            var offset = new Vector3(xCenter, yCenter, zCenter);

            var maxDistX = Math.Max(maxX - xCenter, Math.Abs(xCenter - minX));
            var maxDistY = Math.Max(maxY - yCenter, Math.Abs(yCenter - minY));
            var maxDistZ = Math.Max(maxZ - zCenter, Math.Abs(zCenter - minZ));

            var maxDistRelativeToCenter = new Vector3(maxDistX, maxDistY, maxDistZ);

            var dp = 1 + distancePercentage/100;
            var radius = (Vector3.Distance(maxDistRelativeToCenter, offset)/3 * dp) + distanceConstant;
            return radius;
        }
    }
}