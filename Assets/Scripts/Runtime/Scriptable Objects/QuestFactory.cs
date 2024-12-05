using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestFactory : ScriptableObject
    {
        [SerializeField] private AutoSpawner _autoSpawner;
        
        [Header("Quests")]
        [SerializeField] private SegmentQuest _placeFirstHeartSegment;
        [SerializeField] private SegmentQuest _placeFirstSteamSegment;
        [SerializeField] private ReceiverQuest _activateXReceivers;
        [SerializeField] private Quest _connectSteamAndFlesh;
        [SerializeField] private Quest<int> _collectX;
        [SerializeField] private ReceiverQuest _activateXReceiversSimultaneously;
        

        public SegmentQuest PlaceFirstBloodSegmentQuest()
        {
            var quest = _placeFirstHeartSegment.Build(2) as SegmentQuest;
            OnSegmentPlaced += quest!.Progress;
            quest.OnComplete += () => OnSegmentPlaced -= quest.Progress;
            return quest;
        }
        
        public SegmentQuest PlaceFirstSteamSegmentQuest()
        {
            var quest = _placeFirstSteamSegment.Build(2) as SegmentQuest;
            OnSegmentPlaced += quest!.Progress;
            quest.OnComplete += () => OnSegmentPlaced -= quest.Progress;
            return quest;
        }

        public ReceiverQuest ActivateXReceiversQuest(int x)
        {
            var quest = _activateXReceivers.Build(x) as ReceiverQuest;
            OnReceiversActivated += quest!.Progress;
            quest.OnComplete += () => OnReceiversActivated -= quest.Progress;
            return quest;
        }
        
        public Quest ConnectSteamAndFleshQuest()
        {
            var quest = _connectSteamAndFlesh.Build();
            OnBloodAndSteamConnected += quest.Complete;
            quest.OnComplete += () => OnBloodAndSteamConnected -= quest.Complete;
            return quest;
        }

        public Quest ConnectAllSteamAndFleshQuest()
        {
            var quest = _connectSteamAndFlesh.Build();
            OnBloodAndSteamConnected += quest.Complete;
            quest.OnComplete += () => OnBloodAndSteamConnected -= quest.Complete;
            _autoSpawner.SpawnBloodSource();
            _autoSpawner.SpawnSteamSource();
            return quest;
        }
        
        public Quest<int> CollectXQuest(int x)
        {
            var quest = _collectX.Build(x);
            OnCollect += quest.Progress;
            quest.OnComplete += () => OnCollect -= quest.Progress;
            quest.OnComplete += _autoSpawner.SpawnBloodSource;
            quest.OnComplete += _autoSpawner.SpawnSteamSource;
            for (int i = 0; i < x; i++)
            {
                _autoSpawner.SpawnCollectable();
            }
            return quest;
        }

        public ReceiverQuest ActivateXReceiversSimultaneouslyQuest(int x)
        {
            var quest = _activateXReceiversSimultaneously.Build(x) as ReceiverQuest;
            OnReceiversActivated += quest!.Progress;
            quest.OnComplete += () => OnReceiversActivated -= quest.Progress;
            return quest;
        }

        public void SegmentPlaced(SegmentData segmentData) => OnSegmentPlaced?.Invoke(segmentData);
        public void CollectableCollected(int x) => OnCollect?.Invoke(x);
        public void ReceiversActivated(IEnumerable<SegmentData> receivers) => OnReceiversActivated?.Invoke(receivers);
        public void BloodAndSteamConnected() => OnBloodAndSteamConnected?.Invoke();
        private event Action<SegmentData> OnSegmentPlaced;
        private event Action<int> OnCollect;
        private event Action<IEnumerable<SegmentData>> OnReceiversActivated;
        private event Action OnBloodAndSteamConnected;
    }
}