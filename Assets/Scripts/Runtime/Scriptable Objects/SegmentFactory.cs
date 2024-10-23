using System;
using Runtime.Components.Segments;
using Runtime.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Segment Factory")]
    public class SegmentFactory : ScriptableObject
    {
        [Header("Prefabs")] 
        [SerializeField] private Connector _connectorPrefab;
        [SerializeField] private Slot _slotPrefab;

        public void SpawnSegment(Segment segment)
        {
            var prefab = segment.Model switch
            {
                Model.ConnectorBox => _connectorPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var monoSegment = Instantiate(prefab, segment.Position.AsVector3, Quaternion.identity);
            monoSegment.Segment = segment;
        }

        public void SpawnSlot(Position position)
        {
            var slot = Instantiate(_slotPrefab, position.AsVector3, Quaternion.identity);
            slot.Position = position;
        }
    }
}