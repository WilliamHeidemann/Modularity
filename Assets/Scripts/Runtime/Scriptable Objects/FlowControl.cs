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
            foreach (var receiver in _structure.Receivers)
            {
                CheckForActivation(receiver);
            }
        }

        private void CheckForActivation(SegmentData receiver)
        {
            if (_structure.GetLinks(receiver).Count() != receiver.GetConnectionPoints().Count())
            {
                return;
            }

            if (_structure.GetLinks(receiver).Any(connector => !IsConnectedToSource(connector, receiver)))
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
                foreach (var link in _structure.GetLinks(current))
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