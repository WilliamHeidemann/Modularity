using System;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Currency : ScriptableObject
    {
        [SerializeField] private int _gears;

        public int GetGearAmount()
        {
            return _gears;
        }

        public void Add(int amount)
        {
            _gears += amount;
        }

        public void Pay(int amount)
        {
            _gears -= amount;
        }

        public bool HasAtLeast(int amount)
        {
            return _gears >= amount;
        }
    }
}