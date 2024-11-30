using System;
using System.Collections.Generic;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Components
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private Hand _hand;
        [SerializeField] private TextMeshProUGUI _questDescription;
        [SerializeField] private TextMeshProUGUI _questExplanation;
        [SerializeField] private Quest _mainQuest;
        [SerializeField] private int _questIndex = 0;
        [SerializeField] private GameObject _cameraControlImages;
        [SerializeField] private GameObject _explanationContainer;
        [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private GameObject _handUI;
        [SerializeField] private GameObject _resourcesUI;

        private void Start()
        {
            _questFactory.OnCameraCompleted += CheckCompletion;
            _questFactory.OnSegmentPlaced += _ => CheckCompletion();
            _questFactory.OnSegmentRotated += CheckCompletion;
            _questFactory.OnReceiversActivated += _ => CheckCompletion();
            _questFactory.OnResourcesReached += _ => CheckCompletion();
            _questFactory.OnCollect += _ => CheckCompletion();

            _handUI.SetActive(false);
            _resourcesUI.SetActive(false);

            NextQuest();
        }

        private async void CheckCompletion()
        {
            await Awaitable.NextFrameAsync();
            if (_mainQuest.IsCompleted)
            {
                NextQuest();
            }
        }

        private void NextQuest()
        {
            _cameraControlImages.SetActive(_questIndex == 0);
            _mainQuest = _questIndex switch
            {
                0 => _questFactory.CameraQuest(),
                1 => _questFactory.PlaceOneSegmentQuest(),
                2 => _questFactory.RotateOneSegmentQuest(),
                3 => _questFactory.ActivateXReceiversQuest(1),
                4 => _questFactory.ActivateXReceiversSimultaneouslyQuest(2),
                5 => _questFactory.ReachXBloodResourcesQuest(50),
                6 => _questFactory.ReachXSteamResourcesQuest(50),
                // 7 => _questFactory.CollectXQuest(1),
                _ => _mainQuest
            };

            if (_questIndex == 1)
            {
                _handUI.SetActive(true);
                _resourcesUI.SetActive(true);
                _hand.Initialize();
            }

            if (_questIndex == 3)
            {
                _questFactory.OnReceiversActivated += SpawnSteamSourceOnBrainActivated;
            }

            if (_questIndex == 4)
            {
                _questFactory.OnReceiversActivated -= SpawnSteamSourceOnBrainActivated;
                _hand.EnableSteamSegments();
            }

            _explanationContainer.SetActive(_mainQuest.Explanation != string.Empty);
            _questDescription.text = $"- {_mainQuest.Description}";
            _questExplanation.text = _mainQuest.Explanation;
            _questIndex++;
        }

        private void SpawnSteamSourceOnBrainActivated(IEnumerable<SegmentData> _)
        {
            _autoSpawner.SpawnSteamSource();
        }
    }
}