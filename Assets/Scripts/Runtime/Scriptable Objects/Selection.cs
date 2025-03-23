using Runtime.Components.Segments;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Selection : ScriptableObject
    {
        public Option<Segment> Prefab
        {
            get => _prefab;
            set
            {
                Debug.Log($"Setting Prefab to {value}");
                _prefab = value;
            }
        }

        public int PriceBlood;
        public int PriceSteam;
        [SerializeField] private Option<Segment> _prefab;

        public void Reset()
        {
            Prefab = Option<Segment>.None;
            PriceBlood = 0;
            PriceSteam = 0;
        }
    }
}