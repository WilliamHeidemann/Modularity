using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestManager : ScriptableObject
    {
        [SerializeField] private QuestPool _questPool;
        [SerializeField] private Quest _mainQuest;

        private event Action OnSegmentRotated;
        private event Action<SegmentData> OnSegmentPlaced;
        private event Action OnCollectableCollected;
        private event Action<IEnumerable<SegmentData>> OnReceiversActivated;
        private event Action<int, int> OnResourcesReached;

        public void SegmentRotated()
        {
            OnSegmentRotated?.Invoke();
        }
        
        public void SegmentPlaced(SegmentData segmentData)
        {
            OnSegmentPlaced?.Invoke(segmentData);
        }
        
        public void CollectableCollected()
        {
            OnCollectableCollected?.Invoke();
        }
        
        public void ReceiversActivated(IEnumerable<SegmentData> receivers)
        {
            OnReceiversActivated?.Invoke(receivers);
        }

        public void ResourcesReached(int bloodResources, int steamResources)
        {
            OnResourcesReached?.Invoke(bloodResources, steamResources);
        }

        public void GenerateQuest()
        {
            var quest = _questPool.ActivateXReceiversQuest(1);
            OnReceiversActivated += quest.Progress;
        }
    }
}