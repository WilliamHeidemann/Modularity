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
        [SerializeField] private GameObject _fleshSlotPrefab;
        [SerializeField] private GameObject _metalSlotPrefab;
        
        [SerializeField] private Structure _structure;
        private ObjectPool<GameObject> _fleshPool;
        private ObjectPool<GameObject> _metalPool;
        private readonly List<GameObject> _fleshSlots = new();
        private readonly List<GameObject> _metalSlots = new();

        public void Initialize()
        {
            _fleshSlots.Clear();
            _fleshPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_fleshSlotPrefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: obj => obj.SetActive(false)
            );
            
            _metalPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_metalSlotPrefab),
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
                var slot = connectionType switch
                {
                    ConnectionType.None => throw new ArgumentOutOfRangeException(nameof(connectionType), connectionType, null),
                    ConnectionType.Blood => _fleshPool.Get(),
                    ConnectionType.Steam => _metalPool.Get(),
                    _ => throw new ArgumentOutOfRangeException(nameof(connectionType), connectionType, null)
                };
                
                slot.transform.position = position;
                slot.transform.rotation = rotation;
                
                var slots = connectionType switch
                {
                    ConnectionType.None => throw new ArgumentOutOfRangeException(nameof(connectionType), connectionType, null),
                    ConnectionType.Blood => _fleshSlots,
                    ConnectionType.Steam => _metalSlots,
                    _ => throw new ArgumentOutOfRangeException(nameof(connectionType), connectionType, null)
                };
                
                slots.Add(slot);
            }
        }

        public void HideSlots()
        {
            foreach (GameObject slot in _fleshSlots)
            {
                _fleshPool.Release(slot);
            }
            _fleshSlots.Clear();
            
            foreach (GameObject slot in _metalSlots)
            {
                _metalPool.Release(slot);
            }
            _metalSlots.Clear();
        }
    }
}