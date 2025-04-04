using System;
using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;
using Runtime.Backend;
using Runtime.Components.Systems;
using Runtime.Components.Utility;
using Runtime.DataLayer;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        public event Action OnDrawHand;

        [SerializeField] private Selection _selection;
        [SerializeField] private SegmentPool _pool;
        [SerializeField] private SlotVisualizer _slotVisualizer;
        private readonly LinkedList<List<Segment>> _queuedHands = new();
        private bool _excludeBlood;
        private bool _excludeSteam;
        private bool _excludeReceivers;

        //the segments that the player can choose from
        public List<Segment> SegmentsOptions;

        private const int OptionsCount = 3;

        public void Clear()
        {
            _queuedHands.Clear();
        }
        
        public void ExcludeBlood() => _excludeBlood = true;
        public void IncludeBlood() => _excludeBlood = false;
        public void ExcludeSteam() => _excludeSteam = true;
        public void IncludeSteam() => _excludeSteam = false;
        public void ExcludeReceivers() => _excludeReceivers = true;
        public void IncludeReceivers() => _excludeReceivers = false;

        public void SelectBlueprint(int chosenSegment)
        {
            _selection.Prefab = Option<Segment>.Some(SegmentsOptions[chosenSegment]);
            _selection.PriceBlood = SegmentsOptions[chosenSegment].StaticSegmentData.BloodCost;
            _selection.PriceSteam = SegmentsOptions[chosenSegment].StaticSegmentData.SteamCost;
            
            _slotVisualizer.HideSlots();
            var isBlood = SegmentsOptions[chosenSegment].StaticSegmentData.IsBlood;
            var isSteam = SegmentsOptions[chosenSegment].StaticSegmentData.IsSteam;
            if (isBlood) _slotVisualizer.VisualizeSlots(ConnectionType.Blood);
            if (isSteam) _slotVisualizer.VisualizeSlots(ConnectionType.Steam);
            
            SoundFXPlayer.Instance.Play(SoundFX.CardSelection);
        }

        public void DrawHand()
        {
            if (_queuedHands.Count > 0)
            {
                var hand = _queuedHands.First.Value;
                _queuedHands.RemoveFirst();
                DrawQueuedHand(hand);
            }
            else
            {
                GenerateHand();
            }
        }

        private void GenerateHand()
        {
            SegmentsOptions = new List<Segment>();
            for (int i = 0; i < OptionsCount; i++)
            {
                var segment = SpawnUtility.Get(_pool.GetRandomSegment, IsValid);
                SegmentsOptions.Add(segment);
            }

            OnDrawHand?.Invoke();
        }

        public void DrawQueuedHand(List<Segment> segments)
        {
            if (segments.Count != 3 || segments.Any(segment => segment == null))
            {
                Debug.LogError("Hand must have 3 segments");
                return;
            }

            SegmentsOptions = segments;
            OnDrawHand?.Invoke();
        }

        public void QueueHandsLast(List<List<Segment>> hands)
        {
            if (hands.Any(hand => hand.Count != 3))
            {
                Debug.LogError("All hands must have exactly 3 segments");
                return;
            }

            foreach (var hand in hands)
            {
                _queuedHands.AddLast(hand);
            }
        }

        public void QueueHandFirst(List<Segment> hand)
        {
            _queuedHands.AddFirst(hand);
        }

        private bool IsValid(Segment segment)
        {
            if (SegmentsOptions.Contains(segment))
            {
                return false;
            }

            if (_excludeBlood && segment.StaticSegmentData.IsBlood)
            {
                return false;
            }
            
            if (_excludeSteam && segment.StaticSegmentData.IsSteam)
            {
                return false;
            }
            
            if (_excludeReceivers && segment.StaticSegmentData.IsReceiver)
            {
                return false;
            }

            return true;
        }
    }
}