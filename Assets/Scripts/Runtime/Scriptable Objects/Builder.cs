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
        [SerializeField] private Currency _currency;
        [SerializeField] private Hand _hand;


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

            if (!_currency.HasAtLeast(_selection.Price))
            {
                return;
            }
            
            var segmentData = new SegmentData
            {
                Position = position.AsVector3Int(),
                Rotation = rotation,
                StaticSegmentData = _selection.Prefab.StaticSegmentData,
            };
            
            segmentData.GetConnectionPoints().ForEach(i => Debug.Log(i));
            
            if (!_structure.IsEmpty && !_structure.ConnectsToSomething(segmentData))
            {
                Debug.Log("Cannot connect to anything");
                return;
            }
            // potentially remove old slot
            
            var connector = Instantiate(_selection.Prefab, position, rotation);
            segmentData.GetConnectionPoints().ForEach(connectionPoint => SpawnSlot(position.AsVector3Int(), connectionPoint));
            _structure.AddSegment(segmentData);
            _currency.Pay(_selection.Price);
            _hand.GenerateHand();
        }

        private void SpawnSlot(Vector3 segmentPosition, Vector3 slotPosition)
        {
            if (!_structure.IsOpenPosition(slotPosition.AsVector3Int()))
            {
                return;
            }

            var spawnPosition = (segmentPosition + slotPosition) / 2f;
            var slot = Instantiate(_slotPrefab, spawnPosition, Quaternion.identity);
            slot.Position = slotPosition.AsVector3Int();
        }
    }
}