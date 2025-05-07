using Unity.Services.Analytics;
using UnityEngine;

namespace Runtime.UnityCloud
{
    public class GoogleFormsIntegration : MonoBehaviour
    {
        [SerializeField] private AccumulatedDataPoints _accumulatedDataPoints;
        
        public void SendDataToGoogleForms()
        {
            const string sessionIdFormsEntryId = "entry.1002212484";
            string sessionId = AnalyticsService.Instance.SessionID;
            const string versionFormsEntryId = "entry.65926913";
            string version = _accumulatedDataPoints.GetTutorialVersion();
            var url = $"https://docs.google.com/forms/d/e/1FAIpQLSeVBfEdKMv37HsFFF0UjiTGlntL8Nc6qWqGJKtxo56_O2LqzQ/viewform?{sessionIdFormsEntryId}={sessionId}&{versionFormsEntryId}={version}";
            Application.OpenURL(url);
        }
    }
}
