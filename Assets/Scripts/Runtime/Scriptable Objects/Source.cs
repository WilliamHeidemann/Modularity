using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Source")]
    public class Source : StaticSegmentData
    {
        public bool Blood;
        public bool Steam;
        public int Reach;
    }
}