using Unity.Services.Analytics;
using UnityEngine;

namespace Runtime.UnityCloud
{
    public class GoogleFormsIntegration : MonoBehaviour
    {
        public static void SendDataToGoogleForms()
        {
            string sessionId = AnalyticsService.Instance.SessionID;
            var url = $"https://docs.google.com/forms/d/e/1FAIpQLSeVBfEdKMv37HsFFF0UjiTGlntL8Nc6qWqGJKtxo56_O2LqzQ/viewform?entry.1002212484={sessionId}";
            Application.OpenURL(url);
        }
    }
}
