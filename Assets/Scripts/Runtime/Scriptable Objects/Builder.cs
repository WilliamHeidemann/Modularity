using System.Linq;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using Runtime.DataLayer;
using Runtime.UnityCloud;
using Unity.Services.Analytics;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;
using static Runtime.Scriptable_Objects.AutoSpawner;
using Event = Unity.Services.Analytics.Event;

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
        [SerializeField] private SlotVisualizer _slotVisualizer;
        [SerializeField] private AccumulatedDataPoints _accumulatedDataPoints;
        [SerializeField] private QuestCompletedEventFactory _questEventFactory;

        public delegate void SegmentPlaced();
        public static event SegmentPlaced OnSegmentPlaced;


        public void Build(Vector3Int position, Quaternion placeholderRotation, bool isInitial = false)
        {
            SpawnSelection(position, placeholderRotation, isInitial);
        }
        
        public bool IsValidBuildAttempt(Vector3Int position, Quaternion rotation, out SegmentData segmentData, out Segment prefab) 
        {
            if (!_currency.HasAtLeast(_selection.PriceBlood, _selection.PriceSteam))
            {
                _accumulatedDataPoints.InsufficientResourcesBuildAttempts++;
                segmentData = null;
                prefab = null;
                return false;
            }

            if (!_selection.Prefab.IsSome(out prefab))
            {
                segmentData = null;
                return false;
            }

            segmentData = new SegmentData
            {
                Position = position,
                Rotation = rotation,
                StaticSegmentData = prefab.StaticSegmentData,
            };
            
            if (!_structure.IsValidPlacement(segmentData))
            {
                _accumulatedDataPoints.InvalidPlacementBuildAttempts++;
                return false;
            }
            
            return true;
        }

        public void BuildInstant(SegmentData segmentData, Segment prefab)
        {
            if (prefab.StaticSegmentData != segmentData.StaticSegmentData)
            {
                return;
            }
            
            Instantiate(prefab, segmentData.Position, segmentData.Rotation);
            segmentData.GetConnectionPoints()
                .ForEach(connectionPoint => SpawnSlot(segmentData.Position, connectionPoint));
            _structure.AddSegment(segmentData);
        }

        private void SpawnSelection(Vector3 position, Quaternion rotation, bool isInitial)
        {
            if (!IsValidBuildAttempt(position.AsVector3Int(), rotation, out SegmentData segmentData, out Segment prefab)
                && !isInitial)
            {
                return;
            }

            // potentially remove old slot

            var connector = Instantiate(prefab, position, rotation);
            segmentData.GetConnectionPoints()
                .ForEach(connectionPoint => SpawnSlot(position.AsVector3Int(), connectionPoint));
            _structure.AddSegment(segmentData);
            SoundFXPlayer.Instance.Play(segmentData.StaticSegmentData.SoundFX);
            _slotVisualizer.HideSlots();

            if (isInitial)
            {
                return;
            }

            _hand.DrawHand();
            _questFactory.SegmentPlaced(segmentData);
            _flowControl.UpdateFlow();
            _autoSpawner.CheckForCollectables();
            _currency.Pay(_selection.PriceBlood, _selection.PriceSteam);
            _currencyPopup.SpendCurrency(position.AsVector3Int(), segmentData.StaticSegmentData);
            OnSegmentPlaced?.Invoke();
            _selection.Prefab = Option<Segment>.None;
            _accumulatedDataPoints.SegmentsPlaced++;
            AnalyticsService.Instance.RecordEvent(_questEventFactory.CreateGameStateEvent());
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