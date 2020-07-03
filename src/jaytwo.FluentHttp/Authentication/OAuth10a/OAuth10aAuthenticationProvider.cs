#if !NETSTANDARD1_1
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using jaytwo.DateTimeHelper;
using jaytwo.UrlHelper;

namespace jaytwo.FluentHttp.Authentication.OAuth10a
{
    public class OAuth10aAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _token;
        private readonly string _tokenSecret;

        public OAuth10aAuthenticationProvider(string consumerKey, string consumerSecret)
            : this(consumerKey, consumerSecret, null, null)
        {
        }

        public OAuth10aAuthenticationProvider(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _token = token;
            _tokenSecret = tokenSecret;
        }

        public string OauthVersion { get; set; } = "1.0";

        internal Func<DateTimeOffset> GetUtcNowDelegate { get; set; } = () => DateTimeOffset.UtcNow;

        internal Func<string> GetNonceDelegate { get; set; } = () => Guid.NewGuid().ToString();

        public override async Task AuthenticateAsync(HttpRequestMessage request)
        {
            var calculator = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = _consumerKey,
                ConsumerSecret = _consumerSecret,
                Token = _token,
                TokenSecret = _tokenSecret,
                HttpMethod = request.Method,
                Url = request.RequestUri.AbsoluteUri,
                Timestamp = GetUtcNowDelegate.Invoke().ToUnixTimeSeconds(),
                Nonce = GetNonceDelegate.Invoke(),
                OauthVersion = OauthVersion,
            };

            if (request.Content?.Headers?.ContentType?.MediaType == "application/x-www-form-urlencoded")
            {
                await request.Content.LoadIntoBufferAsync();
                var contentAsString = await request.Content.ReadAsStringAsync();
                calculator.BodyParameters = QueryString.Deserialize(contentAsString).ToDictionary(x => x.Key, x => x.Value.First()); // TODO: care about the edge case where the URL may contain multiple parameters of the same name
            }

            var authorizationHeaderValue = calculator.GetAuthorizationHeaderValue();
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeaderValue);
        }
    }
}

#endif
