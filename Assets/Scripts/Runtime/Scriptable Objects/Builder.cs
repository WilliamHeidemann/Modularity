using System.Linq;
using Runtime.Components;
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


        public void Build(Vector3Int position, Quaternion placeholderRotation)
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
            
            var segmentData = new SegmentData
            {
                Position = position.AsVector3Int(),
                Rotation = rotation,
                StaticSegmentData = _selection.Prefab.StaticSegmentData,
            };
            
            if (!_structure.IsEmpty && !_structure.ConnectsToSomething(segmentData))
            {
                return;
            }
            // potentially remove old slot
            
            var connector = Instantiate(_selection.Prefab, position, rotation);
            segmentData.GetConnectionPoints().ForEach(SpawnSlot);
            _structure.AddSegment(segmentData);
            _resources.Pay(_selection.Price);
            
            //connectionpoints should not be randomized, rather defined by the prefab
            _selection.Prefab.StaticSegmentData.ConnectionPoints.Randomize();
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