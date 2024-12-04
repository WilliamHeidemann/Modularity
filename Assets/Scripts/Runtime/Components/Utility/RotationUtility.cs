using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Components.Utility
{
    public static class RotationUtility
    {
        public static IEnumerable<Quaternion> AllRotations()
        {
            // Define the 6 primary orientations with each axis facing "up" (aligned with +Z)
            var primaryRotations = new[]
            {
                Quaternion.identity, // +Z
                Quaternion.Euler(90, 0, 0), // +X
                Quaternion.Euler(0, 90, 0), // +Y
                Quaternion.Euler(0, -90, 0), // -Y
                Quaternion.Euler(-90, 0, 0), // -X
                Quaternion.Euler(180, 0, 0) // -Z
            };

            // For each primary orientation, add 4 rotations around the current +Z axis
            foreach (var baseRotation in primaryRotations)
            {
                yield return baseRotation * Quaternion.Euler(0, 0, 0); // 0° around Z
                yield return baseRotation * Quaternion.Euler(0, 0, 90); // 90° around Z
                yield return baseRotation * Quaternion.Euler(0, 0, 180); // 180° around Z
                yield return baseRotation * Quaternion.Euler(0, 0, 270); // 270° around Z
            }
        }
    }
}