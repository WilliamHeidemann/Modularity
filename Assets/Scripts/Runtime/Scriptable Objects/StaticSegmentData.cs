using System;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Connector")]
    public class StaticSegmentData : ScriptableObject
    {
        public ConnectionPoints ConnectionPoints;
        public SegmentModel Model;
        public bool IsBlood;
        public bool IsSteam;
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

        public SoundFX SoundFX => IsReceiver switch
        {
            true when IsBlood && IsSteam => SoundFX.MixReceiverPlacement,
            true when IsBlood => SoundFX.FleshReceiverPlacement,
            true when IsSteam => SoundFX.SteamReceiverPlacement,
            false when IsBlood && IsSteam => SoundFX.MixConnectorPlacement,
            false when IsBlood => SoundFX.FleshConnectorPlacement,
            false when IsSteam => SoundFX.SteamConnectorPlacement,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}