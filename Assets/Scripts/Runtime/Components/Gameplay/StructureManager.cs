using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Gameplay
{
    public class StructureManager : MonoBehaviour
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private CardScriptableObject _slot;
        private Option<CardScriptableObject> _selectedCard = Option<CardScriptableObject>.None;
        public static event Action<Segment> OnSegmentAdded;
        
        private void Start()
        {
            CardDisplay.OnCardSelect += Select;
            ConnectionSlot.OnSlotClicked += TryBuild;
            Connectable.OnSpawnSlots += AddSlots;
        }

        private void OnDisable()
        {
            CardDisplay.OnCardSelect -= Select;
            ConnectionSlot.OnSlotClicked -= TryBuild;
            Connectable.OnSpawnSlots -= AddSlots;
        }
        
        public void Select(CardScriptableObject card)
        {
            _selectedCard = Option<CardScriptableObject>.Some(card);
        }
        
        public void Deselect()
        {
            _selectedCard = Option<CardScriptableObject>.None;
        }
        
        public void TryBuild(Position position)
        {
            if (!_selectedCard.IsSome(out var card))
            {
                print("No card selected.");
                return;
            }

            var segment = new Segment(position, card);
            if (!_structure.HasResources(segment.Supply))
            {
                print("Not enough resources.");
                return;
            }

            _structure.AddSegment(segment);
            OnSegmentAdded?.Invoke(segment);
            _selectedCard = Option<CardScriptableObject>.None;
            CardsManager.Instance.SpendCard(card);
        }
        
        private void AddSlots(IEnumerable<Position> positions)
        {
            foreach (var position in positions.Where(_structure.IsAvailable))
            {
                var segment = new Segment(position, _slot);
                OnSegmentAdded?.Invoke(segment);
            }
        }
    }
}