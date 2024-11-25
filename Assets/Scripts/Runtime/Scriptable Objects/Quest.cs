using Runtime.Components.Segments;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Runtime.Scriptable_Objects
{
    [Serializable]
    public class Quest
    {
        [TextArea(3, 10)]
        public string Description;
        public QuestType QuestType;
        public bool IsCompleted => Count >= Target;
        public void Advance(int amount = 1) => Count += amount;
        public int Count;
        public int Target;

        public Quest Copy()
        {
            return new Quest()
            {
                Description = Description,
                QuestType = QuestType,
                Count = 0,
                Target = Target
            };
        }
    }

    public enum QuestType
    {
        Camera,
        PlaceSegment,
        RotateSegment,
        PlaceManySegments,
        ActivateManyReceiversAtOnce
    }
}
