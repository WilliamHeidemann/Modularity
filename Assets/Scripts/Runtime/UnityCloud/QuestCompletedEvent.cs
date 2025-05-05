using System;
using UnityEngine;
using Event = Unity.Services.Analytics.Event;

namespace Runtime.UnityCloud
{
    public class QuestCompletedEvent : GameStateEvent
    {
        public QuestCompletedEvent() : base("questCompleted")
        {
        }

        public int SecondsSpentToCompleteQuest
        {
            set => SetParameter("secondsSpentToCompleteQuest", value);
        }
    }
}