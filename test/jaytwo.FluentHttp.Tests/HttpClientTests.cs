using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Exceptions;
using jaytwo.MimeHelper;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpClientTests
    {
        public const string HttpBinUrl = "http://httpbin.jaytwo.com/";

        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;

        public HttpClientTests(ITestOutputHelper output)
        {
            _output = output;
            _httpClient = new HttpClient().WithBaseAddress(HttpBinUrl);
        }

        [Fact]
        public async Task RequestHeaders_Work()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUri("/headers")
                    .WithHeader("foo", "bar")
                    .WithHeader("fizz", "buzz");
            });

            // assert
            var prototype = new
            {
                headers = default(Dictionary<string, string>),
            };

            response.EnsureSuccessStatusCode();

            var expected = await response.AsAnonymousTypeAsync(prototype);
            Assert.Equal("bar", expected.headers["Foo"]); // don't ask me why, header keys get capitalized
            Assert.Equal("buzz", expected.headers["Fizz"]); // don't ask me why, header keys get capitalized
        }

        [Fact]
        public async Task ResponseHeaders_Work()
        {
            // arrange

            // act
            var response = await _httpClient.GetAsync("/response-headers?foo=bar");

            using (response)
            {
                // assert
                response.EnsureSuccessStatusCode();
                Assert.Equal("bar", response.Headers.GetHeaderValue("foo"));
            }
        }

        [Fact]
        public async Task HttpVersion_10_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/get")
                    .WithHttpVersion(HttpVersion.Version10);
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task HttpVersion_11_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/get")
                    .WithHttpVersion(HttpVersion.Version11);
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task HttpVersion_20_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/get")
                    .WithHttpVersion(HttpVersion.Version20);
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task ResponseHeaders_Work_ContentType()
        {
            // arrange

            // act
            var response = await _httpClient.GetAsync("/image/jpeg");

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
                Assert.Equal(MediaType.image_jpeg, response.Content.Headers.ContentType.MediaType);
            }
        }

        [Fact]
        public async Task ResponseHeaders_Work_ContentLength()
        {
            // arrange

            // act
            var response = await _httpClient.GetAsync("/image/jpeg");

            // assert
            using (response)
            {
                response.EnsureSuccessStatusCode();
                Assert.NotEqual(0, response.Content.Headers.ContentLength);
                Assert.NotEqual("0", response.GetHeaderValue("Content-Length"));
            }
        }

        [Fact]
        public async Task Get_Works()
        {
            // arrange

            // act
            var response = await _httpClient.GetAsync("/get?hello=world");

            // assert
            using (response)
            {
                var prototype = new
                {
                    args = default(Dictionary<string, string>),
                };

                var actual = await response
                    .EnsureSuccessStatusCode()
                    .AsAnonymousTypeAsync(prototype);

                Assert.Equal("world", actual.args["hello"]);
            }
        }

        [Fact]
        public async Task WithPath_before_WithBaseUri()
        {
            // arrange
            using (var client = new HttpClient())
            {
                // act
                var response = await client.SendAsync(request =>
                {
                    request
                        .WithMethod(HttpMethod.Get)
                        .WithUriPath("/get")
                        .WithUriQueryParameter("hello", "world")
                        .WithBaseUri(HttpBinUrl);
                });

                // assert
                using (response)
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            }
        }

        [Fact]
        public async Task WithBaseUri_before_WithPath()
        {
            // arrange
            using (var client = new HttpClient())
            {
                // act
                var response = await client
                    .SendAsync(request =>
                    {
                        request
                            .WithMethod(HttpMethod.Get)
                            .WithBaseUri(HttpBinUrl)
                            .WithUriPath("/get")
                            .WithUriQueryParameter("hello", "world");
                    });

                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Unexpected_MethodNotAllowed_Throws_UnexpectedStatusCodeException()
        {
            // arrange
            using (var response = await _httpClient.DeleteAsync("/get"))
            {
                // act & assert
                var exception = Assert.Throws<UnexpectedStatusCodeException>(() => response.EnsureExpectedStatusCode(HttpStatusCode.OK));

                // assert some more
                Assert.Equal(HttpStatusCode.MethodNotAllowed, exception.StatusCode);
            }
        }

        [Fact]
        public async Task Expected_MethodNotAllowed_DoesNotThrow_UnexpectedStatusCodeException()
        {
            // arrange
            using (var response = await _httpClient.DeleteAsync("/get"))
            {
                // act
                response.EnsureExpectedStatusCode(HttpStatusCode.MethodNotAllowed);

                // assert
                Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            }
        }

        [Fact]
        public async Task Patch_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod("PATCH")
                    .WithUriPath("/patch")
                    .WithUriQueryParameter("hello", "world")
                    .WithJsonContent(new { fruit = "banana" });
            });

            using (response)
            {
                // assert
                var prototype = new
                {
                    args = default(Dictionary<string, string>),
                    json = default(Dictionary<string, string>),
                };

                var actual = await response
                    .EnsureSuccessStatusCode()
                    .AsAnonymousTypeAsync(prototype);

                Assert.Equal("world", actual.args["hello"]);
                Assert.Equal("banana", actual.json["fruit"]);
            }
        }

        [Fact]
        public async Task Post_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Post)
                    .WithUriPath("/post")
                    .WithUriQueryParameter("hello", "world")
                    .WithJsonContent(new { fruit = "banana" });
            });

            using (response)
            {
                // assert
                var prototype = new
                {
                    args = default(Dictionary<string, string>),
                    json = default(Dictionary<string, string>),
                };

                var actual = await response
                    .EnsureSuccessStatusCode()
                    .AsAnonymousTypeAsync(prototype);

                Assert.Equal("world", actual.args["hello"]);
                Assert.Equal("banana", actual.json["fruit"]);
            }
        }

        [Fact]
        public async Task Put_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Put)
                    .WithUriPath("/put")
                    .WithUriQueryParameter("hello", "world")
                    .WithJsonContent(new { fruit = "banana" });
            });

            using (response)
            {
                // assert
                var prototype = new
                {
                    args = default(Dictionary<string, string>),
                    json = default(Dictionary<string, string>),
                };

                var actual = await response
                    .EnsureSuccessStatusCode()
                    .AsAnonymousTypeAsync(prototype);

                Assert.Equal("world", actual.args["hello"]);
                Assert.Equal("banana", actual.json["fruit"]);
            }
        }

        [Fact]
        public async Task Delete_Works()
        {
            // arrange

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Delete)
                    .WithUriPath("/delete")
                    .WithUriQueryParameter("hello", "world")
                    .WithJsonContent(new { fruit = "banana" });
            });

            using (response)
            {
                // assert
                var prototype = new
                {
                    args = default(Dictionary<string, string>),
                    json = default(Dictionary<string, string>),
                };

                var actual = await response
                    .EnsureSuccessStatusCode()
                    .AsAnonymousTypeAsync(prototype);

                Assert.Equal("world", actual.args["hello"]);
                Assert.Equal("banana", actual.json["fruit"]);
            }
        }

        [Fact]
        public async Task Get_json_Works()
        {
            // arrange

            // act
            var response = await _httpClient.GetAsync("/json");

            using (response)
            {
                // assert
                var prototype = new
                {
                    slideshow = new
                    {
                        slides = new[]
                        {
                            new
                            {
                                title = default(string),
                            },
                        },
                    },
                };

                var actual = await response
                    .EnsureSuccessStatusCode()
                    .AsAnonymousTypeAsync(prototype);

                Assert.NotEmpty(actual.slideshow.slides);
                Assert.NotEmpty(actual.slideshow.slides.First().title);
            }
        }
    }
}
