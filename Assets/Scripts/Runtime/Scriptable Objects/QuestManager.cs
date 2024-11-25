using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestManager : ScriptableObject
    {
        [SerializeField] private Quest _cameraQuest;
        
        [SerializeField] private List<Quest> _quests = new();

        public void AdvanceQuests(QuestType questType, int amount = 1)
        {
            _quests
                .Where(quest => quest.QuestType == questType)
                .ForEach(quest => CheckProgress(quest, amount));
        }

        private void CheckProgress(Quest quest, int amount)
        {
            quest.Advance(amount);
            if (quest.IsCompleted)
            {
                Debug.Log($"Quest {quest.QuestType} completed!");
            }
        }
    }
}