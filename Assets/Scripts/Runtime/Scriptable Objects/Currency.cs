using System;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Currency : ScriptableObject
    {
        [SerializeField] private int _gears;
        public event Action<int> OnGearsChanged;

        public void Add(int amount)
        {
            _gears += amount;
            OnGearsChanged?.Invoke(_gears);
        }

        public void Pay(int amount)
        {
            _gears -= amount;
            OnGearsChanged?.Invoke(_gears);
        }

        public bool HasAtLeast(int amount)
        {
            return _gears >= amount;
        }

        public int Amount => _gears;

        public void Initialize()
        {
            _gears = 20;
            OnGearsChanged?.Invoke(_gears);
        }
    }
}