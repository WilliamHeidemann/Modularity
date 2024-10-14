using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Models
{
    [Serializable]
    public struct Supply
    {
        [SerializeField] private int _blood;
        [SerializeField] private int _energy;
        [SerializeField] private int _mechanical;

        private Supply(int blood, int energy, int mechanical)
        {
            _blood = blood;
            _energy = energy;
            _mechanical = mechanical;
        }
        
        public int Blood => _blood;
        public int Energy => _energy;
        public int Mechanical => _mechanical;

        public Supply WithVariation()
        {
            _blood += Random.Range(-1, 2);
            _energy += Random.Range(-1, 2);
            _mechanical += Random.Range(-1, 2);
            return this;
        }
    }
}