using System.Collections.Generic;
using System.Linq;
using Runtime.Scriptable_Objects;
using UnityEngine;

namespace Runtime.Components.Segments
{
    public class BloodReceiverActivatorNeeds2 : SegmentActivator
    {
        [SerializeField] StaticSegmentData _staticSegmentData;
        [SerializeField] Currency _currency;

        private List<Vector3Int> Sources = new();
        private List<Vector3Int> Connections = new();

        override public void Activate(Vector3Int source, Vector3Int connector){
            if (!IsActive)
            {
                if (!Sources.Contains(source)) Sources.Add(source);
                if (!Connections.Contains(connector)) Connections.Add(connector);
                
                if (Sources.Count() > 1 && Connections.Count() > 1)
                {
                    _currency.Add(_staticSegmentData.BloodReward,0);
                    IsActive = true;
                    return;
                }
            }
        }
    }
}