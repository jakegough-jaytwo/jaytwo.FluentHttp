using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Authentication.Digest;
using Moq;
using Xunit;

namespace jaytwo.FluentHttp.Tests.Authentication.Digest
{
    public class DigestAuthenticationProviderTests
    {
        [Fact]
        public async Task AuthenticateAsync_works()
        {
            // arrange
            var user = "Mufasa";
            var pass = "Circle Of Life";
            var uri = "/dir/index.html";
            var realm = "testrealm@host.com";
            var nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093";
            var opaque = "5ccc069c403ebaf9f0171e9517f40e41";

            var clientNonce = "0a4f113b";

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://www.example.com" + uri),
            };

            var mockSender = new Func<HttpRequestMessage, Task<HttpResponseMessage>>(request =>
            {
                var wwwAuthenticateHeader = $"Digest realm=\"{realm}\", qop=\"auth,auth-int\", nonce=\"{nonce}\", opaque=\"{opaque}\"";

                var mockResponse = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };

                mockResponse.Headers.Add("WWW-Authenticate", wwwAuthenticateHeader);

                return Task.FromResult(mockResponse);
            });

            var digestAuth = new DigestAuthenticationProvider(mockSender, user, pass);
            digestAuth.ClientNonceGenerateDelegate = () => clientNonce;

            // act
            await digestAuth.AuthenticateAsync(request);

            // assert
            var authorizationHeader = request.Headers.Authorization.ToString();
            var digestData = DigestServerParams.DeserializeDictionary(authorizationHeader);

            Assert.Equal(user, digestData["username"]);
            Assert.Equal(realm, digestData["realm"]);
            Assert.Equal(nonce, digestData["nonce"]);
            Assert.Equal(uri, digestData["uri"]);
            Assert.Equal("auth", digestData["qop"]);
            Assert.Equal("00000001", digestData["nc"]);
            Assert.Equal(clientNonce, digestData["cnonce"]);
            Assert.Equal("6629fae49393a05397450978507c4ef1", digestData["response"]);
            Assert.Equal(opaque, digestData["opaque"]);
        }
    }
}
