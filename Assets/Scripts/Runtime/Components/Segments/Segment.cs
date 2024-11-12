using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class Segment : MonoBehaviour
    {
        [SerializeField] public StaticSegmentData StaticSegmentData;
        [SerializeField] public SegmentActivator SegmentActivator;
        public Sprite Preview;
    }
}