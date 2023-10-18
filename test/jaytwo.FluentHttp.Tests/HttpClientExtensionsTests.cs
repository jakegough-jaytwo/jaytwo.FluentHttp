using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.HttpClientWrappers;
using jaytwo.Http;
using Moq;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpClientExtensionsTests
    {
        [Fact]
        public void WithBaseAddress_uir_wraps_with_BaseUriWrapper()
        {
            // arrange
            var url = "http://www.example.com/";
            var mockHttpClient = new Mock<IHttpClient>();

            // act
            var wrapped = mockHttpClient.Object.WithBaseUri(new Uri(url));

            // assert
            var typed = Assert.IsType<BaseUriWrapper>(wrapped);
            Assert.Equal(url, typed.BaseUri.OriginalString);
        }

        [Fact]
        public void WithBaseAddress_string_wraps_with_BaseUriWrapper()
        {
            // arrange
            var url = "http://www.example.com/";
            var mockHttpClient = new Mock<IHttpClient>();

            // act
            var wrapped = mockHttpClient.Object.WithBaseUri(url);

            // assert
            var typed = Assert.IsType<BaseUriWrapper>(wrapped);
            Assert.Equal(url, typed.BaseUri.OriginalString);
        }

        // TODO: move to BaseUriWrapperTests
        //[Fact]
        //public void WithBaseAddress_uri()
        //{
        //    // arrange
        //    var url = "http://www.example.com/";
        //    string urlFromCallback = null;
        //    var mockHttpClient = new Mock<IHttpClient>();
        //    mockHttpClient
        //        .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
        //        .Callback<HttpRequestMessage, HttpCompletionOption?, CancellationToken?>((req, co, ct) =>
        //        {
        //            urlFromCallback = req.RequestUri.AbsoluteUri;
        //        })
        //        .ReturnsAsync(new HttpResponseMessage());

        //    // act
        //    var wrapped = mockHttpClient.Object.WithBaseUri(new Uri(url));

        //    // assert
        //    Assert.Equal(url, urlFromCallback);
        //}

        // TODO: move to BaseUriWrapperTests
        //[Fact]
        //public void WithBaseAddress_string()
        //{
        //    // arrange
        //    var url = "http://www.example.com/";
        //    string urlFromCallback = null;
        //    var mockHttpClient = new Mock<IHttpClient>();
        //    mockHttpClient
        //        .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
        //        .Callback<HttpRequestMessage, HttpCompletionOption?, CancellationToken?>((req, co, ct) =>
        //        {
        //            urlFromCallback = req.RequestUri.AbsoluteUri;
        //        })
        //        .ReturnsAsync(new HttpResponseMessage());

        //    // act
        //    var wrapped = mockHttpClient.Object.WithBaseUri(url);

        //    // assert
        //    Assert.Equal(url, urlFromCallback);
        //}

        [Fact]
        public void WithTimeout_wraps_with_TimeoutWrapper()
        {
            // arrange
            var timeout = TimeSpan.FromHours(1);
            var mockHttpClient = new Mock<IHttpClient>();

            // act
            var wrapped = mockHttpClient.Object.WithTimeout(timeout);

            // assert
            var typed = Assert.IsType<TimeoutWrapper>(wrapped);
            Assert.Equal(timeout, typed.Timeout);
        }

        // TODO: implement in TimeoutWrapperTests
        //[Fact]
        //public void WithTimeout()
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();
        //    var timeout = TimeSpan.FromHours(1);

        //    // act
        //    httpClient.WithTimeout(timeout);

        //    // assert
        //    Assert.Equal(timeout, httpClient.Timeout);
        //}

        [Fact]
        public async Task SendAsync_action_HttpRequestMessage()
        {
            // arrange
            using var httpClient = new HttpClient().Wrap();

            // act
            using var response = await httpClient.SendAsync(request =>
            {
                request.WithUri("http://www.google.com");
            });

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task SendAsync_func_HttpRequestMessage_Task()
        {
            // arrange
            using var httpClient = new HttpClient().Wrap();

            // act
            using var response = await httpClient.SendAsync(async request =>
            {
                request.WithUri("http://www.google.com");
                await Task.Delay(0);
            });

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task SendAsync_CancellationToken_action_HttpRequestMessage()
        {
            // arrange
            using var httpClient = new HttpClient().Wrap();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => httpClient.SendAsync(
                    request =>
                    {
                        request.WithUri("http://www.google.com");
                    },
                    cancellationTokenSource.Token));

            // assert
            Assert.True(cancellationTokenSource.IsCancellationRequested);
        }

        [Fact]
        public async Task SendAsync_CancellationToken_func_HttpRequestMessage_Task()
        {
            // arrange
            using var httpClient = new HttpClient().Wrap();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act
            await Assert.ThrowsAsync<TaskCanceledException>(
                async () => await httpClient.SendAsync(
                    async request =>
                    {
                        request.WithUri("http://www.google.com");
                        await Task.Delay(0);
                    },
                    cancellationTokenSource.Token));

            // assert
            Assert.True(cancellationTokenSource.IsCancellationRequested);
        }

        //[Theory]
        //[InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        //[InlineData(HttpCompletionOption.ResponseContentRead, true)]
        //public async Task SendAsync_HttpCompletionOption_action_HttpRequestMessage(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();

        //    // act
        //    using var response = await httpClient.SendAsync(
        //        request =>
        //        {
        //            request.WithUri("http://www.google.com");
        //        },
        //        completionOption: httpCompletionOption);

        //    // assert
        //    response.EnsureSuccessStatusCode();
        //    var stream = await response.Content.ReadAsStreamAsync();
        //    Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        //}

        //[Theory]
        //[InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        //[InlineData(HttpCompletionOption.ResponseContentRead, true)]
        //public async Task SendAsync_HttpCompletionOption_func_HttpRequestMessage_Task(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();

        //    // act
        //    using var response = await httpClient.SendAsync(
        //        async request =>
        //        {
        //            request.WithUri("http://www.google.com");
        //            await Task.Delay(0);
        //        },
        //        completionOption: httpCompletionOption);

        //    // assert
        //    response.EnsureSuccessStatusCode();
        //    var stream = await response.Content.ReadAsStreamAsync();
        //    Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        //}

        //[Theory]
        //[InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        //[InlineData(HttpCompletionOption.ResponseContentRead, true)]
        //public async Task SendAsync_HttpCompletionOption_CancellationToken_action_HttpRequestMessage__stream_is_expected_type(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();

        //    // act
        //    using var response = await httpClient.SendAsync(
        //        request =>
        //        {
        //            request.WithUri("http://www.google.com");
        //        },
        //        completionOption: httpCompletionOption);

        //    // assert
        //    response.EnsureSuccessStatusCode();
        //    var stream = await response.Content.ReadAsStreamAsync();
        //    Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        //}

        //[Theory]
        //[InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        //[InlineData(HttpCompletionOption.ResponseContentRead, true)]
        //public async Task SendAsync_HttpCompletionOption_CancellationToken_func_HttpRequestMessage_Task__stream_is_expected_type(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();

        //    // act
        //    var response = await httpClient.SendAsync(
        //        async request =>
        //        {
        //            request.WithUri("http://www.google.com");
        //            await Task.Delay(0);
        //        },
        //        completionOption: httpCompletionOption);

        //    // assert
        //    using (response)
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var stream = await response.Content.ReadAsStreamAsync();
        //        Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        //    }
        //}

        //[Fact]
        //public async Task SendAsync_HttpCompletionOption_CancellationToken_action_HttpRequestMessage__CancellationToken_works()
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();
        //    var cancellationTokenSource = new CancellationTokenSource(1);

        //    // act & assert
        //    await Assert.ThrowsAsync<TaskCanceledException>(
        //        () => httpClient.SendAsync(
        //            request =>
        //            {
        //                request.WithUri("http://www.google.com");
        //            },
        //            cancellationTokenSource.Token,
        //            HttpCompletionOption.ResponseHeadersRead));
        //}

        //[Fact]
        //public async Task SendAsync_HttpCompletionOption_CancellationToken_func_HttpRequestMessage__CancellationToken_works()
        //{
        //    // arrange
        //    using var httpClient = new HttpClient().Wrap();
        //    var cancellationTokenSource = new CancellationTokenSource(1);

        //    // act & assert
        //    await Assert.ThrowsAsync<TaskCanceledException>(
        //        () => httpClient.SendAsync(
        //            async request =>
        //            {
        //                request.WithUri("http://www.google.com");
        //                await Task.Delay(0);
        //            },
        //            cancellationTokenSource.Token,
        //            HttpCompletionOption.ResponseHeadersRead));
        //}
    }
}
