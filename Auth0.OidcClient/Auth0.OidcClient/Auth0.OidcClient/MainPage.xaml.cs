using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;

namespace Auth0.OidcClient
{
    public partial class MainPage : ContentPage
    {
        IdentityModel.OidcClient.OidcClient _client;

        LoginResult _result;

        Lazy<HttpClient> _apiClient = new Lazy<HttpClient>(() => new HttpClient());

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            _result = await _client.LoginAsync(new LoginRequest());

            if (_result.IsError)
            {
                //OutputText.Text = _result.Error;
                return;
            }

            var sb = new StringBuilder(128);
            foreach (var claim in _result.User.Claims)
            {
                sb.AppendFormat("{0}: {1}\n", claim.Type, claim.Value);
            }

            sb.AppendFormat("\n{0}: {1}\n", "refresh token", _result?.RefreshToken ?? "none");
            sb.AppendFormat("\n{0}: {1}\n", "access token", _result.AccessToken);

            //OutputText.Text = sb.ToString();

            //_apiClient.Value.SetBearerToken(_result?.AccessToken ?? "");
            //_apiClient.Value.BaseAddress = new Uri("https://demo.identityserver.io/");
        }

        public MainPage()
        {
            InitializeComponent();

            var browser = DependencyService.Get<IBrowser>();

            var options = new OidcClientOptions
            {
                Authority = "https://channelyou.eu.auth0.com",
                ClientId = "mXz8kEuaIIWmEMXumX7BJnnsHkrokYw6",
                Scope = "openid profile email api offline_access",
                RedirectUri = "auth0oidcclient://callback",
                Browser = browser,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Policy = new Policy() { RequireAccessTokenHash = false }
            };

            _client = new IdentityModel.OidcClient.OidcClient(options);
        }
    }
}