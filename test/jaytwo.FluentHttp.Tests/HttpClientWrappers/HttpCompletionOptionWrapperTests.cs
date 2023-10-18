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

public class HttpCompletionOptionWrapperTests
{
    [Theory]
    [InlineData(HttpCompletionOption.ResponseHeadersRead)]
    [InlineData(HttpCompletionOption.ResponseContentRead)]
    public async void HttpCompletionOptionWrapper_applies_completion_option(HttpCompletionOption expectedCompletionOption)
    {
        // arrange
        HttpCompletionOption? completionOptionFromCallback = null;
        var mockRequest = new HttpRequestMessage() { RequestUri = default };
        var mockResponse = new HttpResponseMessage();
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient
            .Setup(x => x.SendAsync(mockRequest, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
            .Callback<HttpRequestMessage, HttpCompletionOption?, CancellationToken?>((req, co, ct) =>
            {
                completionOptionFromCallback = co;
            })
            .ReturnsAsync(mockResponse);

        var wrapped = new HttpCompletionOptionWrapper(mockHttpClient.Object, expectedCompletionOption);

        // act
        await wrapped.SendAsync(mockRequest);

        // assert
        Assert.Equal(expectedCompletionOption, completionOptionFromCallback);
    }
}
