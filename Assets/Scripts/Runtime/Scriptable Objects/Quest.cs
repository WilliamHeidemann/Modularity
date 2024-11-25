using Runtime.Components.Segments;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class Quest
    {
        [TextArea(3, 10)]
        public string Description;
        public Quest Build()
        {
            return new Quest()
            {
                Description = Description,
            };
        }
    }
    
    [Serializable]
    public class CountingQuest : Quest
    {
        public void Advance(int amount) => _count += amount;
        private int _count;
        private int _target;
        public bool IsCompleted => _count >= _target;

        public CountingQuest Build(int target)
        {
            return new CountingQuest()
            {
                Description = Description,
                _target = target,
            };
        }
    }
}
