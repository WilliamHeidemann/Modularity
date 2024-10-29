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
            SpawnSelection(position, placeholderRotation);
        }

        private void SpawnSelection(Vector3 position, Quaternion rotation)
        {
            if (!_structure.IsOpenPosition(position.AsVector3Int()))
            {
                return;
            }

            if (!_resources.HasAtLeast(_selection.Price))
            {
                return;
            }

            
            if (!_structure.ConnectsToSomething(position.AsVector3Int()))
            {
                return;
            }
            // potentially remove old slot
            
            var connector = Instantiate(_selection.Prefab, position, rotation);
            _resources.Pay(_selection.Price);
            _structure.AddSegment(connector);
            connector.AdjacentPlaceholderPositions().ForEach(SpawnSlot);
            
            //connectionpoints should not be randomized, rather defined by the prefab
            _selection.Prefab.ConnectionPoints.Randomize();
        }

        private void SpawnSlot(Vector3Int position)
        {
            if (!_structure.IsOpenPosition(position) && !_structure.SlotPositions.Contains(position))
            {
                return;
            }
            var slot = Instantiate(_slotPrefab, position, Quaternion.identity);
            slot.Position = position;
            _structure.SlotPositions.Add(position);
        }
    }
}