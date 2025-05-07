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
                InsufficientResourcesBuildAttempts = _accumulatedDataPoints.InsufficientResourcesBuildAttempts,
                InvalidPlacementBuildAttempts = _accumulatedDataPoints.InvalidPlacementBuildAttempts,
                SecondsSpentPlaying = Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtTutorialStart),
                SecondsSpentToCompleteQuest = Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtQuestStart),
                TutorialVersion = _accumulatedDataPoints.GetTutorialVersion()
            };
        }
        
        public GameStateEvent CreateGameStateEvent()
        {
            return new GameStateEvent("GameState")
            {
                ResourceCount = _currency.BloodAmount + _currency.SteamAmount,
                ResourcesCollectedFromOrbs = _accumulatedDataPoints.ResourcesCollectedFromOrbs,
                ResourcesCollectedFromSources = _accumulatedDataPoints.ResourcesCollectedFromSources,
                ResourcesSpent = _accumulatedDataPoints.ResourcesSpent,
                SegmentsActivated = _accumulatedDataPoints.SegmentsActivated,
                SegmentsPlaced = _accumulatedDataPoints.SegmentsPlaced,
                InsufficientResourcesBuildAttempts = _accumulatedDataPoints.InsufficientResourcesBuildAttempts,
                InvalidPlacementBuildAttempts = _accumulatedDataPoints.InvalidPlacementBuildAttempts,
                SecondsSpentPlaying = Mathf.FloorToInt(Time.time - _accumulatedDataPoints.TimeAtTutorialStart),
                TutorialVersion = _accumulatedDataPoints.GetTutorialVersion()
            };
        }
    }
}