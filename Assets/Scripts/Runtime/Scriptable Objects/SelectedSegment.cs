using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class SelectedSegment : ScriptableObject
    {
        public Segment Prefab;
    }
}