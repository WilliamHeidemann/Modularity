using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Source")]
    public class Source : StaticSegmentData
    {
        public int Blood;
        public int Steam;
    }
}