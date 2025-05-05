using Unity.Services.Analytics;

namespace Runtime.UnityCloud
{
    public class GameStateEvent : Event
    {
        public GameStateEvent(string eventName) : base(eventName)
        {
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