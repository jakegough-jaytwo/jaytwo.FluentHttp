using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Authentication.Token.OpenIdConnect;
using jaytwo.MimeHelper;
using Moq;
using Xunit;

namespace jaytwo.FluentHttp.Tests.Authentication.OpenIdConnect
{
    public class ClientCredentialsAccessTokenProviderTests
    {
        [Fact]
        public async Task GetAccessTokenAsync_works()
        {
            // arrange
            var config = new ClientCredentialsTokenConfig()
            {
                TokenUrl = "https://example.com/oidc/access_token",
            };

            var accessToken = "howdy";
            var expiresIn = 777;
            var tokenType = "banana";

            var mockResponse =
                $@"
{{
    ""access_token"": ""{accessToken}"",
    ""expires_in"": {expiresIn},
    ""token_type"": ""{tokenType}""
}}";

            var mockSender = new Func<HttpRequestMessage, Task<HttpResponseMessage>>(request =>
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(mockResponse, Encoding.UTF8, MediaType.application_json),
                };

                return Task.FromResult(response);
            });

            var tokenProvider = new ClientCredentialsAccessTokenProvider(mockSender, config);

            // act
            var accessTokenResponse = await tokenProvider.GetAccessTokenAsync();

            // assert
            Assert.Equal(accessToken, accessTokenResponse.AccessToken);
            Assert.Equal(expiresIn, accessTokenResponse.ExpiresIn);
            Assert.Equal(tokenType, accessTokenResponse.TokenType);
        }
    }
}
