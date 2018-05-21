using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Auth0.OidcClient
{
    public partial class MainPage : ContentPage
    {
        IdentityModel.OidcClient.OidcClient _oidcClient;
        private HttpClient _apiClient;
        LoginResult _result;


        private string token;

        public MainPage()
        {
            InitializeComponent();

            Login.Clicked += Login_Clicked;
            CallApi.Clicked += CallApi_Clicked;
            CallAuthApi.Clicked += CallAuthApi_Clicked;

            var browser = DependencyService.Get<IBrowser>();

            var options = new OidcClientOptions
            {
                Authority = "https://channelyou.eu.auth0.com",
                ClientId = "mXz8kEuaIIWmEMXumX7BJnnsHkrokYw6",
                Scope = "openid profile email api offline_access",
                RedirectUri = "auth0.oidcclient://callback",
                Browser = browser,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Policy = new Policy() { RequireAccessTokenHash = false }
            };

            _oidcClient = new IdentityModel.OidcClient.OidcClient(options);
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            _result = await _oidcClient.LoginAsync(new LoginRequest());

            if (_result.IsError)
            {
                OutputText.Text = _result.Error;
                return;
            }

            var sb = new StringBuilder(128);
            foreach (var claim in _result.User.Claims)
            {
                sb.AppendFormat("{0}: {1}\n", claim.Type, claim.Value);
            }

            sb.AppendFormat("\n{0}: {1}\n", "refresh token", _result?.RefreshToken ?? "none");
            sb.AppendFormat("\n{0}: {1}\n", "access token", _result.AccessToken);

            OutputText.Text = sb.ToString();

            token = _result.AccessToken;
        }

        private async void CallApi_Clicked(object sender, EventArgs e)
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri("http://10.5.0.102:5050/");
            var result = await _apiClient.GetAsync("api/values");

            if (result.IsSuccessStatusCode)
            {
                OutputText.Text = result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                OutputText.Text = result.ReasonPhrase;
            }
        }
        private async void CallAuthApi_Clicked(object sender, EventArgs e)
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri("http://10.5.0.102:5050/");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/values/1");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _apiClient.SendAsync(requestMessage);

            if (result.IsSuccessStatusCode)
            {
                OutputText.Text = result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                OutputText.Text = result.ReasonPhrase;
            }
        }
    }
}