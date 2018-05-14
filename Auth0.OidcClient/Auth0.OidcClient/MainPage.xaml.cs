using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Auth0.OidcClient
{
    public partial class MainPage : ContentPage
    {
        IdentityModel.OidcClient.OidcClient _client;
        LoginResult _result;

        Lazy<HttpClient> _apiClient = new Lazy<HttpClient>(() => new HttpClient());

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

            _client = new IdentityModel.OidcClient.OidcClient(options);
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            _result = await _client.LoginAsync(new LoginRequest());

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

            _apiClient.Value.SetBearerToken(_result?.AccessToken ?? "");

        }

        private async void CallApi_Clicked(object sender, EventArgs e)
        {
            _apiClient.Value.BaseAddress = new Uri("http://localhost:63564/");
            var result = await _apiClient.Value.GetAsync("api/values");

            if (result.IsSuccessStatusCode)
            {
                OutputText.Text = JArray.Parse(await result.Content.ReadAsStringAsync()).ToString();
            }
            else
            {
                OutputText.Text = result.ReasonPhrase;
            }
        }
        private async void CallAuthApi_Clicked(object sender, EventArgs e)
        {
            if (_apiClient.Value.BaseAddress is null)
                _apiClient.Value.BaseAddress = new Uri("http://localhost:63564/");
            var result = await _apiClient.Value.GetAsync("api/values/1");

            if (result.IsSuccessStatusCode)
            {
                OutputText.Text = JArray.Parse(await result.Content.ReadAsStringAsync()).ToString();
            }
            else
            {
                OutputText.Text = result.ReasonPhrase;
            }
        }
    }
}