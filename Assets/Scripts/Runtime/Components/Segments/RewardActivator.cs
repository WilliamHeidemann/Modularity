using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Utility;
using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class RewardActivator : SegmentActivator
    {
        [SerializeField] private StaticSegmentData _staticSegmentData;
        [SerializeField] private Currency _currency;
        
        public override void Activate()
        {
            if (IsActive)
            {
                return;
            }
            Debug.Log(4);

            GrantReward();
            IsActive = true;
        }
        private void GrantReward()
        {
            _currency.Add(_staticSegmentData.BloodReward,_staticSegmentData.SteamReward);
        }
    }
}