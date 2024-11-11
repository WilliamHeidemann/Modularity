using System;
using Runtime.Components.Segments;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Connector")]
    public class StaticSegmentData : ScriptableObject
    {
        public ConnectionPoints ConnectionPoints;
        public SegmentModel Model;
        public bool Blood;
        public bool Steam;
        public int Resistance;
        public int Power;
        public int BloodReward;
        public int SteamReward;
        public int BloodCost;
        public int SteamCost;

        public bool isReciever => SteamReward > 0 || BloodReward > 0;
    }
}