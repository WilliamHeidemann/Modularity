using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.DataLayer;
using Runtime.UnityCloud;
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
        [SerializeField] private AccumulatedDataPoints _accumulatedDataPoints;

        public delegate void ProducerActivated(StaticSegmentData staticSegmentData);

        public static event ProducerActivated OnProducerActivated;

        public void UpdateFlow()
        {
            var receiversActivatedLast = new List<SegmentData>();
            receiversActivatedLast.Clear();
            foreach (var source in _structure.Sources.Where(source => !source.IsActivated))
            {
                if (HasPathToSelf(source))
                {
                    ActivateSegment(source);
                    OnProducerActivated?.Invoke(source.StaticSegmentData);
                    receiversActivatedLast.Add(source);
                }
            }

            foreach (var receiver in _structure.Receivers.Where(receiver => !receiver.IsActivated))
            {
                if (ShouldActivateReceiver(receiver))
                {
                    ActivateSegment(receiver);
                }
            }

            if (receiversActivatedLast.Any())
            {
                _questFactory.ReceiversActivated(receiversActivatedLast);
            }
        }

        private bool HasPathToSelf(SegmentData self)
        {
            Queue<(SegmentData current, HashSet<SegmentData> path)> queue = new();
            queue.Enqueue((self, new HashSet<SegmentData> { self }));

            while (queue.Any())
            {
                var (current, path) = queue.Dequeue();
                foreach (var link in _structure.GetValidConnections(current))
                {
                    if (link == self && path.Count > 2)
                    {
                        return true;
                    }

                    if (!path.Contains(link))
                    {
                        var path2 = new HashSet<SegmentData>(path)
                        {
                            link
                        };
                        queue.Enqueue((link, path2));
                    }
                }
            }

            return false;
        }

        private bool ShouldActivateReceiver(SegmentData receiver)
        {
            return _structure.GetValidConnectionsWithType(receiver).Count(tuple =>
                       IsReceiverConnectedToSource(tuple.segment, receiver, tuple.connectionType)) >=
                   receiver.StaticSegmentData.Requirements;
        }

        private bool IsReceiverConnectedToSource(SegmentData startNode, SegmentData receiver,
            ConnectionType connectionType)
        {
            Queue<SegmentData> queue = new();
            queue.Enqueue(startNode);

            HashSet<SegmentData> explored = new() { startNode, receiver };

            while (queue.Any())
            {
                var current = queue.Dequeue();
                
                if (current.StaticSegmentData.IsSource && current.IsActivated)
                {
                    if (current.StaticSegmentData.IsBlood && connectionType == ConnectionType.Blood)
                    {
                        return true;
                    }

                    if (current.StaticSegmentData.IsSteam && connectionType == ConnectionType.Steam)
                    {
                        return true;
                    }
                }

                foreach (var link in _structure.GetValidConnections(current))
                {
                    if (!explored.Contains(link))
                    {
                        queue.Enqueue(link);
                        explored.Add(link);
                    }
                }
            }

            return false;
        }

        private void ActivateSegment(SegmentData segmentToActivate)
        {
            if (segmentToActivate.IsActivated)
            {
                return;
            }

            segmentToActivate.IsActivated = true;
            _currencyPopup.GainCurrency(segmentToActivate.Position, segmentToActivate.StaticSegmentData);
            _accumulatedDataPoints.ResourcesCollectedFromSources += segmentToActivate.StaticSegmentData.BloodReward +
                segmentToActivate.StaticSegmentData.SteamReward;
            _accumulatedDataPoints.SegmentsActivated++;

            ParticleType particleType = segmentToActivate.StaticSegmentData switch
            {
                _ when segmentToActivate.StaticSegmentData.IsBlood && segmentToActivate.StaticSegmentData.IsSteam =>
                    ParticleType.HybridBurst,
                _ when segmentToActivate.StaticSegmentData.IsBlood => ParticleType.FleshBurst,
                _ when segmentToActivate.StaticSegmentData.IsSteam => ParticleType.GearBurst,
                _ => throw new ArgumentOutOfRangeException()
            };

            _particleManager.SpawnParticleFX(particleType, segmentToActivate.Position, Quaternion.identity, false);
        }
    }
}