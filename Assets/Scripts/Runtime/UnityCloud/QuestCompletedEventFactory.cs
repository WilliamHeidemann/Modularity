using Runtime.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Analytics;

namespace Runtime.UnityCloud
{
    [CreateAssetMenu]
    public class QuestCompletedEventFactory : ScriptableObject
    {
        [SerializeField] private AccumulatedDataPoints _accumulatedDataPoints;
        [SerializeField] private Currency _currency;

        public QuestCompletedEvent CreateQuestCompletedEvent(string questName)
        {
            return new QuestCompletedEvent{
                QuestName = questName,
                ResourceCount = _currency.BloodAmount + _currency.SteamAmount,
                ResourcesCollectedFromOrbs = _accumulatedDataPoints.ResourcesCollectedFromOrbs,
                ResourcesCollectedFromSources = _accumulatedDataPoints.ResourcesCollectedFromSources,
                ResourcesSpent = _accumulatedDataPoints.ResourcesSpent,
                SegmentsActivated = _accumulatedDataPoints.SegmentsActivated,
                SegmentsPlaced = _accumulatedDataPoints.SegmentsPlaced,
                SecondsSpentPlaying = Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtTutorialStart),
                SecondsSpentToCompleteQuest = Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtQuestStart),
                TutorialVersion = GetTutorialVersion()
            };
        }
        
        private string GetTutorialVersion()
        {
            const string versionA = "(A) Resources are present in the beginning";
            const string versionB = "(B) Resources are absent in the beginning";
            return _accumulatedDataPoints.AreResourcesPresentInTheBeginning ? versionA : versionB;
        }
    }
}