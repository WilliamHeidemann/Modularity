using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestManager : ScriptableObject
    {
        [SerializeField] private QuestPool _questPool;
        [SerializeField] private Quest _mainQuest;
        [SerializeField] private List<Quest> _sideQuests;
        
        public void Initialize()
        {
            _mainQuest = _questPool.CameraQuest;
            _sideQuests = new List<Quest>();
        }
    }
}