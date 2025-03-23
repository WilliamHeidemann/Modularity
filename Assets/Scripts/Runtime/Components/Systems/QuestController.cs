using Runtime.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Components
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private Hand _hand;
        [SerializeField] private TextMeshProUGUI _questDescription;
        [SerializeField] private Quest _quest;
        [SerializeField] private GameObject _cameraControlImages;
        [SerializeField] private AutoSpawner _autoSpawner;
        [SerializeField] private PredefinedHands _predefinedHands;
        private int _questIndex;

        public void Initialize()
        {
            _questIndex = 0;
            NextQuest();
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
                    _autoSpawner.SpawnBloodSource();
                    _hand.QueueHandsLast(_predefinedHands.BloodHands);
                    _hand.DrawHand();
                    break;
                case 10:
                    _quest = _questFactory.PlaceFirstSteamSegmentQuest();
                    _autoSpawner.SpawnSteamSource();
                    _hand.QueueHandsLast(_predefinedHands.SteamHands);
                    break;
                case 20:
                    _quest = _questFactory.ConnectSteamAndFleshQuest();
                    _hand.QueueHandFirst(_predefinedHands.Hybrids);
                    break;
                case 30:
                    _quest = _questFactory.ActivateXReceiversQuest(1);
                    _hand.QueueHandFirst(_predefinedHands.Producers);
                    break;
                case 40:
                    _quest = _questFactory.CollectXQuest(2);
                    _cameraControlImages.SetActive(false);
                    break;
                default:
                    _quest = _questFactory.CollectXQuest(_questIndex - 2);
                    break;
            }

            _quest.DescriptionText = _questDescription;
            _quest.OnComplete += NextQuest;
            _questDescription.text = $"{_quest.Description}";
            _questIndex++;
        }
    }
}