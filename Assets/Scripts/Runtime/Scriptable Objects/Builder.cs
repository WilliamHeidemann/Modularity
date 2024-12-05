using System.Linq;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;
using static Runtime.Scriptable_Objects.AutoSpawner;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Builder : ScriptableObject
    {
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Selection _selection;
        [SerializeField] private Structure _structure;
        [SerializeField] private Currency _currency;
        [SerializeField] private CurrencyPopup _currencyPopup;
        [SerializeField] private Hand _hand;
        [SerializeField] private FlowControl _flowControl;
        [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private QuestFactory _questFactory;

        public delegate void SegmentPlaced();
        public static event SegmentPlaced OnSegmentPlaced;


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

            if (!_currency.HasAtLeast(_selection.PriceBlood, _selection.PriceSteam))
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
            
            if (!_structure.IsValidPlacement(segmentData) && !isInitial)
            {
                return;
            }

            // potentially remove old slot

            var connector = Instantiate(prefab, position, rotation);
            segmentData.GetConnectionPoints()
                .ForEach(connectionPoint => SpawnSlot(position.AsVector3Int(), connectionPoint));
            _structure.AddSegment(segmentData);
            SoundFXPlayer.Instance.Play(segmentData.StaticSegmentData.SoundFX);
            OnSegmentPlaced?.Invoke();

            if (isInitial)
            {
                return;
            }
            
            _questFactory.SegmentPlaced(segmentData);
            _flowControl.UpdateFlow();
            _autoSpawner.CheckForCollectables();
            _currency.Pay(_selection.PriceBlood, _selection.PriceSteam);
            _currencyPopup.SpendCurrency(position.AsVector3Int(), segmentData.StaticSegmentData);
            _hand.DrawHand();
            _selection.Prefab = Option<Segment>.None;
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