using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Builder : ScriptableObject
    {
        [Header("Prefabs")] 
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Selection _selection;

        [Header("Fields")] 
        [SerializeField] private Structure _structure;


        public void Build(Vector3Int position)
        {
            SpawnConnector(position, Quaternion.identity);
        }

        private void SpawnConnector(Vector3 position, Quaternion rotation)
        {
            if (_structure.SegmentPositions.Contains(position.AsVector3Int()))
            {
                return;
            }
            
            // potentially remove old slot
            
            var connector = Instantiate(_selection.Prefab, position, rotation);
            connector.ConnectionPoints.Randomize();
            _structure.SegmentPositions.Add(position.AsVector3Int());
            connector.AdjacentPlaceholderPositions().ForEach(SpawnSlot);
        }

        private void SpawnSlot(Vector3Int position)
        {
            if (_structure.TakenPositions.Contains(position))
            {
                return;
            }
            var slot = Instantiate(_slotPrefab, position, Quaternion.identity);
            slot.Position = position;
            _structure.SlotPositions.Add(position);
        }
    }
}