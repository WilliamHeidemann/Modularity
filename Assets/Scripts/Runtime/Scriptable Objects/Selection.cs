using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Selection : ScriptableObject
    {
        public Segment Prefab;
    }
}