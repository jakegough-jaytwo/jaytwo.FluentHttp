using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpRequestExtensionsTests
    {
        [Theory]
        [InlineData("/foo", "http://google.com", "http://google.com/foo")]
        [InlineData("foo", "http://google.com/", "http://google.com/foo")]
        [InlineData("foo", "http://google.com", "http://google.com/foo")]
        [InlineData("foo?with=query", "http://google.com", "http://google.com/foo?with=query")]
        public void WithBaseUri_after_WithUri(string uri, string baseUri, string expectedUrl)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUri(uri).WithBaseUri(baseUri);

            // assert
            Assert.Equal(expectedUrl, request.RequestUri.OriginalString);
        }

        [Theory]
        [InlineData("/foo", "/foo")]
        [InlineData("/foo/bar", "/foo/bar")]
        public void WithPath_without_url(string path, string expectedUrl)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUriPath(path);

            // assert
            Assert.Equal(expectedUrl, request.RequestUri.ToString());
        }

        [Theory]
        [InlineData("foo=bar", "?foo=bar")]
        public void WithQuery_without_url(string query, string expectedUrl)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(query);

            // assert
            Assert.Equal(expectedUrl, request.RequestUri.ToString());
        }

        [Theory]
        [InlineData("/hello", "foo=bar", "/hello?foo=bar")]
        public void WithQuery_before_WithPath(string path, string query, string expectedUrl)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(query).WithUriPath(path);

            // assert
            Assert.Equal(expectedUrl, request.RequestUri.ToString());
        }
    }
}
