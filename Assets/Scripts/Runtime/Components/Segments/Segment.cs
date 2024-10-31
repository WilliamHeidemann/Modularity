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
        [SerializeField] public StaticSegmentData StaticSegmentData;
    }
}