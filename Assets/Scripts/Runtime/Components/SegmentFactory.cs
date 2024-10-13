using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Runtime.Components
{
    public class SegmentFactory : MonoSingleton<SegmentFactory>
    {
        [SerializeField] private Structure _structure;
        private Option<Card> _toBuild = Option<Card>.None;

        [Header("Prefabs")]
        [SerializeField] private MonoSegment _connectorBoxPrefab;
        [SerializeField] private MonoSegment _cogsPrefab;
        [SerializeField] private MonoSegment _pipesPrefab;
        [SerializeField] private MonoSegment _heartPrefab;
        [SerializeField] private MonoSegment _eyesPrefab;
        [SerializeField] private MonoSegment _tentaclePrefab;
        [SerializeField] private MonoSegment _wingsPrefab;
        [SerializeField] private ConnectionSlot _connectionSlotPrefab;

        [Header("Resource Texts")] 
        [SerializeField] private TextMeshProUGUI _bloodText;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private TextMeshProUGUI _mechanicalText;
        
        private void Start()
        {
            _structure = new Structure(_bloodText, _energyText, _mechanicalText);
            PlaceSegment(Segment.StartingSegment);
        }

        public void Select(Option<Card> card)
        {
            _toBuild = card;
        }
        
        public void TryBuild(Position position)
        {
            if (!_toBuild.IsSome(out var card))
            {
                print("No card selected.");
                return;
            }
            
            var segment = new Segment(position, card);
            if (!_structure.HasResources(segment.Data))
            {
                print("Not enough resources.");
                return;
            }

            PlaceSegment(segment);
            _toBuild = Option<Card>.None;
            CardsManager.Instance.SpendCard(card);
        }
        
        private void PlaceSegment(Segment segment)
        {
            var prefab = segment.Model switch
            {
                Model.ConnectorBox => _connectorBoxPrefab,
                Model.ConnectionSlot => _connectionSlotPrefab,
                Model.Cogs => _cogsPrefab,
                Model.Pipes => _pipesPrefab,
                Model.Heart => _heartPrefab,
                Model.Eyes => _eyesPrefab,
                Model.Tentacle => _tentaclePrefab,
                Model.Wings => _wingsPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var monoSegment = Instantiate(prefab, segment.Position.AsVector3, Quaternion.identity);
            monoSegment.Segment = segment;
            _structure.AddSegment(segment);
        }

        public void SpawnPlaceholders(IEnumerable<Position> positions)
        {
            foreach (var position in positions.Where(_structure.IsAvailable))
            {
                var segment = new Segment(position, new Card(Model.ConnectionSlot));
                PlaceSegment(segment);
            }
        }
    }
}