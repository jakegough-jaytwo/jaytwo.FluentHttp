using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Authentication.Token.OpenIdConnect;
using jaytwo.MimeHelper;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace jaytwo.FluentHttp.Tests.Authentication.OpenIdConnect
{
    public class ClientCredentialsTokenProviderTests
    {
        [Fact]
        public async Task GetTokenAsync_works()
        {
            // arrange
            var mockAccessTokenResponse = new AccessTokenResponse()
            {
                ExpiresIn = 1,
                AccessToken = "orange",
                TokenType = "apple",
            };

            var mockAccessTokenProvider = new Mock<IAccessTokenProvider>();
            mockAccessTokenProvider
                .Setup(x => x.GetAccessTokenAsync())
                .ReturnsAsync(mockAccessTokenResponse);

            var tokenProvider = new ClientCredentialsTokenProvider(mockAccessTokenProvider.Object);

            // act
            var actualToken = await tokenProvider.GetTokenAsync();

            // assert
            Assert.Equal(mockAccessTokenResponse.AccessToken, actualToken);
        }

        [Fact]
        public async Task GetTokenAsync_does_not_call_GetAccessToken_again_if_token_is_fresh()
        {
            // arrange
            var mockAccessTokenResponse = new AccessTokenResponse()
            {
                AccessToken = "orange",
                TokenType = "apple",
                Created = new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
                ExpiresIn = 10,
                Threshold = TimeSpan.FromSeconds(9),
                NowDelegate = () => new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
            };

            var mockAccessTokenProvider = new Mock<IAccessTokenProvider>();
            mockAccessTokenProvider
                .Setup(x => x.GetAccessTokenAsync())
                .ReturnsAsync(mockAccessTokenResponse);

            var tokenProvider = new ClientCredentialsTokenProvider(mockAccessTokenProvider.Object);

            // act
            await tokenProvider.GetTokenAsync();
            await tokenProvider.GetTokenAsync();
            await tokenProvider.GetTokenAsync();

            // assert
            mockAccessTokenProvider.Verify(x => x.GetAccessTokenAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTokenAsync_calls_GetAccessToken_again_if_token_is_not_fresh()
        {
            // arrange
            var mockAccessTokenResponse = new AccessTokenResponse()
            {
                AccessToken = "orange",
                TokenType = "apple",
                Created = new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
                ExpiresIn = 10,
                Threshold = TimeSpan.FromSeconds(9),
                NowDelegate = () => new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
            };

            var mockAccessTokenProvider = new Mock<IAccessTokenProvider>();
            mockAccessTokenProvider
                .Setup(x => x.GetAccessTokenAsync())
                .ReturnsAsync(mockAccessTokenResponse);

            var tokenProvider = new ClientCredentialsTokenProvider(mockAccessTokenProvider.Object);

            // act
            await tokenProvider.GetTokenAsync();
            await tokenProvider.GetTokenAsync();
            mockAccessTokenResponse.NowDelegate = () => new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 20));
            await tokenProvider.GetTokenAsync();

            // assert
            mockAccessTokenProvider.Verify(x => x.GetAccessTokenAsync(), Times.Exactly(2));
        }
    }
}
