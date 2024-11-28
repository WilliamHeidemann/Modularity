using System.Collections.Generic;
using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private TextMeshProUGUI _questDescription;
        [SerializeField] private Quest _mainQuest;
        [SerializeField] private int _questIndex = 0;
        
        private void Start()
        {
            _questFactory.OnCameraCompleted += CheckCompletion;
            _questFactory.OnSegmentPlaced += _ => CheckCompletion();
            _questFactory.OnSegmentRotated += CheckCompletion;
            _questFactory.OnReceiversActivated += _ => CheckCompletion();
            _questFactory.OnResourcesReached += _ => CheckCompletion();
            _questFactory.OnCollect += _ => CheckCompletion();
            
            NextQuest();
        }

        private async void CheckCompletion()
        {
            await Awaitable.NextFrameAsync();
            Debug.Log("Checking quest completion.");
            if (_mainQuest.IsCompleted)
            {
                Debug.Log("Quest completed.");
                NextQuest();
            }
        }

        private void NextQuest()
        {
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
            _questDescription.text = _mainQuest.Description;
            _questIndex++;
        }
    }
}