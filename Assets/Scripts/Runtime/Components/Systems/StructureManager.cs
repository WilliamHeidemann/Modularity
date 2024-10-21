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
        [SerializeField] private SegmentFactory _segmentFactory;
        private Option<Model> _selectedCard = Option<Model>.None;
        private static readonly HashSet<Position> Slots = new();

        private void Start()
        {
            MonoCard.OnCardSelect += Select;
            Slot.OnSlotClicked += TryBuild;
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
            Slot.OnSlotClicked -= TryBuild;
            Connectable.OnSpawnSlots -= AddSlots;
            CardManager.OnHandReplaced -= Deselect;
        }
        
        public void Select(Model model) => _selectedCard = Option<Model>.Some(model);
        public void Deselect() => _selectedCard = Option<Model>.None;

        public void TryBuild(Position position)
        {
            if (!_selectedCard.IsSome(out var model))
            {
                print("No card selected.");
                return;
            }

            var segment = new Segment(position, model);
            Build(segment);
            Deselect();
            CardManager.Instance.SpendCard(model);
        }

        private void Build(Segment segment)
        {
            _structure.AddSegment(segment);
            _segmentFactory.SpawnSegment(segment);
        }
        
        private void AddSlots(IEnumerable<Position> positions)
        {
            foreach (var position in positions.Where(p => _structure.IsAvailable(p) && !Slots.Contains(p)))
            {
                _segmentFactory.SpawnSlot(position);
                Slots.Add(position);
            }
        }
    }
}