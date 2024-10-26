using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        public readonly HashSet<Vector3Int> SegmentPositions = new();
        public readonly HashSet<Vector3Int> SlotPositions = new();
        public HashSet<Vector3Int> TakenPositions => SegmentPositions.Concat(SlotPositions).ToHashSet();
    }
}