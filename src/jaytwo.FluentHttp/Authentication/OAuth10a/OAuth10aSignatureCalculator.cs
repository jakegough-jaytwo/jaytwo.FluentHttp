#if !NETSTANDARD1_1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using jaytwo.UrlHelper;

namespace jaytwo.FluentHttp.Authentication.OAuth10a
{
    public class OAuth10aSignatureCalculator
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string Token { get; set; }

        public string TokenSecret { get; set; }

        public long? Timestamp { get; set; }

        public string Nonce { get; set; }

        public string Url { get; set; }

        public string SignatureMethod { get; set; } = "HMAC-SHA1";

        public string OauthVersion { get; set; } = "1.0";

        public HttpMethod HttpMethod { get; set; }

        public IDictionary<string, string> BodyParameters { get; set; }

        public string GetSignature()
        {
            if (SignatureMethod != "HMAC-SHA1")
            {
                throw new InvalidOperationException($"Unknown {nameof(SignatureMethod)}: {SignatureMethod}");
            }

            if (!string.IsNullOrWhiteSpace(OauthVersion) && OauthVersion != "1.0")
            {
                throw new InvalidOperationException($"Unknown {nameof(OauthVersion)}: {OauthVersion}");
            }

            if (string.IsNullOrWhiteSpace(Nonce))
            {
                throw new InvalidOperationException($"{nameof(Nonce)} is required");
            }

            if (Timestamp == null)
            {
                throw new InvalidOperationException($"{nameof(Timestamp)} is required");
            }

            var signatureParameters = GetSignatureBaseStringParameters();
            var signatureBaseString = GetSignatureBaseString(HttpMethod, Url, signatureParameters);
            var signature = SignBaseString(signatureBaseString, ConsumerSecret, TokenSecret);

            return signature;
        }

        public string GetAuthorizationHeaderValue()
        {
            var oauthParametersStrings = GetOAuthParameters()
                .Select(x => Uri.EscapeDataString(x.Key) + "=\"" + Uri.EscapeDataString(x.Value ?? string.Empty) + "\"")
                .ToList();

            var authorizationHeader = "OAuth " + string.Join(",", oauthParametersStrings);

            return authorizationHeader;
        }

        internal static string GetSignatureBaseString(HttpMethod httpMethod, string baseUrl, IDictionary<string, string> parameters)
        {
            var orderedParameterStrings = parameters
                .OrderBy(x => x.Key).ThenBy(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            var concatenatedParameterString = QueryString.Serialize(orderedParameterStrings);

            var baseUrlWithoutQuery = UrlHelper.Url.RemoveQuery(baseUrl);

            var signatureBaseString =
                $"{httpMethod}" +
                "&" +
                Uri.EscapeDataString(baseUrlWithoutQuery) +
                "&" +
                Uri.EscapeDataString(concatenatedParameterString);

            return signatureBaseString;
        }

        internal static string SignBaseString(string baseString, string consumerSecret, string tokenSecret)
        {
            string hashString;
            var key = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(tokenSecret ?? string.Empty);
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(baseString));
                hashString = Convert.ToBase64String(hashBytes);
            }

            return hashString;
        }

        internal IDictionary<string, string> GetSignatureBaseStringParameters()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", ConsumerKey },
                { "oauth_signature_method", SignatureMethod },
                { "oauth_timestamp", $"{Timestamp}" },
                { "oauth_nonce", Nonce },
            };

            if (!string.IsNullOrEmpty(Token))
            {
                parameters.Add("oauth_token", Token);
            }

            if (!string.IsNullOrEmpty(OauthVersion))
            {
                parameters.Add("oauth_version", OauthVersion);
            }

            var queryParameters = QueryString.Deserialize(UrlHelper.Url.GetQuery(Url));
            queryParameters.ToList().ForEach(x => parameters.Add(x.Key, x.Value.First())); // TODO: care about the edge case where the URL may contain multiple parameters of the same name

            BodyParameters?.ToList().ForEach(x => parameters.Add(x.Key, x.Value));

            return parameters;
        }

        internal string GetSignatureBaseString()
        {
            var parameters = GetSignatureBaseStringParameters();
            return GetSignatureBaseString(HttpMethod, Url, parameters);
        }

        internal IDictionary<string, string> GetOAuthParameters()
        {
            var signature = GetSignature();

            var parameters = new Dictionary<string, string>()
            {
                { "oauth_signature", signature },
            };

            GetSignatureBaseStringParameters()
                .Where(x => x.Key.StartsWith("oauth_"))
                .ToList()
                .ForEach(x => parameters.Add(x.Key, x.Value));

            return parameters;
        }
    }
}

#endif
