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
        [SerializeField] private Structure _structure;
        [SerializeField] private Resources _resources;


        public void Build(Vector3Int position, Quaternion placeholderRotation = new())
        {
            SpawnConnector(position, placeholderRotation);
        }

        private void SpawnConnector(Vector3 position, Quaternion rotation)
        {
            if (_structure.SegmentPositions.Contains(position.AsVector3Int()))
            {
                return;
            }

            if (!_resources.HasAtLeast(_selection.Price))
            {
                return;
            }
            
            // potentially remove old slot
            
            _resources.Pay(_selection.Price);
            var connector = Instantiate(_selection.Prefab, position, rotation);
            _structure.SegmentPositions.Add(position.AsVector3Int());
            connector.AdjacentPlaceholderPositions().ForEach(SpawnSlot);
            _selection.Prefab.ConnectionPoints.Randomize();
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