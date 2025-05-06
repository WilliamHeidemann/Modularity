using Unity.Services.Analytics;
using UnityEngine;

namespace Runtime.UnityCloud
{
    public static class GoogleFormsIntegration
    {
        public static void SendDataToGoogleForms()
        {
            var sessionId = AnalyticsService.Instance.SessionID;
            string url = $"https://docs.google.com/forms/d/e/1FAIpQLSeVBfEdKMv37HsFFF0UjiTGlntL8Nc6qWqGJKtxo56_O2LqzQ/viewform?entry.1002212484={sessionId}";
            Application.OpenURL(url);
        }
    }
}
