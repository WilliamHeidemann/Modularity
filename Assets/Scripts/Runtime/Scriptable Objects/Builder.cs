using System.Linq;
using Codice.CM.Client.Differences.Merge;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEditor.Experimental.GraphView;
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

            var connector = Instantiate(_selection.Prefab, position, rotation);
            
            if (!_structure.CanConnect(position.AsVector3Int(),connector.AdjacentPlaceholderPositions().ToHashSet()))
            {
                Destroy(connector.gameObject);
                return;

            }
            // potentially remove old slot
            
            _resources.Pay(_selection.Price);
            _structure.SegmentPositions.Add(position.AsVector3Int());
            connector.AdjacentPlaceholderPositions().ForEach(SpawnSlot);
            

            //connectionpoints should not be randomized, rather defined by the prefab
            _selection.Prefab.ConnectionPoints.Randomize();
            
            // inform Strucuture of new segment
            _structure.AddSegment(position.AsVector3Int(), _selection.Prefab._staticSegmentData, connector.AdjacentPlaceholderPositions().ToHashSet());
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