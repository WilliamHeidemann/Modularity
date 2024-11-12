using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<GameObject> _gameObjects = new();

        public void UpdateFlow()
        {
            foreach (var segment in _structure.GraphData.Where(segment => segment.StaticSegmentData.Power > 0))
            {
                BestFirstFlow(segment);
            }
        }

        public void AddToGameObjects(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
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
                foreach (var segment in _structure.GetLinks(k))
                {
                    ActivateSegment(segment, segmentData, k, flow);

                    if (!(explored.Keys.Contains(segment) &&
                          explored[segment] >= flow - segment.StaticSegmentData.Resistance))
                    {
                        queue[segment] = flow - segment.StaticSegmentData.Resistance;
                        explored[segment] = flow - segment.StaticSegmentData.Resistance;
                    }
                }
            }
        }
        
        private void ActivateSegment(SegmentData segmentData, SegmentData source, SegmentData from, int flow)
        {

            var segment = GetSegmentAtPos(segmentData.Position);
            
            var power = flow - segmentData.StaticSegmentData.Resistance;

            //here in order to accomodate only activating under certain conditions some data-points are passed to the activator
            //more could be given if nescesary e.g. turn number given by list count and other stuff if needed for more complex activations
            segment.SegmentActivator.Activate(source.Position, from.Position);
            
        }
        private Segment GetSegmentAtPos(Vector3Int position) 
            => _gameObjects.First(go => go.transform.position.AsVector3Int() == position).GetComponent<Segment>();
        public void Clear()
        {
            _gameObjects.Clear();
        }


    }
    
}