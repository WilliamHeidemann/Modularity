using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "ScoreTracker", menuName = "ScoreTracker")]
    public class ScoreTracker : ScriptableObject
    {
        [SerializeField] private Currency _currency;
        [SerializeField] private Structure _structure;
        [SerializeField] private int _highScore;

        [Header("Score Value")] [SerializeField]
        private int _heartsConnectedValue;

        [SerializeField] private int _furnacesConnectedValue;
        [SerializeField] private int _brainsActivatedValue;
        [SerializeField] private int _enginesActivatedValue;
        [SerializeField] private int _hybridsActivatedValue;
        [SerializeField] private int _energySpheresCollectedValue;
        [SerializeField] private int _pipeValue;
        [SerializeField] private int _connectedReceiverValue;
        [SerializeField] private int _currencyValue;


        [Header("Things accumulated")] [SerializeField]
        private int _heartsConnected;

        [SerializeField] private int _furnacesConnected;
        [SerializeField] private int _brainsActivated;
        [SerializeField] private int _enginesActivated;
        [SerializeField] private int _hybridsActivated;
        [SerializeField] private int _energySpheresCollected;
        private string _highScoreSavePath;

        private void OnEnable()
        {
            FlowControl.OnProducerActivated += ProducerActivation;
            FlowControl.OnSourcesLinkedCheck += UpdateSourceScore;
            AutoSpawner.OnCollectedCollectables += Collected;
            _highScoreSavePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Saves" +
                                 Path.AltDirectorySeparatorChar;
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
            LoadHighScore();
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

        private int CalculateScore()
        {
            var sum = 0;
            sum += CalculateAmalgamationScore();
            sum += _brainsActivated * _brainsActivatedValue;
            sum += _enginesActivated * _enginesActivatedValue;
            sum += _hybridsActivated * _hybridsActivatedValue;
            sum += _energySpheresCollected * _energySpheresCollectedValue;
            return sum;
        }

        private int CalculateFinalScore()
        {
            var sum = 0;
            sum += CalculateAmalgamationScore();
            sum += _brainsActivated * _brainsActivatedValue;
            sum += _enginesActivated * _enginesActivatedValue;
            sum += _hybridsActivated * _hybridsActivatedValue;
            sum += _energySpheresCollected * _energySpheresCollectedValue;
            sum += _currency.BloodAmount * _currencyValue;
            sum += _currency.SteamAmount * _currencyValue;
            sum += _currency.SteamAmount * _currency.BloodAmount;
            return sum;
        }

        private int CalculateAmalgamationScore()
        {
            var sum = 0;
            foreach (var segment in GetLargestConnectedAmalgamation())
            {
                if (segment.StaticSegmentData.IsReceiver) sum += _connectedReceiverValue;
                else if (segment.StaticSegmentData.IsSource && segment.StaticSegmentData.IsBlood)
                    sum += _heartsConnectedValue;
                else if (segment.StaticSegmentData.IsSource && segment.StaticSegmentData.IsSteam)
                    sum += _furnacesConnectedValue;
                else sum += _pipeValue;
            }

            return sum;
        }

        private List<SegmentData> GetLargestConnectedAmalgamation()
        {
            List<SegmentData> largestAmalgamation = new();
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

        private void SaveHighScore(int score)
        {
            string json = score.ToString();
            if (!Directory.Exists(_highScoreSavePath)) Directory.CreateDirectory(_highScoreSavePath);
            using StreamWriter writer = new StreamWriter(_highScoreSavePath + "SavedScore.json");
            writer.Write(json);
        }

        private void LoadHighScore()
        {
            string json = string.Empty;

            if (!File.Exists(_highScoreSavePath + "SavedScore.json")) _highScore = 0;
            else
            {
                using (StreamReader reader = new StreamReader(_highScoreSavePath + "SavedScore.json"))
                {
                    json = reader.ReadToEnd();
                }

                _highScore = int.Parse(json);
            }
        }

        private void ResetHighScore()
        {
            if (File.Exists(_highScoreSavePath + "SavedScore.json"))
            {
                File.Delete(_highScoreSavePath + "SavedScore.json");
            }

            LoadHighScore();
        }

        public int GetScore() => CalculateScore(); 
        public int GetFinalScore() => CalculateFinalScore(); 
        public int GetHighScore()
        {
            var score = CalculateFinalScore();
            if (score > _highScore) 
            {
                SaveHighScore(score);
                LoadHighScore();    
            }
            return _highScore;
        } 

        public int hearthConnections => _heartsConnected;
        public int furnaceConnections => _furnacesConnected;
        public int brainActivations => _brainsActivated;
        public int engineActivations => _enginesActivated;
        public int hybridActivations => _hybridsActivated;
        public int energySpheresCollected => _energySpheresCollected;
    }
}