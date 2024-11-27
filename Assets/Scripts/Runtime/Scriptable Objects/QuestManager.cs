using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class QuestManager : ScriptableObject
    {
        [SerializeField] private QuestFactory _questFactory;
        [SerializeField] private Quest _mainQuest;
        
    }
}