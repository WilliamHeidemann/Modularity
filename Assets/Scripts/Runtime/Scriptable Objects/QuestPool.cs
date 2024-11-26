using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestPool : ScriptableObject
    {
        [SerializeField] private Quest _camera;
        [SerializeField] private Quest _placeOneSegment;
        [SerializeField] private Quest _rotateOneSegment;
        [SerializeField] private CountingQuest _activateXReceivers;
        [SerializeField] private CountingQuest _activateXReceiversSimultaneously;
        [SerializeField] private CountingQuest _reachXBloodResources;
        [SerializeField] private CountingQuest _reachXSteamResources;
        [SerializeField] private CountingQuest _collectX;
        
        public Quest CameraQuest => _camera.Build();
        public Quest PlaceOneSegmentQuest => _placeOneSegment.Build();
        public Quest RotateOneSegmentQuest => _rotateOneSegment.Build();
        public CountingQuest ActivateXReceiversQuest(int x) => _activateXReceivers.Build(x);
        public CountingQuest ActivateXReceiversSimultaneouslyQuest(int x) => _activateXReceiversSimultaneously.Build(x);
        public CountingQuest ReachXBloodResourcesQuest(int x) => _reachXBloodResources.Build(x);
        public CountingQuest ReachXSteamResourcesQuest(int x) => _reachXSteamResources.Build(x);
        public CountingQuest CollectibleQuest(int x) => _collectX.Build(x);
    }
}