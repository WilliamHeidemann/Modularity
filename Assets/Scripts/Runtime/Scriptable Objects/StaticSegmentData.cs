using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Connector")]
    public class StaticSegmentData : ScriptableObject
    {
        public ConnectionPoints ConnectionPoints;
        public SegmentModel Model;
        public int Resistance;
    }
}