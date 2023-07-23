using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.Http.Authentication;
using Moq;
using Xunit;

namespace jaytwo.FluentHttp.Tests;

public class AuthenticationTests
{
    public const string HttpBinUrl = "http://httpbin.jaytwo.com/";

    [Theory]
    [InlineData("hello", "world", HttpStatusCode.OK)]
    [InlineData("hello", "goodbye", HttpStatusCode.Unauthorized)]
    public async Task WithBasicAuthentication_Works(string user, string pass, HttpStatusCode expected)
    {
        // arrange
        using var client = new FluentHttpClientBuilder().Build().Wrap()
            .WithBasicAuthentication(user, pass);

        // act
        using var response = await client.SendAsync(request =>
        {
            request
                .WithMethod(HttpMethod.Get)
                .WithBaseUri(HttpBinUrl)
                .WithUriPath($"/basic-auth/hello/world");
        });

        // assert
        Assert.Equal(expected, response.StatusCode);
    }

    [Theory]
    [InlineData("hello", HttpStatusCode.OK)]
    [InlineData(null, HttpStatusCode.Unauthorized)]
    public async Task WithTokenAuthentication_string_Works(string token, HttpStatusCode expected)
    {
        // arrange
        using var client = new FluentHttpClientBuilder().Build().Wrap()
            .WithTokenAuthentication(token);

        // act
        using var response = await client.SendAsync(request =>
        {
            request
                .WithMethod(HttpMethod.Get)
                .WithBaseUri(HttpBinUrl)
                .WithUriPath($"/bearer")
                .WithHeader("Authorization", "hello");
        });

        // assert
        Assert.Equal(expected, response.StatusCode);
    }

    [Theory]
    [InlineData("hello", HttpStatusCode.OK)]
    [InlineData(null, HttpStatusCode.Unauthorized)]
    public async Task WithTokenAuthentication_delegate_Works(string token, HttpStatusCode expected)
    {
        // arrange
        using var client = new FluentHttpClientBuilder().Build().Wrap()
            .WithTokenAuthentication(() => token);

        // act
        using var response = await client.SendAsync(request =>
        {
            request
                .WithMethod(HttpMethod.Get)
                .WithBaseUri(HttpBinUrl)
                .WithUriPath($"/bearer")
                .WithUriQueryParameter("Authorization", "Bearer hello");
        });

        // assert
        Assert.Equal(expected, response.StatusCode);
    }

    [Theory]
    [InlineData("hello", HttpStatusCode.OK)]
    [InlineData(null, HttpStatusCode.Unauthorized)]
    public async Task WithTokenAuthentication_provider_Works(string token, HttpStatusCode expected)
    {
        // arrange}
        var mockTokenProvider = new Mock<ITokenProvider>();
        mockTokenProvider.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
        using var client = new FluentHttpClientBuilder().Build().Wrap()
            .WithTokenAuthentication(mockTokenProvider.Object);

        // act
        using var response = await client.SendAsync(request =>
        {
            request
                .WithMethod(HttpMethod.Get)
                .WithBaseUri(HttpBinUrl)
                .WithUriPath($"/bearer")
                .WithUriQueryParameter("Authorization", "Bearer hello");
        });

        // assert
        Assert.Equal(expected, response.StatusCode);
    }
}
