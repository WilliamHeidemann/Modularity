using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class SteamReceiverActivator0 : SegmentActivator
    {
        [SerializeField] StaticSegmentData _staticSegmentData;
        [SerializeField] Currency _currency;
        override public void Activate(){
            if (!IsActive)
            {
                _currency.Add(0, _staticSegmentData.SteamReward);
                IsActive = true;
            }
        }
    }
}