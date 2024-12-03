using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestFactory : ScriptableObject
    {
        [SerializeField] private Quest _camera;
        [SerializeField] private SegmentQuest _placeFirstHeartSegment;
        [SerializeField] private SegmentQuest _placeFirstSteamSegment;
        [SerializeField] private Quest _rotateOneSegment;
        [SerializeField] private ReceiverQuest _activateXReceivers;
        [SerializeField] private Quest _connectSteamAndFlesh;
        [SerializeField] private Quest<int> _collectX;
        [SerializeField] private ReceiverQuest _activateXReceiversSimultaneously;
        [SerializeField] private ResourcesQuest _reachXBloodResources;
        [SerializeField] private ResourcesQuest _reachXSteamResources;
        
        public Quest CameraQuest()
        {
            var quest = _camera.Build();
            OnCameraCompleted += quest.Complete;
            quest.OnComplete += () => OnCameraCompleted -= quest.Complete;
            return quest;
        }

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
        
        

        public Quest RotateOneSegmentQuest()
        {
            var quest = _rotateOneSegment.Build();
            OnSegmentRotated += quest.Complete;
            quest.OnComplete += () => OnSegmentRotated -= quest.Complete;
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
        
        public Quest<int> CollectXQuest(int x)
        {
            var quest = _collectX.Build(x);
            OnCollect += quest.Progress;
            quest.OnComplete += () => OnCollect -= quest.Progress;
            return quest;
        }

        public ReceiverQuest ActivateXReceiversSimultaneouslyQuest(int x)
        {
            var quest = _activateXReceiversSimultaneously.Build(x) as ReceiverQuest;
            OnReceiversActivated += quest!.Progress;
            quest.OnComplete += () => OnReceiversActivated -= quest.Progress;
            return quest;
        }

        public ResourcesQuest ReachXBloodResourcesQuest(int x)
        {
            var quest = _reachXBloodResources.Build(x) as ResourcesQuest;
            OnResourcesReached += quest!.Progress;
            quest.OnComplete += () => OnResourcesReached -= quest.Progress;
            return quest;
        }

        public ResourcesQuest ReachXSteamResourcesQuest(int x)
        {
            var quest = _reachXSteamResources.Build(x) as ResourcesQuest;
            OnResourcesReached += quest!.Progress;
            quest.OnComplete += () => OnResourcesReached -= quest.Progress;
            return quest;
        }

        public void CameraCompleted() => OnCameraCompleted?.Invoke();
        public void SegmentRotated() => OnSegmentRotated?.Invoke();
        public void SegmentPlaced(SegmentData segmentData) => OnSegmentPlaced?.Invoke(segmentData);
        public void CollectableCollected(int x) => OnCollect?.Invoke(x);
        public void ReceiversActivated(IEnumerable<SegmentData> receivers) => OnReceiversActivated?.Invoke(receivers);
        public void ResourcesReached((int bloodResources, int steamResources) resources) => OnResourcesReached?.Invoke((resources.bloodResources, resources.steamResources));
        public void BloodAndSteamConnected() => OnBloodAndSteamConnected?.Invoke();
        private event Action OnCameraCompleted;
        private event Action OnSegmentRotated;
        private event Action<SegmentData> OnSegmentPlaced;
        private event Action<int> OnCollect;
        private event Action<IEnumerable<SegmentData>> OnReceiversActivated;
        private event Action<(int, int)> OnResourcesReached;
        private event Action OnBloodAndSteamConnected;
    }
}