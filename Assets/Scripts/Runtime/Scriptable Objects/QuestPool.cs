using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestPool : ScriptableObject
    {
        [SerializeField] private Quest _camera;
        [SerializeField] private SegmentQuest _placeOneSegment;
        [SerializeField] private Quest _rotateOneSegment;
        [SerializeField] private ReceiverQuest _activateXReceivers;
        [SerializeField] private ReceiverQuest _activateXReceiversSimultaneously;
        [SerializeField] private ResourcesQuest _reachXBloodResources;
        [SerializeField] private ResourcesQuest _reachXSteamResources;
        [SerializeField] private Quest<int> _collectX;
        
        public Quest CameraQuest => _camera.Build();
        public Quest<SegmentData> PlaceOneSegmentQuest => _placeOneSegment.Build(1);
        public Quest RotateOneSegmentQuest => _rotateOneSegment.Build();
        public Quest<IEnumerable<SegmentData>> ActivateXReceiversQuest(int x) => _activateXReceivers.Build(x);
        public Quest<IEnumerable<SegmentData>> ActivateXReceiversSimultaneouslyQuest(int x) => _activateXReceiversSimultaneously.Build(x);
        public Quest<(int, int)> ReachXBloodResourcesQuest(int x) => _reachXBloodResources.Build(x);
        public Quest<(int, int)> ReachXSteamResourcesQuest(int x) => _reachXSteamResources.Build(x);
        public Quest<int> CollectXQuest(int x) => _collectX.Build(x);
    }
}