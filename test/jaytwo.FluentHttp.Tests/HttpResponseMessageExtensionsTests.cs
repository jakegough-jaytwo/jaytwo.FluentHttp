using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Exceptions;
using Newtonsoft.Json;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpResponseMessageExtensionsTests
    {
        [Fact]
        public async Task EnsureSuccessStatusCodeAsync_OK()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            var task = Task.FromResult(response);

            // act
            await task.EnsureSuccessStatusCodeAsync();

            // assert (that it doesn't throw an exception)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task EnsureSuccessStatusCodeAsync_BadRequest()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.BadRequest;
            var task = Task.FromResult(response);

            // act && assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => task.EnsureSuccessStatusCodeAsync());
            Assert.Contains("400", exception.Message);
        }

        [Fact]
        public void EnsureExpectedStatusCode_single()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.NotFound;

            // act
            response.EnsureExpectedStatusCode(HttpStatusCode.NotFound);

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public void EnsureExpectedStatusCode_multiple_a()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.NotFound;

            // act
            response.EnsureExpectedStatusCode(HttpStatusCode.NotFound, HttpStatusCode.BadGateway);

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public void EnsureExpectedStatusCode_multiple_b()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.NotFound;

            // act
            response.EnsureExpectedStatusCode(HttpStatusCode.BadGateway, HttpStatusCode.NotFound);

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public void EnsureExpectedStatusCode_throws_exception_single()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            // act
            var exception = Assert.Throws<UnexpectedStatusCodeException>(() => response.EnsureExpectedStatusCode(HttpStatusCode.NotFound));

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public void EnsureExpectedStatusCode_throws_exception_multiple()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            // act
            var exception = Assert.Throws<UnexpectedStatusCodeException>(() => response.EnsureExpectedStatusCode(HttpStatusCode.NotFound, HttpStatusCode.BadGateway));

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public async Task EnsureExpectedStatusCodeAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.NotFound;
            var task = Task.FromResult(response);

            // act
            await task.EnsureExpectedStatusCodeAsync(HttpStatusCode.NotFound);

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public async Task EnsureExpectedStatusCodeAsync_throws_exception()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            var task = Task.FromResult(response);

            // act
            var exception = await Assert.ThrowsAsync<UnexpectedStatusCodeException>(() => task.EnsureExpectedStatusCodeAsync(HttpStatusCode.NotFound));

            // assert (that it doesn't throw an exception)
            Assert.NotNull(response);
        }

        [Fact]
        public async Task AsAnonymousTypeAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{ \"hello\":\"world\" }");
            var anonymousPrototype = new { hello = default(string) };

            // act
            var result = await response.AsAnonymousTypeAsync(anonymousPrototype);

            // assert
            Assert.NotNull(result);
            Assert.Equal("world", result.hello);
        }

        [Fact]
        public async Task AsAnonymousTypeAsync_task()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{ \"hello\":\"world\" }");
            var task = Task.FromResult(response);
            var anonymousPrototype = new { hello = default(string) };

            // act
            var result = await task.AsAnonymousTypeAsync(anonymousPrototype);

            // assert
            Assert.NotNull(result);
            Assert.Equal("world", result.hello);
        }

        [Fact]
        public async Task AsByteArrayAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            var bytes = Encoding.UTF8.GetBytes("hello world");
            response.Content = new ByteArrayContent(bytes);

            // act
            var result = await response.AsByteArrayAsync();

            // assert
            Assert.NotNull(result);
            Assert.Equal(result, bytes);
        }

        [Fact]
        public async Task AsByteArrayAsync_task()
        {
            // arrange
            var response = new HttpResponseMessage();
            var bytes = Encoding.UTF8.GetBytes("hello world");
            var memoryStream = new MemoryStream(bytes);
            response.Content = new StreamContent(memoryStream);
            var task = Task.FromResult(response);

            // act
            var result = await task.AsByteArrayAsync();

            // assert
            Assert.NotNull(result);
            Assert.Equal(result, bytes);
        }

        [Fact]
        public async Task AsStreamAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            var message = "hello world";
            var bytes = Encoding.UTF8.GetBytes(message);
            var memoryStream = new MemoryStream(bytes);
            response.Content = new StreamContent(memoryStream);

            // act
            var result = await response.AsStreamAsync();

            // assert
            Assert.NotNull(result);
            using (result)
            using (var reader = new StreamReader(result))
            {
                Assert.Equal(message, reader.ReadToEnd());
            }
        }

        [Fact]
        public async Task AsStreamAsync_task()
        {
            // arrange
            var response = new HttpResponseMessage();
            var message = "hello world";
            var bytes = Encoding.UTF8.GetBytes(message);
            response.Content = new ByteArrayContent(bytes);
            var task = Task.FromResult(response);

            // act
            var result = await task.AsStreamAsync();

            // assert
            Assert.NotNull(result);
            using (result)
            using (var reader = new StreamReader(result))
            {
                Assert.Equal(message, reader.ReadToEnd());
            }
        }

        [Fact]
        public async Task AsStringAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            var message = "hello world";
            var bytes = Encoding.UTF8.GetBytes(message);
            response.Content = new ByteArrayContent(bytes);

            // act
            var result = await response.AsStringAsync();

            // assert
            Assert.NotNull(result);
            Assert.Equal(message, result);
        }

        [Fact]
        public async Task AsStringAsync_task()
        {
            // arrange
            var response = new HttpResponseMessage();
            var message = "hello world";
            var bytes = Encoding.UTF8.GetBytes(message);
            response.Content = new ByteArrayContent(bytes);
            var task = Task.FromResult(response);

            // act
            var result = await task.AsStringAsync();

            // assert
            Assert.NotNull(result);
            Assert.Equal(message, result);
        }

        [Fact]
        public async Task AsAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{ \"hello\":\"world\" }");

            // act
            var result = await response.AsAsync<Dictionary<string, string>>();

            // assert
            Assert.NotNull(result);
            Assert.Equal("world", result["hello"]);
        }

        [Fact]
        public async Task AsAsync_task()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{ \"hello\":\"world\" }");
            var task = Task.FromResult(response);

            // act
            var result = await task.AsAsync<Dictionary<string, string>>();

            // assert
            Assert.NotNull(result);
            Assert.Equal("world", result["hello"]);
        }

        [Fact]
        public async Task ParseWithAsync()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{ \"hello\":\"world\" }");

            // act
            var result = await response.ParseWithAsync(x =>
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(x);
            });

            // assert
            Assert.NotNull(result);
            Assert.Equal("world", result["hello"]);
        }

        [Fact]
        public async Task ParseWithAsync_task()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("{ \"hello\":\"world\" }");
            var task = Task.FromResult(response);

            // act
            var result = await task.ParseWithAsync(x =>
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(x);
            });

            // assert
            Assert.NotNull(result);
            Assert.Equal("world", result["hello"]);
        }

        [Fact]
        public void GetHeaderValue_from_response()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Headers.Add("foo", "bar");

            // act
            var actual = response.GetHeaderValue("foo");

            // assert
            Assert.Equal("bar", actual);
        }

        [Fact]
        public void GetHeaderValue_from_content()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("hello world");
            response.Content.Headers.TryAddWithoutValidation("foo", "bar");

            // act
            var actual = response.GetHeaderValue("foo");

            // assert
            Assert.Equal("bar", actual);
        }

        [Theory]
        [InlineData("foo", StringComparison.OrdinalIgnoreCase, "bar")]
        [InlineData("Foo", StringComparison.OrdinalIgnoreCase, "bar")]
        [InlineData("foo", StringComparison.Ordinal, "bar")]
        [InlineData("Foo", StringComparison.Ordinal, null)]
        public void GetHeaderValue_ignore_case_from_response(string key, StringComparison stringComparison, string expected)
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Headers.Add("foo", "bar");

            // act
            var actual = response.GetHeaderValue(key, stringComparison);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("foo", StringComparison.OrdinalIgnoreCase, "bar")]
        [InlineData("Foo", StringComparison.OrdinalIgnoreCase, "bar")]
        [InlineData("foo", StringComparison.Ordinal, "bar")]
        [InlineData("Foo", StringComparison.Ordinal, null)]
        public void GetHeaderValue_ignore_case_from_content(string key, StringComparison stringComparison, string expected)
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("hello world");
            response.Content.Headers.TryAddWithoutValidation("foo", "bar");

            // act
            var actual = response.GetHeaderValue(key, stringComparison);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetHeaderValue_with_content_header_does_not_exist()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("hello world");

            // act
            var actual = response.GetHeaderValue("foo");

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetHeaderValue_header_does_not_exist()
        {
            // arrange
            var response = new HttpResponseMessage();

            // act
            var actual = response.GetHeaderValue("foo");

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetHeaderValue_StringComparison_with_content_header_does_not_exist()
        {
            // arrange
            var response = new HttpResponseMessage();
            response.Content = new StringContent("hello world");

            // act
            var actual = response.GetHeaderValue("foo", StringComparison.OrdinalIgnoreCase);

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetHeaderValue_StringComparison_header_does_not_exist()
        {
            // arrange
            var response = new HttpResponseMessage();

            // act
            var actual = response.GetHeaderValue("foo", StringComparison.OrdinalIgnoreCase);

            // assert
            Assert.Null(actual);
        }
    }
}
