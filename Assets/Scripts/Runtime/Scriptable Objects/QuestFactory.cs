using System;
using System.Collections.Generic;
using Runtime.DataLayer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestFactory : ScriptableObject
    {
        [SerializeField] private AutoSpawner _autoSpawner;
        
        [Header("Quests")]
        [SerializeField] private MeasureQuest _panQuest;
        [SerializeField] private MeasureQuest _rotateQuest;
        [SerializeField] private MeasureQuest _zoomQuest;
        [SerializeField] private SegmentQuest _placeFirstHeartSegment;
        [SerializeField] private Quest<int> _rotateSegmentQuest;
        [SerializeField] private ReceiverQuest _activateHeartReceiver;
        [SerializeField] private SegmentQuest _introduceResourcesAndPlaceSegment;
        [SerializeField] private HybridQuest _hybridQuest;
        [SerializeField] private PlaceSourceQuest _placeSteamSourceQuest;
        [SerializeField] private ReceiverQuest _activateFurnaceReceiver;
        [SerializeField] private Quest<int> _collectX;
        [Header("Currently Unused Quests")]
        [SerializeField] private SegmentQuest _placeFirstSteamSegment;
        [SerializeField] private Quest _connectSteamAndFlesh;
        [SerializeField] private ReceiverQuest _activateXReceiversSimultaneously;
        [SerializeField] private SegmentQuest _placeManyBloodSegments;
        

        public void Clear()
        {
            OnSegmentPlaced = null;
            OnCollect = null;
            OnReceiversActivated = null;
            OnBloodAndSteamConnected = null;
        }

        public SegmentQuest PlaceFirstBloodSegmentQuest()
        {
            var quest = _placeFirstHeartSegment.Build(1) as SegmentQuest;
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

        public ReceiverQuest ActivateBloodSourceQuest(int x)
        {
            var quest = _activateHeartReceiver.Build(x) as ReceiverQuest;
            OnReceiversActivated += quest!.Progress;
            quest.OnComplete += () => OnReceiversActivated -= quest.Progress;
            return quest;
        }
        
        public ReceiverQuest ActivateSteamSourceQuest(int x)
        {
            var quest = _activateFurnaceReceiver.Build(x) as ReceiverQuest;
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
        
        public MeasureQuest PanQuest()
        {
            var quest = _panQuest.Build() as MeasureQuest;
            OnPan += quest!.Progress;
            quest.OnComplete += () => OnPan -= quest.Progress;
            return quest;
        }
        
        public MeasureQuest RotateQuest()
        {
            var quest = _rotateQuest.Build() as MeasureQuest;
            OnRotate += quest!.Progress;
            quest.OnComplete += () => OnRotate -= quest.Progress;
            return quest;
        }
        
        public MeasureQuest ZoomQuest()
        {
            var quest = _zoomQuest.Build() as MeasureQuest;
            OnZoom += quest!.Progress;
            quest.OnComplete += () => OnZoom -= quest.Progress;
            return quest;
        }
        
        public Quest<int> RotateSegmentQuest()
        {
            var quest = _rotateSegmentQuest.Build() as Quest<int>;
            OnRotateSegment += quest!.Progress;
            quest.OnComplete += () => OnRotateSegment -= quest.Progress;
            return quest;
        }
        
        public SegmentQuest PlaceManyBloodSegmentsQuest(int x)
        {
            var quest = _placeManyBloodSegments.Build(x) as SegmentQuest;
            OnSegmentPlaced += quest!.Progress;
            quest.OnComplete += () => OnSegmentPlaced -= quest.Progress;
            return quest;
        }
        
        public HybridQuest HybridQuest()
        {
            var quest = _hybridQuest.Build(1) as HybridQuest;
            OnSegmentPlaced += quest!.Progress;
            quest.OnComplete += () => OnSegmentPlaced -= quest.Progress;
            return quest;
        }
        
        public PlaceSourceQuest PlaceSteamSourceQuest()
        {
            var quest = _placeSteamSourceQuest.Build(1) as PlaceSourceQuest;
            OnSegmentPlaced += quest!.Progress;
            quest.OnComplete += () => OnSegmentPlaced -= quest.Progress;
            return quest;
        }
        
        public SegmentQuest IntroduceResourcesAndPlaceSegmentQuest()
        {
            var quest = _introduceResourcesAndPlaceSegment.Build(3) as SegmentQuest;
            OnSegmentPlaced += quest!.Progress;
            quest.OnComplete += () => OnSegmentPlaced -= quest.Progress;
            return quest;
        }

        public void SegmentPlaced(SegmentData segmentData) => OnSegmentPlaced?.Invoke(segmentData);
        public void CollectableCollected(int x) => OnCollect?.Invoke(x);
        public void ReceiversActivated(IEnumerable<SegmentData> receivers) => OnReceiversActivated?.Invoke(receivers);
        public void BloodAndSteamConnected() => OnBloodAndSteamConnected?.Invoke();
        public void Pan(float value) => OnPan?.Invoke(value);
        public void Rotate(float value) => OnRotate?.Invoke(value);
        public void Zoom(float value) => OnZoom?.Invoke(value);
        public void RotateSegment() => OnRotateSegment?.Invoke(1);

        private event Action<SegmentData> OnSegmentPlaced;
        private event Action<int> OnCollect;
        private event Action<IEnumerable<SegmentData>> OnReceiversActivated;
        private event Action OnBloodAndSteamConnected;
        private event Action<float> OnPan;
        private event Action<float> OnRotate;
        private event Action<float> OnZoom;
        private event Action<int> OnRotateSegment;
    }
}