using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Nito.AsyncEx;
using OktaDemo.XF.Helpers;
using OktaDemo.XF.iOS.Implementations;
using OktaDemo.XF.Interfaces;
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

        private readonly AsyncAutoResetEvent _loginResultWaitHandle = new AsyncAutoResetEvent(false);

        // Authorization code flow using OIDAuthState automatic code exchanges.
        public async Task<AuthInfo> LoginAsync()
        {
            //var issuer = new NSUrl(Constants.Issuer);
            var redirectUri = new NSUrl(Constants.RedirectUri);

            //Console.WriteLine($"Fetching configuration for issuer: {issuer}");

            try
            {
                // discovers endpoints
                var configuration =
                    await AuthorizationService.DiscoverServiceConfigurationForDiscoveryAsync(
                        new NSUrl(Constants.DiscoveryEndpoint));

                Console.WriteLine($"Got configuration: {configuration}");

                // builds authentication request
                var request = new AuthorizationRequest(configuration, Constants.ClientId,
                    new string[] {Scope.OpenId, Scope.Profile, "offline_access"}, redirectUri, ResponseType.Code, null);
                // performs authentication request
                var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate;
                Console.WriteLine($"Initiating authorization request with scope: {request.Scope}");

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