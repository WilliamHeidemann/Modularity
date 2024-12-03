using System.Collections.Generic;
using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PredefinedHands : ScriptableObject
    {
        public List<Segment> BloodHand1;
        public List<Segment> BloodHand2;

        public List<List<Segment>> BloodHands => new()
        {
            BloodHand1,
            BloodHand2,
        };

        public List<Segment> SteamHand1;
        public List<Segment> SteamHand2;

        public List<List<Segment>> SteamHands => new()
        {
            SteamHand1,
            SteamHand2
        };

        public List<Segment> Producers;
        public List<Segment> Hybrids;
    }
}