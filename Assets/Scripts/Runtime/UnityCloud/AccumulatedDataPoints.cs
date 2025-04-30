using UnityEngine;

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
        public float TimeAtTutorialStart;
        public float TimeAtQuestStart;

        public void Clear()
        {
            ResourcesCollectedFromOrbs = 0;
            ResourcesCollectedFromSources = 0;
            ResourcesSpent = 0;
            SegmentsActivated = 0;
            SegmentsPlaced = 0;
            TimeAtTutorialStart = Time.time;
            TimeAtQuestStart = Time.time;
        }
    }
}