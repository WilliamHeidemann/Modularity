using Runtime.Components;
using Runtime.Components.Segments;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Builder : ScriptableObject
    {
        [Header("Prefabs")] [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Segment _connectorPrefab;

        [Header("Fields")] [SerializeField] private Structure _structure;

        // Segments and slots may not be spawned in positions that are already occupied! 

        public void Build(Vector3Int position)
        {
            SpawnConnector(position, Quaternion.identity);
        }

        private void SpawnConnector(Vector3 position, Quaternion rotation)
        {
            var connector = Instantiate(_connectorPrefab, position, rotation);
            connector.AdjacentPlaceholderPositions().ForEach(SpawnSlot);
        }

        private void SpawnSlot(Vector3Int position)
        {
            var slot = Instantiate(_slotPrefab, position, Quaternion.identity);
            slot.Position = position;
        }
    }
}