using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OktaDemo.XF.Interfaces;
using Xamarin.Forms;

namespace OktaDemo.XF.Pages
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            _activityIndicator.BindingContext = this;
            if (Device.RuntimePlatform == Device.Android)
            {
                _activityIndicator.Scale = 0.2;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                _activityIndicator.Scale = 2;
            }
        }

        private async void OnLogInButtonClicked(object sender, EventArgs e)
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();
            IsBusy = true;
            var authInfo = await loginProvider.LoginAsync();
            IsBusy = false;
            if (string.IsNullOrWhiteSpace(authInfo.AccessToken) || !authInfo.IsAuthorized)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Error", "The app can't authenticate you", "OK");
                });
            }
            else
            {
                //TODO: Save the access and refresh tokens somewhere secure

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(authInfo.IdToken);
                var name = jsonToken?.Payload?.Claims?.FirstOrDefault(x => x.Type == "name")?.Value;
                var email = jsonToken?.Payload?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;
                var preferredUsername = jsonToken?.Payload?.Claims
                    ?.FirstOrDefault(x => x.Type == "preferred_username")?.Value;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new AuthInfoPage(name, email, preferredUsername));
                });
            }
        }
    }
}