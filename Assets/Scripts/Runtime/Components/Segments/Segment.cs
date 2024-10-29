using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime.Components.Segments
{
    public class Segment : MonoBehaviour
    {
        public ConnectionPoints ConnectionPoints;
        [SerializeField] public StaticSegmentData StaticSegmentData;
        
        public IEnumerable<Vector3Int> AdjacentPlaceholderPositions() =>
            ConnectionPoints
                .AsVector3Ints()
                .Select(direction => transform.TransformDirection(direction))
                .Select(direction => direction.AsVector3Int())
                .Select(direction => transform.position.AsVector3Int() + direction);
    }
}