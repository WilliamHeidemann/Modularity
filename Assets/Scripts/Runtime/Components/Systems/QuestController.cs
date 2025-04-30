using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Backend;
using Runtime.Scriptable_Objects;
using Runtime.UnityCloud;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private AccumulatedDataPoints _accumulatedDataPoints;
        [SerializeField] private QuestCompletedEventFactory _questCompletedEventFactory;
        [SerializeField] private Hand _hand;
        [SerializeField] private CanvasGroup _questCanvasGroup;
        [SerializeField] private TextMeshProUGUI _questDescription;
        [SerializeField] private Quest _quest;
        [SerializeField] private GameObject _cameraControlImages;
        [SerializeField] private HandUI _handUI;
        [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private PredefinedHands _predefinedHands;
        [SerializeField] private GameObject _reRollButton;
        [SerializeField] private GameObject _resourceUI;
        [SerializeField] private CurrencyPopup _currencyPopup;
        [SerializeField] private GameObject _scoreTracker;

        public class TutorialStep
        {
            public Func<Quest> Quest;
            public Action OnStart = () => { };
            public Action OnComplete = () => { };
            public bool IsCompleted = false;
        }
        
        private readonly List<TutorialStep> _steps = new();

        public void Initialize()
        {
            _accumulatedDataPoints.TimeAtTutorialStart = Time.time;
            _autoSpawner.SpawnBloodSource();
            _hand.IncludeBlood();
            _hand.ExcludeSteam();
            _hand.ExcludeSources();
            ToggleResourceUI(isVisible: false);
            ToggleScoreUI(isVisible: false);
            SetUpTutorialSteps();
            NextQuest();
        }

        private void SetUpTutorialSteps()
        {
            var panQuest = new TutorialStep { Quest = _questFactory.PanQuest };
            var rotateQuest = new TutorialStep { Quest = _questFactory.RotateQuest };
            var zoomQuest = new TutorialStep { Quest = _questFactory.ZoomQuest };
            var placeFirstBloodSegmentQuest = new TutorialStep
            {
                Quest = _questFactory.PlaceFirstBloodSegmentQuest,
                OnStart = () =>
                {
                    _handUI.gameObject.SetActive(true);
                    _handUI.SetCardsVisible(1);
                    _hand.DrawQueuedHand(_predefinedHands.BloodHand1);
                }
            };
            var rotateSegmentQuest = new TutorialStep
            {
                Quest = _questFactory.RotateSegmentQuest,
                OnStart = () =>
                {
                    _hand.DrawQueuedHand(_predefinedHands.BloodHand2);
                    _handUI.SetCardsVisible(3);
                }
            };
            var introduceResourcesQuest = new TutorialStep
            {
                Quest = _questFactory.IntroduceResourcesAndPlaceSegmentQuest,
                OnStart = () =>
                {
                    _hand.DrawQueuedHand(_predefinedHands.BloodHand1);
                    ToggleResourceUI(isVisible: true);
                }
            };
            
            var activateHeartReceiverQuest = new TutorialStep
            {
                Quest = () => _questFactory.ActivateBloodSourceQuest(1),
                OnStart = () =>
                {
                    _hand.IncludeSources();
                    _hand.ExcludeSources();
                }
            };
            var hybridQuest = new TutorialStep
            {
                Quest = _questFactory.HybridQuest,
                OnStart = () =>
                {
                    _hand.IncludeSteam();
                    _hand.DrawQueuedHand(_predefinedHands.Hybrids);
                    _handUI.SetCardsVisible(1);
                }
            };
            var placeSteamSourceQuest = new TutorialStep
            {
                Quest = _questFactory.PlaceSteamSourceQuest,
                OnStart = () =>
                {
                    _hand.DrawQueuedHand(_predefinedHands.Furnaces);
                    _hand.ExcludeBlood();
                }
            };
            var activateSteamSourceQuest = new TutorialStep
            {
                Quest = () => _questFactory.ActivateSteamSourceQuest(1),
                OnStart = () =>
                {
                    _handUI.SetCardsVisible(3);
                },
                OnComplete = () =>
                {
                    _hand.IncludeSources();
                    _hand.IncludeBlood();
                }
            };
            var sphereQuest = new TutorialStep
            {
                Quest = () => _questFactory.CollectXQuest(3),
                OnStart = () => { ToggleScoreUI(isVisible: true); }
            };
            
            _steps.Add(panQuest);
            _steps.Add(rotateQuest);
            _steps.Add(zoomQuest);
            _steps.Add(placeFirstBloodSegmentQuest);
            _steps.Add(rotateSegmentQuest);
            _steps.Add(introduceResourcesQuest);
            _steps.Add(activateHeartReceiverQuest);
            _steps.Add(hybridQuest);
            _steps.Add(placeSteamSourceQuest);
            _steps.Add(activateSteamSourceQuest);
            _steps.Add(sphereQuest);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N)) _quest.Complete();
        }

        private void ToggleResourceUI(bool isVisible)
        {
            _handUI.SetCostVisible(isVisible);
            _resourceUI.SetActive(isVisible);
            _currencyPopup.SetPopupsVisible(isVisible);
        }

        private void ToggleScoreUI(bool isVisible)
        {
            _scoreTracker.SetActive(isVisible);
        }

        private void NextQuest()
        {
            var tutorialStep = GetNextStep();

            _quest = tutorialStep.Quest();
            _quest.DescriptionText = _questDescription;
            TweenAnimations.FadeText(_questCanvasGroup, _questDescription, _quest.Description, false);
            tutorialStep.OnStart();
            _accumulatedDataPoints.TimeAtQuestStart = Time.time;

            _quest.OnComplete += () =>
            {
                tutorialStep.IsCompleted = true;
                tutorialStep.OnComplete();
                AnalyticsService.Instance.RecordEvent(_questCompletedEventFactory.Create(_quest.Name));
                NextQuest();
            };
        }

        private TutorialStep GetNextStep()
        {
            var step = _steps.FirstOrDefault(x => !x.IsCompleted);
            if (step == null)
            {
                return _steps.Last();
            }

            return step;
        }
    }
}