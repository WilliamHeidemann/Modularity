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
        public CountingQuest CollectibleQuest => _collectX.Build(0);
    }
}