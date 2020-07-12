using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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
            var httpClient = new HttpClient();
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
            var httpClient = new HttpClient();
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
            var httpClient = new HttpClient();
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
            var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(request =>
            {
                request.WithUri("http://www.google.com");
            });

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task SendAsync_func_HttpRequestMessage_Task()
        {
            // arrange
            var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(async request =>
            {
                request.WithUri("http://www.google.com");
                await Task.Delay(0);
            });

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task SendAsync_CancellationToken_action_HttpRequestMessage()
        {
            // arrange
            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act
            await Assert.ThrowsAsync<TaskCanceledException>(() => httpClient.SendAsync(cancellationTokenSource.Token, request =>
            {
                request.WithUri("http://www.google.com");
            }));

            // assert
            Assert.True(cancellationTokenSource.IsCancellationRequested);
        }

        [Fact]
        public async Task SendAsync_CancellationToken_func_HttpRequestMessage_Task()
        {
            // arrange
            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await httpClient.SendAsync(cancellationTokenSource.Token, async request =>
            {
                request.WithUri("http://www.google.com");
                await Task.Delay(0);
            }));

            // assert
            Assert.True(cancellationTokenSource.IsCancellationRequested);
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_action_HttpRequestMessage(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(httpCompletionOption, request =>
            {
                request.WithUri("http://www.google.com");
            });

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                Assert.Equal(expectedResultCanSeek, stream.CanSeek);
            }
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_func_HttpRequestMessage_Task(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(httpCompletionOption, async request =>
            {
                request.WithUri("http://www.google.com");
                await Task.Delay(0);
            });

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                Assert.Equal(expectedResultCanSeek, stream.CanSeek);
            }
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_action_HttpRequestMessage__stream_is_expected_type(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(httpCompletionOption, CancellationToken.None, request =>
            {
                request.WithUri("http://www.google.com");
            });

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                Assert.Equal(expectedResultCanSeek, stream.CanSeek);
            }
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, false)]
        [InlineData(HttpCompletionOption.ResponseContentRead, true)]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_func_HttpRequestMessage_Task__stream_is_expected_type(HttpCompletionOption httpCompletionOption, bool expectedResultCanSeek)
        {
            // arrange
            var httpClient = new HttpClient();

            // act
            var response = await httpClient.SendAsync(httpCompletionOption, CancellationToken.None, async request =>
            {
                request.WithUri("http://www.google.com");
                await Task.Delay(0);
            });

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
            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act & assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => httpClient.SendAsync(HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token, request =>
            {
                request.WithUri("http://www.google.com");
            }));
        }

        [Fact]
        public async Task SendAsync_HttpCompletionOption_CancellationToken_func_HttpRequestMessage__CancellationToken_works()
        {
            // arrange
            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(1);

            // act & assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => httpClient.SendAsync(HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token, async request =>
            {
                request.WithUri("http://www.google.com");
                await Task.Delay(0);
            }));
        }
    }
}
