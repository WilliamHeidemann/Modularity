using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Models;
using Runtime.Scriptable_Objects;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class StructureManager : MonoBehaviour
    {
        [SerializeField] private Structure _startingConfiguration;
        [SerializeField] private Structure _structure;
        [SerializeField] private Card _slot;
        [SerializeField] private SegmentFactory _segmentFactory;
        private Option<Card> _selectedCard = Option<Card>.None;
        
        private void Start()
        {
            MonoCard.OnCardSelect += Select;
            ConnectionSlot.OnSlotClicked += TryBuild;
            Connectable.OnSpawnSlots += AddSlots;
            CardManager.OnHandReplaced += Deselect;
            
            _structure.Clear();
            foreach (var segment in _startingConfiguration.AllSegments())
            {
                Build(segment);
            }
        }

        private void OnDisable()
        {
            MonoCard.OnCardSelect -= Select;
            ConnectionSlot.OnSlotClicked -= TryBuild;
            Connectable.OnSpawnSlots -= AddSlots;
            CardManager.OnHandReplaced -= Deselect;
        }
        
        public void Select(Card card) => _selectedCard = Option<Card>.Some(card);
        public void Deselect() => _selectedCard = Option<Card>.None;

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

            Build(segment);
            Deselect();
            CardManager.Instance.SpendCard(card);
        }

        private void Build(Segment segment)
        {
            _structure.AddSegment(segment);
            _segmentFactory.SpawnSegment(segment);
        }
        
        private void AddSlots(IEnumerable<Position> positions)
        {
            foreach (var position in positions.Where(_structure.IsAvailable))
            {
                var segment = new Segment(position, _slot);
                Build(segment);
            }
        }
    }
}