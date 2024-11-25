using UnityEngine;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestPool : ScriptableObject
    {
        [SerializeField] private Quest _camera;
        [SerializeField] private Quest _placeOneSegment;
        [SerializeField] private Quest _rotateOneSegment;
        [SerializeField] private CountingQuest _collectible;
        
        public Quest CameraQuest => _camera.Build();
        public CountingQuest CollectibleQuest => _collectible.Build(0);
    }
}