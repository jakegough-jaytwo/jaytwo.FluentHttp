#if !NETSTANDARD1_1
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Digest
{
    public class DigestAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _httpClient;
        private readonly string _username;
        private readonly string _password;

        public DigestAuthenticationProvider(HttpClient httpClient, string user, string pass)
            : this(httpClient.SendAsync, user, pass)
        {
        }

        public DigestAuthenticationProvider(Func<HttpRequestMessage, Task<HttpResponseMessage>> httpClient, string user, string pass)
        {
            _httpClient = httpClient;
            _username = user;
            _password = pass;
        }

        internal Func<string> ClientNonceGenerateDelegate { get; set; } = () => Guid.NewGuid().ToString();

        public override async Task AuthenticateAsync(HttpRequestMessage request)
        {
            var digestServerParams = await GetDigestServerParams(request.RequestUri);

            var clientNonce = ClientNonceGenerateDelegate.Invoke();
            var nonceCount = 1;
            var headerValue = await GetDigesAuthorizationtHeaderAsync(digestServerParams, request, clientNonce, nonceCount);

            request.Headers.Authorization = AuthenticationHeaderValue.Parse(headerValue);
        }

        internal async Task<DigestServerParams> GetDigestServerParams(Uri uri)
        {
            using (var unauthenticatedRequest = new HttpRequestMessage(HttpMethod.Get, uri))
            using (var unauthenticatedResponse = await _httpClient.Invoke(unauthenticatedRequest))
            {
                var wwwAuthenticateHeader = unauthenticatedResponse
                    .EnsureExpectedStatusCode(HttpStatusCode.Unauthorized)
                    .GetHeaderValue("www-authenticate");

                return DigestServerParams.Parse(wwwAuthenticateHeader);
            }
        }

        internal async Task<string> GetDigesAuthorizationtHeaderAsync(DigestServerParams digestServerParams, HttpRequestMessage request, string clientNonce, int nonceCount)
        {
            var uri = request.RequestUri.PathAndQuery;
            var nonceCountAsString = $"{nonceCount}".PadLeft(8, '0'); // padleft not strictly necessary, but it makes the documented example work
            var response = await DigestCalculator.GetResponseAsync(digestServerParams, request, uri, _username, _password, clientNonce, nonceCountAsString);

            var data = new Dictionary<string, string>()
            {
                { "username",  _username },
                { "realm",  digestServerParams.Realm },
                { "nonce", digestServerParams.Nonce },
                { "uri", uri },
                { "response", response },
                { "qop", digestServerParams.Qop },
                { "nc", nonceCountAsString },
                { "cnonce", clientNonce },
                { "opaque", digestServerParams.Opaque },
            };

            var result = DigestServerParams.SerializeDictionary(data);
            return result;
        }
    }
}
#endif
