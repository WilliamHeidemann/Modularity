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
        [SerializeField] private Quest _quest;
        [SerializeField] private GameObject _cameraControlImages;
        [SerializeField] private GameObject _explanationContainer;
        [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private GameObject _handUI;
        [SerializeField] private GameObject _resourcesUI;
        [SerializeField] private PredefinedHands _predefinedHands;
        private int _questIndex;

        private void Start()
        {
            _handUI.SetActive(false);
            _resourcesUI.SetActive(false);

            NextQuest();
        }

        private void NextQuest()
        {
            switch (_questIndex)
            {
                case 0:
                    _quest = _questFactory.CameraQuest();
                    _cameraControlImages.SetActive(true);
                    break;
                case 1:
                    _quest = _questFactory.PlaceOneSegmentQuest();
                    _autoSpawner.SpawnBloodSource();
                    _cameraControlImages.SetActive(false);
                    _handUI.SetActive(true);
                    _resourcesUI.SetActive(true);
                    _hand.QueueSpecificHands(_predefinedHands.BloodHands);
                    _hand.ExcludeSteamSegments();
                    _hand.DrawHand();
                    break;
                case 2:
                    _quest = _questFactory.RotateOneSegmentQuest();
                    break;
                case 3:
                    _quest = _questFactory.ActivateXReceiversQuest(1);
                    break;
                case 4:
                    _quest = _questFactory.ConnectSteamAndFleshQuest();
                    _hand.EnableSteamSegments();
                    _hand.QueueSpecificHands(_predefinedHands.SteamHands);
                    _hand.DrawHand();
                    _autoSpawner.SpawnSteamSource();
                    break;
                case 5:
                    _quest = _questFactory.CollectXQuest(2);
                    _autoSpawner.StartSpawningCollectables();
                    _autoSpawner.SpawnCollectable();
                    _autoSpawner.SpawnCollectable();
                    break;
                case 6:
                    _quest = _questFactory.ActivateXReceiversSimultaneouslyQuest(2);
                    break;
                case 7:
                    _quest = _questFactory.ReachXBloodResourcesQuest(50);
                    break;
                case 8:
                    _quest = _questFactory.ReachXSteamResourcesQuest(50);
                    break;
                case 9:
                    _quest = _questFactory.ActivateXReceiversSimultaneouslyQuest(6);
                    break;
                default:
                    _quest = _questFactory.ActivateXReceiversSimultaneouslyQuest(6);
                    break;
            }
            
            _quest.OnComplete += NextQuest;
            _explanationContainer.SetActive(_quest.Explanation != string.Empty);
            _questDescription.text = $"{_quest.Description}";
            _questExplanation.text = _quest.Explanation;
            _questIndex++;
        }
    }
}