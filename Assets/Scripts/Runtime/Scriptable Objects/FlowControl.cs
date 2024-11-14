using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runtime.Components;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class FlowControl : ScriptableObject
    {
        [SerializeField] private Structure _structure;
        [SerializeField] private List<Segment> _segments = new();

        public void AddSegment(Segment segment) => _segments.Add(segment);
        public void Clear() => _segments.Clear();
        public void UpdateFlow()
        {
            var everythingConnectedToASource = new HashSet<SegmentData>();
            
            foreach (var source in _structure.Sources)
            {
                var reachableSegments = ReachableSegments(source);
                everythingConnectedToASource.UnionWith(reachableSegments);
            }

            foreach (var receiver in _structure.Receivers)
            {
                CheckForActivation(receiver);
            }
        }
        private void CheckForActivation(SegmentData receiver)
        {
            if (_structure.GetLinks(receiver).Count() != receiver.GetConnectionPoints().Count()) return;
            foreach (var connector in _structure.GetLinks(receiver))
            {
                if (!IsConnectedToSource(connector, receiver)) return;
            }
            ActivateSegment(receiver);

        }
        private bool IsConnectedToSource(SegmentData segment, SegmentData notThrough)
        {
            Queue<SegmentData> queue = new();
            queue.Enqueue(segment);

            HashSet<SegmentData> explored = new() { segment, notThrough };

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var link in _structure.GetLinks(current))
                {
                    if (link.StaticSegmentData.IsSource) return true;
                    
                    if (!explored.Contains(link))
                    {
                        queue.Enqueue(link);
                        explored.Add(link);
                    }
                }
            }

            return false;
        }

        private HashSet<SegmentData> ReachableSegments(SegmentData source)
        {
            Queue<SegmentData> queue = new();
            queue.Enqueue(source);

            HashSet<SegmentData> explored = new() { source };

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var link in _structure
                             .GetLinks(current))
                {
                    if (!explored.Contains(link))
                    {
                        queue.Enqueue(link);
                        explored.Add(link);
                    }
                }
            }

            return explored;
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