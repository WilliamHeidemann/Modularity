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
        [SerializeField] private SegmentQuest _placeOneSegment;
        [SerializeField] private Quest _rotateOneSegment;
        [SerializeField] private ReceiverQuest _activateXReceivers;
        [SerializeField] private ReceiverQuest _activateXReceiversSimultaneously;
        [SerializeField] private ResourcesQuest _reachXBloodResources;
        [SerializeField] private ResourcesQuest _reachXSteamResources;
        [SerializeField] private Quest<int> _collectX;
        
        public Quest CameraQuest()
        {
            var quest = _camera.Build();
            OnCameraCompleted += quest.Complete;
            return quest;
        }

        public SegmentQuest PlaceOneSegmentQuest()
        {
            var quest = _placeOneSegment.Build(1) as SegmentQuest;
            OnSegmentPlaced += quest!.Progress;
            return quest;
        }

        public Quest RotateOneSegmentQuest()
        {
            var quest = _rotateOneSegment.Build();
            OnSegmentRotated += quest.Complete;
            return quest;
        }

        public ReceiverQuest ActivateXReceiversQuest(int x)
        {
            var quest = _activateXReceivers.Build(x) as ReceiverQuest;
            OnReceiversActivated += quest!.Progress;
            return quest;
        }

        public ReceiverQuest ActivateXReceiversSimultaneouslyQuest(int x)
        {
            var quest = _activateXReceiversSimultaneously.Build(x) as ReceiverQuest;
            OnReceiversActivated += quest!.Progress;
            return quest;
        }

        public ResourcesQuest ReachXBloodResourcesQuest(int x)
        {
            var quest = _reachXBloodResources.Build(x) as ResourcesQuest;
            OnResourcesReached += quest!.Progress;
            return quest;
        }

        public ResourcesQuest ReachXSteamResourcesQuest(int x)
        {
            var quest = _reachXSteamResources.Build(x) as ResourcesQuest;
            OnResourcesReached += quest!.Progress;
            return quest;
        }

        public Quest<int> CollectXQuest(int x)
        {
            var quest = _collectX.Build(x);
            OnCollectableCollected += quest.Progress;
            return quest;
        }

        public void CameraCompleted() => OnCameraCompleted?.Invoke();
        public void SegmentRotated() => OnSegmentRotated?.Invoke();
        public void SegmentPlaced(SegmentData segmentData) => OnSegmentPlaced?.Invoke(segmentData);
        public void CollectableCollected(int x) => OnCollectableCollected?.Invoke(x);
        public void ReceiversActivated(IEnumerable<SegmentData> receivers) => OnReceiversActivated?.Invoke(receivers);
        public void ResourcesReached((int bloodResources, int steamResources) resources) => OnResourcesReached?.Invoke((resources.bloodResources, resources.steamResources));
        private event Action OnCameraCompleted;
        private event Action OnSegmentRotated;
        private event Action<SegmentData> OnSegmentPlaced;
        private event Action<int> OnCollectableCollected;
        private event Action<IEnumerable<SegmentData>> OnReceiversActivated;
        private event Action<(int, int)> OnResourcesReached;
    }
}