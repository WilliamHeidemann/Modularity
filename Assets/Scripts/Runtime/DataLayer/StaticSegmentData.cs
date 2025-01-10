using System;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;

namespace Runtime.DataLayer
{
    [CreateAssetMenu(menuName = "Connector")]
    public class StaticSegmentData : ScriptableObject
    {
        public ConnectionPoints ConnectionPoints;
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
            _ => throw new ArgumentOutOfRangeException($"Segment is neither blood nor steam: {name}")
        };
        
        protected bool Equals(StaticSegmentData other)
        {
            return base.Equals(other) && ConnectionPoints.Equals(other.ConnectionPoints) && IsBlood == other.IsBlood && IsSteam == other.IsSteam && IsSource == other.IsSource && BloodReward == other.BloodReward && SteamReward == other.SteamReward && BloodCost == other.BloodCost && SteamCost == other.SteamCost && BloodRequirements == other.BloodRequirements && SteamRequirements == other.SteamRequirements;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StaticSegmentData)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(ConnectionPoints);
            hashCode.Add(IsBlood);
            hashCode.Add(IsSteam);
            hashCode.Add(IsSource);
            hashCode.Add(BloodReward);
            hashCode.Add(SteamReward);
            hashCode.Add(BloodCost);
            hashCode.Add(SteamCost);
            hashCode.Add(BloodRequirements);
            hashCode.Add(SteamRequirements);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(StaticSegmentData left, StaticSegmentData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StaticSegmentData left, StaticSegmentData right)
        {
            return !Equals(left, right);
        }
    }
}