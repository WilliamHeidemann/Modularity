using System.Linq;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Builder : ScriptableObject
    {
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        [SerializeField] private Currency _currency;
        [SerializeField] private Hand _hand;


        public void Build(Vector3Int position, Quaternion placeholderRotation, bool isInitial = false)
        {
            SpawnSelection(position, placeholderRotation, isInitial);
        }

        private void SpawnSelection(Vector3 position, Quaternion rotation, bool isInitial)
        {
            if (!_structure.IsOpenPosition(position.AsVector3Int()))
            {
                return;
            }

            if (!_currency.HasAtLeast(_selection.Price))
            {
                return;
            }

            if (!_selection.Prefab.IsSome(out var prefab))
            {
                return;
            }

            var segmentData = new SegmentData
            {
                Position = position.AsVector3Int(),
                Rotation = rotation,
                StaticSegmentData = prefab.StaticSegmentData,
            };

            if (!_structure.IsEmpty && !_structure.ConnectsToSomething(segmentData) && !isInitial)
            {
                return;
            }
            // potentially remove old slot

            var connector = Instantiate(prefab, position, rotation);
            segmentData.GetConnectionPoints()
                .ForEach(connectionPoint => SpawnSlot(position.AsVector3Int(), connectionPoint));
            _structure.AddSegment(segmentData);
            if (!isInitial) _currency.Pay(_selection.Price);
            _hand.GenerateHand();
            if (!isInitial) _selection.Prefab = Option<Segment>.None;
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