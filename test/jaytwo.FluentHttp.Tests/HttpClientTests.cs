using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Exceptions;
using jaytwo.MimeHelper;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpClientTests
    {
        private readonly HttpClient _httpClient;

        private readonly ITestOutputHelper _output;
        private readonly ILogger _logger;

        public HttpClientTests(ITestOutputHelper output)
        {
            _output = output;
            _logger = output.BuildLogger();
            _httpClient = new HttpClient().WithBaseAddress("http://httpbin.org");
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

            var expected = response.AsAnonymousType(prototype);
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

                var expected = response.AsAnonymousType(prototype);
                Assert.Equal("world", expected.args["hello"]);
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
                        .WithUriPath("/get?hello=world")
                        .WithBaseUri("http://httpbin.org");
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
                            .WithBaseUri("http://httpbin.org")
                            .WithUriPath("/get")
                            .WithUriQuery("hello=world");
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
                    .WithUri("/patch?hello=world")
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

                var expected = response.AsAnonymousType(prototype);
                Assert.Equal("world", expected.args["hello"]);
                Assert.Equal("banana", expected.json["fruit"]);
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
                    .WithUri("/post?hello=world")
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

                var expected = response.AsAnonymousType(prototype);
                Assert.Equal("world", expected.args["hello"]);
                Assert.Equal("banana", expected.json["fruit"]);
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
                    .WithUri("/put?hello=world")
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

                var expected = response.AsAnonymousType(prototype);
                Assert.Equal("world", expected.args["hello"]);
                Assert.Equal("banana", expected.json["fruit"]);
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
                    .WithUri("/delete?hello=world")
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

                var expected = response.AsAnonymousType(prototype);
                Assert.Equal("world", expected.args["hello"]);
                Assert.Equal("banana", expected.json["fruit"]);
            }
        }

        [Fact]
        public async Task BasicAuth_Works()
        {
            // arrange
            var user = "hello";
            var pass = "world";

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/basic-auth/{user}/{pass}")
                    .WithBasicAuthentication(user, pass);
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task HiddenBasicAuth_Works()
        {
            // arrange
            var user = "hello";
            var pass = "world";

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithBasicAuthentication(user, pass)
                    .WithUriPath($"/hidden-basic-auth/{user}/{pass}");
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory]
        [InlineData("auth", "MD5")]
        [InlineData("auth-int", "MD5")]
        // not worth supporting, not even postman or mozilla supports RFC 7616
        //[InlineData("auth", "SHA-256")]
        //[InlineData("auth", "SHA-512")]
        //[InlineData("auth-int", "SHA-256")]
        //[InlineData("auth-int", "SHA-512")]
        public async Task DigestAuth_Works(string qop, string algorithm)
        {
            // arrange
            var user = "hello";
            var pass = "world";

            // act
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request =>
                {
                    request
                        .WithMethod(HttpMethod.Get)
                        .WithBaseUri("http://httpbin.org") // full url is required in the request
                        .WithUriPath($"/digest-auth/{qop}/{user}/{pass}/{algorithm}")
                        .WithDigestAuthentication(_httpClient, user, pass);
                });

                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task TokenAuth_Works()
        {
            // arrange
            var token = "hello";

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/bearer")
                    .WithTokenAuthentication(token);
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

                var actual = response.AsAnonymousType(prototype);
                Assert.NotEmpty(actual.slideshow.slides);
                Assert.NotEmpty(actual.slideshow.slides.First().title);
            }
        }
    }
}
