using System.Collections.Generic;
using System.Linq;
using Runtime.Components.Segments;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "ScoreTracker", menuName = "ScoreTracker")]
    public class ScoreTracker : ScriptableObject
    {
        [SerializeField] private Currency _currency;
        [SerializeField] private Structure _structure;

        [Header("Score Value")]
        [SerializeField] private int _heartsConnectedValue;
        [SerializeField] private int _furnacesConnectedValue;
        [SerializeField] private int _brainsActivatedValue;
        [SerializeField] private int _enginesActivatedValue;
        [SerializeField] private int _hybridsActivatedValue;
        [SerializeField] private int _energySpheresCollectedValue;
        [SerializeField] private int _pipeValue;
        [SerializeField] private int _connectedReceiverValue;
        [SerializeField] private int _currencyValue;


        [Header("Things accumulated")] 
        [SerializeField] private int _heartsConnected;
        [SerializeField] private int _furnacesConnected;
        [SerializeField] private int _brainsActivated;
        [SerializeField] private int _enginesActivated;
        [SerializeField] private int _hybridsActivated;
        [SerializeField] private int _energySpheresCollected;

        private void OnEnable()
        {
            FlowControl.OnProducerActivated += ProducerActivation;
            FlowControl.OnSourcesLinkedCheck += UpdateSourceScore;
            AutoSpawner.OnCollectedCollectables += Collected;
        }

        private void OnDisable()
        {
            FlowControl.OnProducerActivated -= ProducerActivation;
            FlowControl.OnSourcesLinkedCheck -= UpdateSourceScore;
            AutoSpawner.OnCollectedCollectables -= Collected;
        }
public void Clear()
        {
            _heartsConnected = 0;
            _furnacesConnected = 0;
            _brainsActivated = 0;
            _hybridsActivated = 0;
            _enginesActivated = 0;
            _energySpheresCollected = 0;
        }
        private void ProducerActivation(StaticSegmentData staticSegmentData)
        {
            if (staticSegmentData.IsBlood && staticSegmentData.IsSteam)
            {
                _hybridsActivated += 1;
            }
            else if (staticSegmentData.IsBlood)
            {
                _brainsActivated += 1;
            }
            else _enginesActivated += 1;
        }

        private void UpdateSourceScore(HashSet<SegmentData> sources)
        {
            var blood = 0;
            var steam = 0;
            foreach (var source in sources)
            {
                if (source.StaticSegmentData.IsBlood)
                {
                    blood += 1;
                }
                else steam += 1;
            }

            _heartsConnected = blood;
            _furnacesConnected = steam;
        }

        private void Collected(int noCollected)
        {
            _energySpheresCollected += noCollected;
        }
        
        public void CheckHand(List<Segment> segments, int rerollCostBlood, int rerollCostSteam)
        {
            if (CanAffordCard(segments, rerollCostBlood, rerollCostSteam)) return;
            //IF the hand cannot be afforded put end game call here
            var score = CalculateScore();
            Debug.Log(score);
        }
        private bool CanAffordCard(List<Segment> segments, int rerollCostBlood, int rerollCostSteam)
        {      
            if (_currency.BloodAmount > rerollCostBlood && _currency.SteamAmount > rerollCostSteam) return true;
            foreach (var segment in segments)
            {
                if (segment.StaticSegmentData.BloodCost > 0 && segment.StaticSegmentData.SteamCost > 0)
                {
                    if(_currency.BloodAmount - segment.StaticSegmentData.BloodCost > -1 && _currency.SteamAmount - segment.StaticSegmentData.SteamCost > -1) return true;
                }
                else if (segment.StaticSegmentData.BloodCost > 0 && _currency.BloodAmount - segment.StaticSegmentData.BloodCost > -1) return true;
                else if (segment.StaticSegmentData.SteamCost > 0 && _currency.SteamAmount - segment.StaticSegmentData.SteamCost > -1) return true;
            }
            return false;
        }
        private int CalculateScore()
        {
            var sum = 0;
            sum += CalculateAmalgamationScore();
            sum += _brainsActivated * _brainsActivatedValue;
            sum += _enginesActivated * _enginesActivatedValue;
            sum += _hybridsActivated * _hybridsActivatedValue;
            sum += _energySpheresCollected * _energySpheresCollectedValue;
            sum += _currency.BloodAmount * _currencyValue;
            sum += _currency.SteamAmount * _currencyValue;
            return sum;
        }
        private int CalculateAmalgamationScore()
        {
            var sum = 0;
            foreach (var segment in GetLargestConnectedAmalgamation())
            {
                if (segment.StaticSegmentData.IsReceiver) sum += _connectedReceiverValue;
                else if (segment.StaticSegmentData.IsSource && segment.StaticSegmentData.IsBlood) sum += _heartsConnectedValue;
                else if (segment.StaticSegmentData.IsSource && segment.StaticSegmentData.IsSteam) sum += _furnacesConnectedValue;
                else sum += _pipeValue;
            }
            return sum;
        }
        private List<SegmentData> GetLargestConnectedAmalgamation()
        {
            List<SegmentData> largestAmalgamation = new ();
            foreach (var segment in _structure.Segments)
            {
                if (largestAmalgamation.Contains(segment)) break;
                var tempList = GetConnectedSegments(segment);
                if (tempList.Count() < largestAmalgamation.Count()) break;
                largestAmalgamation = tempList;
            }
            return largestAmalgamation;
        }
        private List<SegmentData> GetConnectedSegments(SegmentData segment)
        {
            
            Queue<SegmentData> queue = new();
            queue.Enqueue(segment);

            List<SegmentData> connectedSegments = new() { segment };

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var link in _structure.GetValidConnections(current))
                {
                    
                    if (!connectedSegments.Contains(link))
                    {
                        queue.Enqueue(link);
                        connectedSegments.Add(link);
                    }
                }
            }
            return connectedSegments;
        }
    }
}