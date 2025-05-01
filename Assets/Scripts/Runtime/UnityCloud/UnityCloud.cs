using Unity.Services.Analytics;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.Analytics;

namespace Runtime.UnityCloud
{
    public class UnityCloud : MonoBehaviour
    {
        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
    }
}