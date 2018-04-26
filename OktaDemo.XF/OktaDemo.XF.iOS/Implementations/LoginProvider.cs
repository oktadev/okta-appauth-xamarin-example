using System;
using System.Threading.Tasks;
using Foundation;
using Nito.AsyncEx;
using OktaDemo.XF.Helpers;
using OktaDemo.XF.Interfaces;
using OktaDemo.XF.iOS.Implementations;
using OktaDemo.XF.Models;
using OpenId.AppAuth;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(LoginProvider))]
namespace OktaDemo.XF.iOS.Implementations
{
    public class LoginProvider : IAuthStateChangeDelegate, IAuthStateErrorDelegate, ILoginProvider
    {
        // The authorization state. This is the AppAuth object that you should keep around and
        // serialize to disk.
        private AuthState _authState;

        private readonly AsyncAutoResetEvent _loginResultWaitHandle
            = new AsyncAutoResetEvent(false);

        public async Task<AuthInfo> LoginAsync()
        {
            var redirectUri = new NSUrl(Constants.RedirectUri);

            try
            {
                // discovers endpoints
                var configuration = await AuthorizationService
                    .DiscoverServiceConfigurationForDiscoveryAsync(
                        new NSUrl(Constants.DiscoveryEndpoint));

                Console.WriteLine($"Got configuration: {configuration}");

                var request = new AuthorizationRequest(
                    configuration, Constants.ClientId,
                    Constants.Scopes, redirectUri, ResponseType.Code, null);
                
                // Performs authentication request
                Console.WriteLine($"Initiating authorization request with scope: {request.Scope}");
                var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate;
                appDelegate.CurrentAuthorizationFlow = AuthState.PresentAuthorizationRequest(request,
                    UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController, (authState, error) =>
                    {
                        if (authState != null)
                        {
                            _authState = authState;
                            Console.WriteLine(
                                $"Got authorization tokens. Access token: {authState.LastTokenResponse.AccessToken}");
                        }
                        else
                        {
                            Console.WriteLine($"Authorization error: {error.LocalizedDescription}");
                            _authState = null;
                        }

                        //We need this line to tell the Login method to return the result
                        _loginResultWaitHandle.Set();
                    });
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error retrieving discovery document: {ex}");
                _authState = null;
                //We need this line to tell the Login method to return the result
                _loginResultWaitHandle.Set();
            }

            await _loginResultWaitHandle.WaitAsync();
            return new AuthInfo()
            {
                IsAuthorized = _authState?.IsAuthorized ?? false,
                AccessToken = _authState?.LastTokenResponse?.AccessToken,
                IdToken = _authState?.LastTokenResponse?.IdToken,
                RefreshToken = _authState?.LastTokenResponse?.RefreshToken,
                Scope = _authState?.LastTokenResponse?.Scope
            };
        }

        public IntPtr Handle { get; }

        public void Dispose()
        {
        }

        void IAuthStateChangeDelegate.DidChangeState(AuthState state)
        {
        }

        void IAuthStateErrorDelegate.DidEncounterAuthorizationError(AuthState state, NSError error)
        {
            Console.WriteLine($"Received authorization error: {error}.");
        }
    }
}
