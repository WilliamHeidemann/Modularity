using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Selection : ScriptableObject
    {
        public Option<Segment> Prefab;
        public int PriceBlood;
        public int PriceSteam;

        public void Reset()
        {
            Prefab = Option<Segment>.None;
            PriceBlood = 0;
            PriceSteam = 0;
        }
    }
}