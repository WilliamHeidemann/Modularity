using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.DataLayer;
using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class FlowControl : ScriptableObject
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private CurrencyPopup _currencyPopup;
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private ParticleManager _particleManager;
        private readonly List<SegmentData> _receiversActivatedLast = new();

        public delegate void ProducerActivated(StaticSegmentData staticSegmentData);
        public static event ProducerActivated OnProducerActivated;
        public delegate void SourcesLinked(HashSet<SegmentData> sources);
        public static event SourcesLinked OnSourcesLinkedCheck;

        public void UpdateFlow()
        {
            _receiversActivatedLast.Clear();
            foreach (var receiver in _structure.Receivers.Where(receiver => !receiver.IsActivated))
            {
                CheckForActivation(receiver);
            }

            if (_receiversActivatedLast.Any())
            {
                _questFactory.ReceiversActivated(_receiversActivatedLast);
            }

            if (!_structure.Sources.Any()) return;
            if (AllSourcesLinked(_structure.Sources.First()))
            {
                _questFactory.BloodAndSteamConnected();
            }
        }

        private void CheckForActivation(SegmentData receiver)
        {
            if (_structure.GetValidConnections(receiver).Count() != receiver.GetConnectionPoints().Count())
            {
                return;
            }

            if (_structure.GetValidConnections(receiver).Any(connector =>
                    !(connector.StaticSegmentData.IsSource || IsConnectedToSource(connector, receiver))))
            {
                return;
            }

            ActivateSegment(receiver);
            OnProducerActivated?.Invoke(receiver.StaticSegmentData);
        }

        private bool IsConnectedToSource(SegmentData segment, SegmentData receiver)
        {
            Queue<SegmentData> queue = new();
            queue.Enqueue(segment);

            HashSet<SegmentData> explored = new() { segment, receiver };

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var link in _structure.GetValidConnections(current))
                {
                    // if (link.StaticSegmentData.IsSource)
                    // {
                    //     return true;
                    // }
                    if (link == receiver && current != segment)
                    {
                        return true;
                    }

                    if (!explored.Contains(link))
                    {
                        queue.Enqueue(link);
                        explored.Add(link);
                    }
                }
            }

            return false;
        }


        private bool AllSourcesLinked(SegmentData segment)
        {
            Queue<SegmentData> queue = new();
            queue.Enqueue(segment);

            HashSet<SegmentData> explored = new() { segment };
            HashSet<SegmentData> sources = new() { segment };

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var link in _structure.GetValidConnections(current))
                {
                    if (link.StaticSegmentData.IsSource)
                    {
                        sources.Add(link);
                    }

                    if (!explored.Contains(link))
                    {
                        queue.Enqueue(link);
                        explored.Add(link);
                    }
                }
            }

            OnSourcesLinkedCheck?.Invoke(sources);
            return sources.Count == _structure.Sources.Count();
        }

        private void ActivateSegment(SegmentData segmentToActivate)
        {
            if (segmentToActivate.IsActivated)
            {
                return;
            }

            _receiversActivatedLast.Add(segmentToActivate);
            segmentToActivate.IsActivated = true;
            _currencyPopup.GainCurrency(segmentToActivate.Position, segmentToActivate.StaticSegmentData);

            ParticleType particleType = segmentToActivate.StaticSegmentData switch
            {
                _ when segmentToActivate.StaticSegmentData.IsBlood && segmentToActivate.StaticSegmentData.IsSteam => ParticleType.HybridBurst,
                _ when segmentToActivate.StaticSegmentData.IsBlood => ParticleType.FleshBurst,
                _ when segmentToActivate.StaticSegmentData.IsSteam => ParticleType.GearBurst,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _particleManager.SpawnParticleFX(particleType, segmentToActivate.Position, Quaternion.identity, false);
        }
    }
}