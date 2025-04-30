using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace Runtime.UnityCloud
{
    public class Authenticator : MonoBehaviour
    {
        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}