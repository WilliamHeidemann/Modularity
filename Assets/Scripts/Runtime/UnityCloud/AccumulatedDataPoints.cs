using UnityEngine;
using UnityEngine.Analytics;

namespace Runtime.UnityCloud
{
    [CreateAssetMenu]
    public class AccumulatedDataPoints : ScriptableObject
    {
        public int ResourcesCollectedFromOrbs;
        public int ResourcesCollectedFromSources;
        public int ResourcesSpent;
        public int SegmentsActivated;
        public int SegmentsPlaced;
        public int InsufficientResourcesBuildAttempts;
        public int InvalidPlacementBuildAttempts;
        public float TimeAtTutorialStart;
        public float TimeAtQuestStart;
        public bool AreResourcesPresentInTheBeginning;
        [SerializeField] private bool _overrideVersionToPlay;

        public void Clear()
        {
            ResourcesCollectedFromOrbs = 0;
            ResourcesCollectedFromSources = 0;
            ResourcesSpent = 0;
            SegmentsActivated = 0;
            SegmentsPlaced = 0;
            InsufficientResourcesBuildAttempts = 0;
            InvalidPlacementBuildAttempts = 0;
            TimeAtTutorialStart = Time.time;
            TimeAtQuestStart = Time.time;
            if (!_overrideVersionToPlay)
            {
                AreResourcesPresentInTheBeginning = GetUserSpecificRandomBool();
            }
        }
        
        public static bool GetUserSpecificRandomBool()
        {
            int hash = Mathf.Abs(AnalyticsSessionInfo.userId.GetHashCode());
            return hash % 2 == 0;
        }
    }
}