using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.HttpClientWrappers;
using jaytwo.Http;
using Moq;
using Xunit;

namespace jaytwo.FluentHttp.Tests.HttpClientWrappers;

public class BaseUriWrapperTests
{
    [Fact]
    public async void BaseUriWrapper_applies_base_uri_on_send()
    {
        // arrange
        var url = "http://www.example.com/";
        string urlFromCallback = null;
        var mockRequest = new HttpRequestMessage() { RequestUri = default };
        var mockResponse = new HttpResponseMessage();
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient
            .Setup(x => x.SendAsync(mockRequest, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
            .Callback<HttpRequestMessage, HttpCompletionOption?, CancellationToken?>((req, co, ct) =>
            {
                urlFromCallback = req.RequestUri.AbsoluteUri;
            })
            .ReturnsAsync(mockResponse);

        var wrapped = new BaseUriWrapper(mockHttpClient.Object, new Uri(url));

        // act
        await wrapped.SendAsync(mockRequest);

        // assert
        Assert.Equal(url, urlFromCallback);
        Assert.Equal(url, mockRequest.RequestUri.OriginalString);
    }
}
