using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Models
{
    [Serializable]
    public struct Supply
    {
        [field: SerializeField] public int Blood { get; private set; }
        [field: SerializeField] public int Energy { get; private set; }
        [field: SerializeField] public int Mechanical { get; private set; }

        private Supply(int blood, int energy, int mechanical)
        {
            Blood = blood;
            Energy = energy;
            Mechanical = mechanical;
        }

        public Supply WithVariation()
        {
            Blood += Random.Range(-4, 4);
            Energy += Random.Range(-4, 4);
            Mechanical += Random.Range(-4, 4);
            return this;
        }
    }
}