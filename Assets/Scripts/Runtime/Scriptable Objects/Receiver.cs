using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Receiver")]
    public class Receiver : StaticSegmentData
    {
        public bool NeedsBlood;
        public bool NeedsSteam;
        public int Reward;
    }
}