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
                    if (!segment.IsActive) ActivateSegment(segment, flow);

                    if (!(explored.Keys.Contains(segment) &&
                          explored[segment] >= flow - segment.StaticSegmentData.Resistance))
                    {
                        queue[segment] = flow - segment.StaticSegmentData.Resistance;
                        explored[segment] = flow - segment.StaticSegmentData.Resistance;
                    }
                }
            }
        }
        
        private void ActivateSegment(SegmentData segmentData, int flow)
        {

            var segment = GetSegmentAtPos(segmentData.Position);
            Debug.Log(segment.name);
            segmentData.IsActive = true;
            segment.GetComponent<Segment>();
            var power = flow - segmentData.StaticSegmentData.Resistance;
            if (segmentData.StaticSegmentData.isReciever)
            {
                //_currency.Add(segmentData.StaticSegmentData.BloodReward, segmentData.StaticSegmentData.SteamReward);
            }
        }

        private GameObject GetSegmentAtPos(Vector3Int position)
        {
            GameObject result = new();
            foreach (var segment in _gameObjects)
            {
                if (segment.transform.position.AsVector3Int() == position)
                {
                    result = segment;
                    break;
                }
            }
            return result;
        }
        public void Clear()
        {
            _gameObjects.Clear();
        }


    }
    
}