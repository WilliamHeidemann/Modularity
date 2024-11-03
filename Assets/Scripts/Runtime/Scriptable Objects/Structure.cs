using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using Runtime.Components.Utility;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class Structure : ScriptableObject
    {
        [SerializeField] private List<SegmentData> _graphData = new();
        [SerializeField] private Currency _currency;

        public void AddSegment(SegmentData segmentData)
        {
            _graphData.Add(segmentData);
            UpdateFlow();
        }

        public bool ConnectsToSomething(SegmentData segmentData)
        {
            return _graphData.Any(data => CanConnect(segmentData, data));
        }

        private bool CanConnect(SegmentData segmentData1, SegmentData segmentData2)
        {
            var from1To2 = segmentData1.GetConnectionPoints().Contains(segmentData2.Position);
            var from2To1 = segmentData2.GetConnectionPoints().Contains(segmentData1.Position);
            var steamFlow = segmentData1.StaticSegmentData.Steam && segmentData2.StaticSegmentData.Steam;
            var bloodFlow = segmentData1.StaticSegmentData.Blood && segmentData2.StaticSegmentData.Blood;
            return from1To2 && from2To1 && (steamFlow || bloodFlow);
        }

        private IEnumerable<SegmentData> GetLinks(SegmentData segmentData)
        {
            var connectionsOneWay = _graphData.Where(data => segmentData.GetConnectionPoints().Contains(data.Position));
            return connectionsOneWay.Where(data => CanConnect(data, segmentData));
        }

        public bool IsEmpty => _graphData.Count == 0;
        public bool IsOpenPosition(Vector3Int position) => _graphData.All(data => data.Position != position);

        public void Clear()
        {
            _graphData.Clear();
        }

        private void UpdateFlow()
        {
            foreach (var segment in _graphData.Where(segment => segment.StaticSegmentData.Power > 0))
            {
                BestFirstFlow(segment);
            }
        }

        private void BestFirstFlow(SegmentData segmentData)
        {
            Dictionary<SegmentData, int> queue = new()
            {
                [segmentData] = segmentData.StaticSegmentData.Power
            };

            Dictionary<SegmentData, int> explored = new()
            {
                [segmentData] = segmentData.StaticSegmentData.Power
            };

            while (queue.Any())
            {
                var k = queue.Keys.First();
                int flow = queue[k];
                queue.Remove(k);

                if (flow <= 1) continue;
                foreach (var segment in GetLinks(k))
                {
                    if (!segment.isActive) ActivateSegment(segment, flow);

                    if (!(explored.Keys.Contains(segment) &&
                          explored[segment] >= flow - segment.StaticSegmentData.Resistance))
                    {
                        queue[segment] = flow - segment.StaticSegmentData.Resistance;
                        explored[segment] = flow - segment.StaticSegmentData.Resistance;
                    }
                }
            }
        }

        public void ActivateSegment(SegmentData segmentData, int flow)
        {
            segmentData.isActive = true;
            var power = flow - segmentData.StaticSegmentData.Resistance;
            if (segmentData.StaticSegmentData.Reward > 0)
            {
                _currency.Add(segmentData.StaticSegmentData.Reward);
            }
        }
    }
}