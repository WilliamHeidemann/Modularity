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
    }
}