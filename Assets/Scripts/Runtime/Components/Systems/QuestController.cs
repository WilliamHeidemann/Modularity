using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Backend;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Runtime.Components.Systems
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private QuestFactory _questFactory;
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

        private int _questIndex;
        private Tutorial _tutorial;

        public void Initialize()
        {
            _questIndex = 0;
            _tutorial = new Tutorial(_questFactory);
            _autoSpawner.SpawnBloodSource();
            _hand.IncludeBlood();
            _hand.ExcludeSteam();
            _hand.ExcludeReceivers();
            ToggleResourceUI(isVisible: false);
            ToggleScoreUI(isVisible: false);
            NextQuest2();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N)) _quest.Complete();
        }

        private void NextQuest()
        {
            switch (_questIndex)
            {
                case 0:
                    _quest = _questFactory.PanQuest();
                    break;
                case 1:
                    _quest = _questFactory.RotateQuest();
                    break;
                case 2:
                    _quest = _questFactory.ZoomQuest();
                    break;
                case 3:
                    _quest = _questFactory.PlaceFirstBloodSegmentQuest();
                    _handUI.gameObject.SetActive(true);
                    _handUI.SetCardsVisible(1);
                    _hand.DrawQueuedHand(_predefinedHands.BloodHand1);
                    break;
                case 4:
                    _quest = _questFactory.RotateSegmentQuest();
                    _hand.DrawQueuedHand(_predefinedHands.BloodHand2);
                    _handUI.SetCardsVisible(3);
                    break;
                case 5:
                    ToggleResourceUI(isVisible: true);
                    _quest = _questFactory.ActivateHeartReceiverQuest(1);
                    _hand.IncludeReceivers();
                    _quest.OnComplete += _hand.ExcludeReceivers;
                    break;
                case 6:
                    _quest = _questFactory.HybridQuest();
                    _hand.IncludeSteam();
                    _hand.DrawQueuedHand(_predefinedHands.Hybrids);
                    _reRollButton.SetActive(false);
                    _handUI.SetCardsVisible(1);
                    break;
                case 7:
                    _quest = _questFactory.PlaceSteamSourceQuest();
                    _hand.DrawQueuedHand(_predefinedHands.Furnaces);
                    break;
                case 8:
                    _quest = _questFactory.ActivateFurnaceReceiverQuest(1);
                    _reRollButton.SetActive(true);
                    _hand.IncludeReceivers();
                    _hand.ExcludeBlood();
                    _handUI.SetCardsVisible(3);
                    _quest.OnComplete += _hand.IncludeBlood;
                    break;
                default:
                    _quest = _questFactory.CollectXQuest(_questIndex - 7);
                    ToggleScoreUI(isVisible: true);
                    break;
            }

            _quest.DescriptionText = _questDescription;
            TweenAnimations.FadeText(_questCanvasGroup, _questDescription, _quest.Description, _questIndex == 0);
            _quest.OnComplete += NextQuest;
            _questIndex++;
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

        private void NextQuest2()
        {
            var tutorialStep = _tutorial.GetNextStep();

            _quest = tutorialStep.Quest();
            _quest.DescriptionText = _questDescription;
            TweenAnimations.FadeText(_questCanvasGroup, _questDescription, _quest.Description, false);
            tutorialStep.OnStart();

            _quest.OnComplete += () =>
            {
                tutorialStep.IsCompleted = true;
                tutorialStep.OnComplete();
                NextQuest2();
            };
        }

        public class TutorialStep
        {
            public TutorialStep()
            {
                Debug.Log($"Creating tutorial step");
            }

            public Func<Quest> Quest;
            public Action OnStart = () => { };
            public Action OnComplete = () => { };
            public bool IsCompleted = false;
        }

        public class Tutorial
        {
            private readonly List<TutorialStep> _steps = new();

            public Tutorial(QuestFactory questFactory)
            {
                _steps.Add(new TutorialStep { Quest = questFactory.PanQuest });
                _steps.Add(new TutorialStep { Quest = questFactory.RotateQuest });
                _steps.Add(new TutorialStep { Quest = questFactory.ZoomQuest });
                _steps.Add(new TutorialStep
                {
                    Quest = questFactory.PlaceFirstBloodSegmentQuest,
                    OnStart = () =>
                    {
                        
                    }
                });
                _steps.Add(new TutorialStep { Quest = questFactory.RotateSegmentQuest });
                _steps.Add(new TutorialStep { Quest = () => questFactory.ActivateHeartReceiverQuest(1) });
                _steps.Add(new TutorialStep { Quest = questFactory.HybridQuest });
                _steps.Add(new TutorialStep { Quest = questFactory.PlaceSteamSourceQuest });
                _steps.Add(new TutorialStep { Quest = () => questFactory.ActivateFurnaceReceiverQuest(1) });
                _steps.Add(new TutorialStep { Quest = () => questFactory.CollectXQuest(3) });
            }

            public TutorialStep GetNextStep()
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
}