using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class FlowControl : ScriptableObject
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private List<Segment> _segments = new();
        [SerializeField] private AutomaticSourceSpawning _sourceSpawner;


        public void AddSegment(Segment segment) => _segments.Add(segment);
        public void Clear() => _segments.Clear();

        public void UpdateFlow()
        {
            foreach (var receiver in _structure.Receivers)
            {
                CheckForActivation(receiver);
            }

            if (_structure.Sources.Any() && AllSourcesLnked(_structure.Sources.First()))
            {
                _sourceSpawner.SpawnRandomSource();
                _sourceSpawner.SpawnRandomSource();
            }
        }

        private void CheckForActivation(SegmentData receiver)
        {
            if (_structure.GetValidConnections(receiver).Count() != receiver.GetConnectionPoints().Count())
            {
                return;
            }

            if (_structure.GetValidConnections(receiver).Any(connector => !IsConnectedToSource(connector, receiver)))
            {
                return;
            }

            ActivateSegment(receiver);
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
                    if (link.StaticSegmentData.IsSource)
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


        private bool AllSourcesLnked(SegmentData segment)
        {
            Queue<SegmentData> queue = new();
            queue.Enqueue(segment);

            HashSet<SegmentData> explored = new() { segment};
            HashSet<SegmentData> sources = new() { segment};

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

            return sources.Count() == _structure.Sources.Count();
        }


        private void ActivateSegment(SegmentData segmentToActivate)
        {
            var segmentOption = GetSegmentAtPosition(segmentToActivate.Position);
            if (!segmentOption.IsSome(out var segment))
            {
                return;
            }

            segment.SegmentActivator.Activate();
        }

        private Option<Segment> GetSegmentAtPosition(Vector3Int position)
            => _segments.FirstOption(segment => segment.transform.position.AsVector3Int() == position);
    }
}