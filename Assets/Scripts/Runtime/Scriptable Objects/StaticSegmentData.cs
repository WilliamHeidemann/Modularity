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
        public bool IsSource;
        public int BloodReward;
        public int SteamReward;
        public int BloodCost;
        public int SteamCost;
        public int BloodRequirements;
        public int SteamRequirements;

        public bool IsReceiver => BloodRequirements > 0 || SteamRequirements > 0;
        public int Requirements => BloodRequirements + SteamRequirements;
        public bool IsConnector => !IsReceiver && !IsSource;
    }
}