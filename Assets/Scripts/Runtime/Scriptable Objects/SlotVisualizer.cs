using System;
using System.Collections.Generic;
using Runtime.DataLayer;
using UnityEngine;
using UnityEngine.Pool;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class SlotVisualizer : ScriptableObject
    {
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Structure _structure;
        private ObjectPool<GameObject> _objectPool;
        private readonly List<GameObject> _slots = new();


        public void Initialize()
        {
            _slots.Clear();
            _objectPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_slotPrefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: obj => obj.SetActive(false)
            );
        }

        public void VisualizeSlots(ConnectionType connectionType)
        {
            var positionAndRotations = _structure.GetOpenSlots(connectionType);
            foreach (var (position, rotation) in positionAndRotations)
            {
                var slot = _objectPool.Get();
                slot.transform.position = position;
                slot.transform.rotation = rotation;
                _slots.Add(slot);
            }
        }

        public void HideSlots()
        {
            foreach (GameObject slot in _slots)
            {
                _objectPool.Release(slot);
            }
            _slots.Clear();
        }
    }
}