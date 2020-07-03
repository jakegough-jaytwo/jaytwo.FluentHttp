using System;
using System.Collections.Generic;
using System.Text;
using jaytwo.FluentHttp.Authentication.Token.OpenIdConnect;
using Xunit;

namespace jaytwo.FluentHttp.Tests.Authentication.OpenIdConnect
{
    public class AccessTokenResponseTests
    {
        [Fact]
        public void IsFresh_true_under_threshold()
        {
            // arrange
            var tokenResponse = new AccessTokenResponse()
            {
                Created = new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
                ExpiresIn = 10,
                Threshold = TimeSpan.FromSeconds(9),
                NowDelegate = () => new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
            };

            // act
            var isFresh = tokenResponse.IsFresh();

            // assert
            Assert.True(isFresh);
        }

        [Fact]
        public void IsFresh_false_over_threshold()
        {
            // arrange
            var tokenResponse = new AccessTokenResponse()
            {
                Created = new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 00)),
                ExpiresIn = 10,
                Threshold = TimeSpan.FromSeconds(9),
                NowDelegate = () => new DateTimeOffset(new DateTime(2020, 6, 9, 12, 00, 2)),
            };

            // act
            var isFresh = tokenResponse.IsFresh();

            // assert
            Assert.False(isFresh);
        }

        [Fact]
        public void IsFresh_true_when_created()
        {
            // arrange
            var tokenResponse = new AccessTokenResponse()
            {
                ExpiresIn = 100, // anything greater than the default timespan of 60
            };

            // act
            var isFresh = tokenResponse.IsFresh();

            // assert
            Assert.True(isFresh);
        }
    }
}
