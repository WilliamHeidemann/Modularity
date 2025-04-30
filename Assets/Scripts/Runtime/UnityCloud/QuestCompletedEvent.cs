using Unity.Services.Analytics;

namespace Runtime.UnityCloud
{
    public class QuestCompletedEvent : Event
    {
        public QuestCompletedEvent(string questName, int resourceCount, int resourcesCollectedFromOrbs, int resourcesCollectedFromSources,
            int resourcesSpent, int segmentsActivated, int segmentsPlaced, int secondsSpentPlaying,
            int secondsSpentToCompleteQuest) : base("questCompleted")
        {
            QuestName = questName;
            ResourceCount = resourceCount;
            ResourcesCollectedFromOrbs = resourcesCollectedFromOrbs;
            ResourcesCollectedFromSources = resourcesCollectedFromSources;
            ResourcesSpent = resourcesSpent;
            SegmentsPlaced = segmentsPlaced;
            SegmentsActivated = segmentsActivated;
            SecondsSpentPlaying = secondsSpentPlaying;
            SecondsSpentToCompleteQuest = secondsSpentToCompleteQuest;
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
    }
}