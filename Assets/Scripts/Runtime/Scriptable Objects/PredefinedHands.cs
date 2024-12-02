using System.Collections.Generic;
using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class PredefinedHands : ScriptableObject
    {
        public List<Segment> BloodHand1;
        public List<Segment> BloodHand2;
        public List<Segment> BloodHand3;
        public List<Segment> BloodHand4;
        public List<Segment> BloodHand5;
        
        public List<List<Segment>> BloodHands => new()
        {
            BloodHand1,
            BloodHand2,
            BloodHand3,
            BloodHand4,
            BloodHand5
        };
        
        public List<Segment> SteamHand1;
        public List<Segment> SteamHand2;
        public List<Segment> SteamHand3;

        public List<List<Segment>> SteamHands => new()
        {
            SteamHand1,
            SteamHand2,
            SteamHand3
        };
        
        public List<Segment> Brains;
    }
}