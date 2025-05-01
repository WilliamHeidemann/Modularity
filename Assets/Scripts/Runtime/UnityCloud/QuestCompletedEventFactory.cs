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

        public QuestCompletedEvent Create(string questName)
        {
            return new QuestCompletedEvent(
                questName: questName,
                resourceCount: _currency.BloodAmount + _currency.SteamAmount,
                resourcesCollectedFromOrbs: _accumulatedDataPoints.ResourcesCollectedFromOrbs,
                resourcesCollectedFromSources: _accumulatedDataPoints.ResourcesCollectedFromSources,
                resourcesSpent: _accumulatedDataPoints.ResourcesSpent,
                segmentsActivated: _accumulatedDataPoints.SegmentsActivated,
                segmentsPlaced: _accumulatedDataPoints.SegmentsPlaced,
                secondsSpentPlaying: Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtTutorialStart),
                secondsSpentToCompleteQuest: Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtQuestStart),
                tutorialVersion: GetTutorialVersion()
            );
        }
        
        private string GetTutorialVersion()
        {
            const string versionA = "(A) Resources are present in the beginning";
            const string versionB = "(B) Resources are absent in the beginning";
            return _accumulatedDataPoints.AreResourcesPresentInTheBeginning ? versionA : versionB;
        }
    }
}