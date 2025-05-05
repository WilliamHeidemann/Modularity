using System;
using UnityEngine;
using Event = Unity.Services.Analytics.Event;

namespace Runtime.UnityCloud
{
    public class QuestCompletedEvent : Event
    {
        // public QuestCompletedEvent(string questName, int resourceCount, int resourcesCollectedFromOrbs, int resourcesCollectedFromSources,
        //     int resourcesSpent, int segmentsActivated, int segmentsPlaced, int secondsSpentPlaying,
        //     int secondsSpentToCompleteQuest, string tutorialVersion) : base("questCompleted")
        // {
        //     QuestName = questName;
        //     ResourceCount = resourceCount;
        //     ResourcesCollectedFromOrbs = resourcesCollectedFromOrbs;
        //     ResourcesCollectedFromSources = resourcesCollectedFromSources;
        //     ResourcesSpent = resourcesSpent;
        //     SegmentsPlaced = segmentsPlaced;
        //     SegmentsActivated = segmentsActivated;
        //     SecondsSpentPlaying = secondsSpentPlaying;
        //     SecondsSpentToCompleteQuest = secondsSpentToCompleteQuest;
        //     TutorialVersion = tutorialVersion;
        // }

        public QuestCompletedEvent() : base("questCompleted")
        {
        }

        public string QuestName
        {
            set => SetParameter("questName", value);
        }

        public int ResourcesCollectedFromOrbs
        {
            set => SetParameter("resourcesCollectedFromOrbs", value);
        }

        public int ResourcesCollectedFromSources
        {
            set => SetParameter("resourcesCollectedFromSources", value);
        }

        public int ResourcesSpent
        {
            set => SetParameter("resourcesSpent", value);
        }

        public int SegmentsActivated
        {
            set => SetParameter("segmentsActivated", value);
        }

        public int SegmentsPlaced
        {
            set => SetParameter("segmentsPlaced", value);
        }

        public int SecondsSpentPlaying
        {
            set => SetParameter("secondsSpentPlaying", value);
        }

        public int SecondsSpentToCompleteQuest
        {
            set => SetParameter("secondsSpentToCompleteQuest", value);
        }

        public int ResourceCount
        {
            set => SetParameter("resourceCount", value);
        }
        
        public string TutorialVersion
        {
            set => SetParameter("tutorialVersion", value);
        }
    }
}