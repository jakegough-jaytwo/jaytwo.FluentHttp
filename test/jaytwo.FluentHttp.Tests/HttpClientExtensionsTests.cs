using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpClientExtensionsTests
    {
        [Fact]
        public void WithBaseAddress_uri()
        {
            // arrange
            using var httpClient = new HttpClient();
            var url = "http://www.example.com/";

            // act
            httpClient.WithBaseAddress(new Uri(url));

            // assert
            Assert.Equal(url, httpClient.BaseAddress.AbsoluteUri);
        }

        [Fact]
        public void WithBaseAddress_string()
        {
            // arrange
            using var httpClient = new HttpClient();
            var url = "http://www.example.com/";

            // act
            httpClient.WithBaseAddress(url);

            // assert
            Assert.Equal(url, httpClient.BaseAddress.AbsoluteUri);
        }

        [Fact]
        public void WithTimeout()
        {
            // arrange
            using var httpClient = new HttpClient();
            var timeout = TimeSpan.FromHours(1);

            // act
            httpClient.WithTimeout(timeout);

            // assert
            Assert.Equal(timeout, httpClient.Timeout);
        }

        [Fact]
        public async Task SendAsync_action_HttpRequestMessage()
        {
            // arrange
            using var httpClient = new HttpClient();

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
            using var httpClient = new HttpClient();

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
            using var httpClient = new HttpClient();
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
            using var httpClient = new HttpClient();
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

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_action_HttpRequestMessage(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            using var httpClient = new HttpClient();

            // act
            using var response = await httpClient.SendAsync(
                request =>
                {
                    request.WithUri("http://www.google.com");
                },
                completionOption: httpCompletionOption);

            // assert
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_func_HttpRequestMessage_Task(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            using var httpClient = new HttpClient();

            // act
            using var response = await httpClient.SendAsync(
                async request =>
                {
                    request.WithUri("http://www.google.com");
                    await Task.Delay(0);
                },
                completionOption: httpCompletionOption);

            // assert
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_action_HttpRequestMessage__stream_is_expected_type(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            using var httpClient = new HttpClient();

            // act
            using var response = await httpClient.SendAsync(
                request =>
                {
                    request.WithUri("http://www.google.com");
                },
                completionOption: httpCompletionOption);

            // assert
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            Assert.Equal(expectedResultCanSeek, stream.CanSeek);
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_func_HttpRequestMessage_Task__stream_is_expected_type(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            using var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(
                async request =>
                {
                    request.WithUri("http://www.google.com");
                    await Task.Delay(0);
                },
                completionOption: httpCompletionOption);

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                Assert.Equal(expectedResultCanSeek, stream.CanSeek);
            }
        }

        [Fact]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_action_HttpRequestMessage__CancellationToken_works()
        {
            // arrange
            using var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act & assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => httpClient.SendAsync(
                    request =>
                    {
                        request.WithUri("http://www.google.com");
                    },
                    cancellationTokenSource.Token,
                    HttpCompletionOption.ResponseHeadersRead));
        }

        [Fact]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_func_HttpRequestMessage__CancellationToken_works()
        {
            // arrange
            using var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act & assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => httpClient.SendAsync(
                    async request =>
                    {
                        request.WithUri("http://www.google.com");
                        await Task.Delay(0);
                    },
                    cancellationTokenSource.Token,
                    HttpCompletionOption.ResponseHeadersRead));
        }
    }
}
