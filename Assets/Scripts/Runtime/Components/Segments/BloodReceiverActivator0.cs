using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class BloodReceiverActivator0 : SegmentActivator
    {
        [SerializeField] StaticSegmentData _staticSegmentData;
        [SerializeField] Currency _currency;
        override public void Activate(Vector3Int source, Vector3Int connector){
            if (!IsActive)
            {
                _currency.Add(_staticSegmentData.BloodReward,0);
                IsActive = true;
            }
        }
    }
}