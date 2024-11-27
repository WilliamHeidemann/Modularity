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
        
        public Quest CameraQuest => _camera.Build(CameraCompleted);
        public Quest<SegmentData> PlaceOneSegmentQuest => _placeOneSegment.Build(1, SegmentPlaced);
        public Quest RotateOneSegmentQuest => _rotateOneSegment.Build(SegmentRotated);
        public Quest<IEnumerable<SegmentData>> ActivateXReceiversQuest(int x) => _activateXReceivers.Build(x, ReceiversActivated);
        public Quest<IEnumerable<SegmentData>> ActivateXReceiversSimultaneouslyQuest(int x) => _activateXReceiversSimultaneously.Build(x, ReceiversActivated);
        public Quest<(int, int)> ReachXBloodResourcesQuest(int x) => _reachXBloodResources.Build(x, ResourcesReached);
        public Quest<(int, int)> ReachXSteamResourcesQuest(int x) => _reachXSteamResources.Build(x, ResourcesReached);
        public Quest<int> CollectXQuest(int x) => _collectX.Build(x, CollectableCollected);
        
        public void SegmentRotated() => OnSegmentRotated?.Invoke();
        public void SegmentPlaced(SegmentData segmentData) => OnSegmentPlaced?.Invoke(segmentData);
        public void CollectableCollected(int x) => OnCollectableCollected?.Invoke(x);
        public void ReceiversActivated(IEnumerable<SegmentData> receivers) => OnReceiversActivated?.Invoke(receivers);
        public void ResourcesReached((int bloodResources, int steamResources) resources) => OnResourcesReached?.Invoke(resources.bloodResources, resources.steamResources);
        private event Action CameraCompleted;
        private event Action OnSegmentRotated;
        private event Action<SegmentData> OnSegmentPlaced;
        private event Action<int> OnCollectableCollected;
        private event Action<IEnumerable<SegmentData>> OnReceiversActivated;
        private event Action<int, int> OnResourcesReached;
    }
}