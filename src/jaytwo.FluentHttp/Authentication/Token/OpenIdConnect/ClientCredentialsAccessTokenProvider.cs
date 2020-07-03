using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Token.OpenIdConnect
{
    public class ClientCredentialsAccessTokenProvider : IAccessTokenProvider
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _httpClient;
        private readonly ClientCredentialsTokenConfig _config;

        public ClientCredentialsAccessTokenProvider(HttpClient httpClient, ClientCredentialsTokenConfig config)
            : this(httpClient.SendAsync, config)
        {
        }

        public ClientCredentialsAccessTokenProvider(Func<HttpRequestMessage, Task<HttpResponseMessage>> httpClient, ClientCredentialsTokenConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<AccessTokenResponse> GetAccessTokenAsync()
        {
            var payload = new
            {
                grant_type = "client_credentials",
                client_id = _config.ClientId,
                client_secret = _config.ClientSecret,
                resource = _config.Resource,
            };

            using (var request = new HttpRequestMessage(HttpMethod.Post, _config.TokenUrl).WithUrlEncodedFormContent(payload))
            using (var response = await _httpClient.Invoke(request))
            {
                var result = response
                    .EnsureSuccessStatusCode()
                    .As<AccessTokenResponse>();

                return result;
            }
        }
    }
}
