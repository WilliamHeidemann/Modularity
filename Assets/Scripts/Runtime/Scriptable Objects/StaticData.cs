using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Connector")]
    public class StaticData : ScriptableObject
    {
        public ConnectionPoints ConnectionPoints;
        public SegmentModel Model;
    }
}