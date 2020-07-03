using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Authentication.OAuth10a;
using Xunit;

namespace jaytwo.FluentHttp.Tests.Authentication.OAuth10a
{
    public class OAuth10aAuthenticationProviderTests
    {
        [Fact]
        public async Task AuthenticateAsync_works()
        {
            // arrange
            var consumerKey = "c5bb4dcb7bd6826c7c4340df3f791188";
            var consumerSecret = "7d30246211192cda43ede3abd9b393b9";
            var token = "VbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI=";
            var tokenSecret = "XCF9RzyQr4UEPloA+WlC06BnTfYC1P0Fwr3GUw/B0Es=";
            var nonce = "0bba225a40d1bbac2430aa0c6163ce44";
            var nowUnixTime = 1344885636;

            var oauth10aAAuth = new OAuth10aAuthenticationProvider(consumerKey, consumerSecret, token, tokenSecret)
            {
                OauthVersion = null,
                GetUtcNowDelegate = () => DateTimeOffset.FromUnixTimeSeconds(nowUnixTime),
                GetNonceDelegate = () => nonce,
            };

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://etws.etrade.com/accounts/rest/accountlist"),
            };

            var expected = "OAuth oauth_nonce=\"0bba225a40d1bbac2430aa0c6163ce44\",oauth_timestamp=\"1344885636\",oauth_consumer_key=\"c5bb4dcb7bd6826c7c4340df3f791188\",oauth_token=\"VbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI%3D\",oauth_signature=\"%2FXiv96DzZabnUG2bzPZIH2RARHM%3D\",oauth_signature_method=\"HMAC-SHA1\"";

            // act
            await oauth10aAAuth.AuthenticateAsync(request);

            // assert
            var expectedHeader = AuthenticationHeaderValue.Parse(expected);
            var actualHeader = request.Headers.Authorization;

            Assert.Equal(expectedHeader.Scheme, actualHeader.Scheme);
            Assert.Equal(
                expectedHeader.Parameter.Split(',').OrderBy(x => x),
                expectedHeader.Parameter.Split(',').OrderBy(x => x));
        }
    }
}
