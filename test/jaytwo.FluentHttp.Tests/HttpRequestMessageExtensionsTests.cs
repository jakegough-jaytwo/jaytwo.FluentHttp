using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.UrlHelper;
using Newtonsoft.Json;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class HttpRequestMessageExtensionsTests
    {
        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("FOO")]
        public void WithMethod(string methodString)
        {
            // arrange
            var request = new HttpRequestMessage();
            var method = new HttpMethod(methodString);

            // act
            request.WithMethod(method);

            // assert
            Assert.Equal(methodString, request.Method.Method);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("FOO")]
        public void WithMethod_string(string methodString)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithMethod(methodString);

            // assert
            Assert.Equal(methodString, request.Method.Method);
        }

        [Fact]
        public void WithBaseUri_string_without_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithBaseUri("http://www.google.com/");

            // assert
            Assert.Equal("http://www.google.com/", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithBaseUri_string_with_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("/foo/bar", UriKind.Relative);

            // act
            request.WithBaseUri("http://www.google.com/");

            // assert
            Assert.Equal("http://www.google.com/foo/bar", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithBaseUri_uri_without_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithBaseUri(new Uri("http://www.google.com/"));

            // assert
            Assert.Equal("http://www.google.com/", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithBaseUri_uri_with_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("/foo/bar", UriKind.Relative);

            // act
            request.WithBaseUri(new Uri("http://www.google.com/"));

            // assert
            Assert.Equal("http://www.google.com/foo/bar", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUri_string_without_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUri("http://www.google.com/");

            // assert
            Assert.Equal("http://www.google.com/", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUri_string_with_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("/foo/bar", UriKind.Relative);

            // act
            request.WithUri("http://www.google.com/");

            // assert
            Assert.Equal("http://www.google.com/", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUri_uri_without_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUri(new Uri("http://www.google.com/"));

            // assert
            Assert.Equal("http://www.google.com/", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUri_uri_with_existing_uri()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("/foo/bar", UriKind.Relative);

            // act
            request.WithUri(new Uri("http://www.google.com/"));

            // assert
            Assert.Equal("http://www.google.com/", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUri_format()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUri("http://www.google.com/{0}/{1}/{2}", "*", 1, "foo");

            // assert
            Assert.Equal("http://www.google.com/%2A/1/foo", request.RequestUri.AbsoluteUri);
        }

        [Theory]
        [InlineData(null, "a/b/c", "a/b/c")]
        [InlineData("http://www.google.com/", "a/b/c", "http://www.google.com/a/b/c")]
        [InlineData("http://www.google.com/a/b/c", "d/e/f", "http://www.google.com/a/b/c/d/e/f")]
        [InlineData("http://www.google.com/a/b/c", "./d/e/f", "http://www.google.com/a/b/c/d/e/f")]
        [InlineData("http://www.google.com/a/b/c", "../d/e/f", "http://www.google.com/a/b/d/e/f")]
        [InlineData("http://www.google.com/a/b/c", "../../d/e/f", "http://www.google.com/a/d/e/f")]
        [InlineData("http://www.google.com/a/b/c", "/d/e/f", "http://www.google.com/d/e/f")]
        public void WithUriPath(string startingUrl, string path, string expected)
        {
            // arrange
            var request = new HttpRequestMessage();
            if (startingUrl != null)
            {
                request.RequestUri = new Uri(startingUrl);
            }

            // act
            request.WithUriPath(path);

            // assert
            Assert.Equal(expected, request.RequestUri.ToString());
        }

        [Fact]
        public void WithUriPath_format()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://www.google.com/");

            // act
            request.WithUriPath("{0}/{1}/{2}", "*", 1, "foo");

            // assert
            Assert.Equal("http://www.google.com/%2A/1/foo", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQuery_without_RequestUri()
        {
            // arrange
            var obj = new
            {
                hello = "world",
                foo = "bar",
            };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(obj);

            // assert
            Assert.Equal("?hello=world&foo=bar", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQuery_with_RequestUri()
        {
            // arrange
            var obj = new
            {
                hello = "world",
                foo = "bar",
            };
            var request = new HttpRequestMessage().WithUri("http://www.example.com/");

            // act
            request.WithUriQuery(obj);

            // assert
            Assert.Equal("http://www.example.com/?hello=world&foo=bar", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQuery_object()
        {
            // arrange
            var obj = new
            {
                hello = "world",
                foo = "bar",
            };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(obj);

            // assert
            Assert.Equal("?hello=world&foo=bar", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQuery_dictionary_string_string()
        {
            // arrange
            var dictionary = new Dictionary<string, string>()
            {
                { "hello", "world" },
                { "foo", "bar" },
            };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(dictionary);

            // assert
            Assert.Equal("?hello=world&foo=bar", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQuery_dictionary_string_stringarray()
        {
            // arrange
            var dictionary = new Dictionary<string, string[]>()
            {
                { "hello", new[] { "world", "team" } },
                { "foo", new[] { "bar" } },
            };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(dictionary);

            // assert
            Assert.Equal("?hello=world&hello=team&foo=bar", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQuery_dictionary_string_object()
        {
            // arrange
            var dictionary = new Dictionary<string, object>()
            {
                { "hello", "world" },
                { "foo", 123 },
            };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(dictionary);

            // assert
            Assert.Equal("?hello=world&foo=123", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQuery_dictionary_string_objectarray()
        {
            // arrange
            var dictionary = new Dictionary<string, object[]>()
            {
                { "hello", new object[] { "world", 123 } },
                { "foo", new object[] { "bar", 456 } },
            };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQuery(dictionary);

            // assert
            Assert.Equal("?hello=world&hello=123&foo=bar&foo=456", request.RequestUri.OriginalString);
        }

        [Fact]
        public void GetHeaderValue_from_request()
        {
            // arrange
            var request = new HttpRequestMessage().WithHeader("foo", "bar");

            // act
            var actual = request.GetHeaderValue("foo");

            // assert
            Assert.Equal("bar", actual);
        }

        [Fact]
        public void GetHeaderValue_from_content()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.Content = new StringContent("hello world");
            request.Content.Headers.TryAddWithoutValidation("foo", "bar");

            // act
            var actual = request.GetHeaderValue("foo");

            // assert
            Assert.Equal("bar", actual);
        }

        [Theory]
        [InlineData("foo", StringComparison.OrdinalIgnoreCase, "bar")]
        [InlineData("Foo", StringComparison.OrdinalIgnoreCase, "bar")]
        [InlineData("foo", StringComparison.Ordinal, "bar")]
        [InlineData("Foo", StringComparison.Ordinal, null)]
        public void GetHeaderValue_ignore_case_from_request(string key, StringComparison stringComparison, string expected)
        {
            // arrange
            var request = new HttpRequestMessage().WithHeader("foo", "bar");

            // act
            var actual = request.GetHeaderValue(key, stringComparison);

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
            var request = new HttpRequestMessage();
            request.Content = new StringContent("hello world");
            request.Content.Headers.TryAddWithoutValidation("foo", "bar");

            // act
            var actual = request.GetHeaderValue(key, stringComparison);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetHeaderValue_with_content_header_does_not_exist()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.Content = new StringContent("hello world");

            // act
            var actual = request.GetHeaderValue("foo");

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetHeaderValue_header_does_not_exist()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            var actual = request.GetHeaderValue("foo");

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetHeaderValue_StringComparison_with_content_header_does_not_exist()
        {
            // arrange
            var request = new HttpRequestMessage();
            request.Content = new StringContent("hello world");

            // act
            var actual = request.GetHeaderValue("foo", StringComparison.OrdinalIgnoreCase);

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetHeaderValue_StringComparison_header_does_not_exist()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            var actual = request.GetHeaderValue("foo", StringComparison.OrdinalIgnoreCase);

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void WithHttpVersion_Version11()
        {
            // arrange
            Version version = HttpVersion.Version11;
            var request = new HttpRequestMessage();

            // act
            request.WithHttpVersion(version);

            // assert
            Assert.Equal(version, request.Version);
        }

        [Fact]
        public void WithHttpVersion_Version20()
        {
            // arrange
            Version version = HttpVersion.Version20;
            var request = new HttpRequestMessage();

            // act
            request.WithHttpVersion(version);

            // assert
            Assert.Equal(version, request.Version);
        }

        [Fact]
        public void WithHeader_string()
        {
            // arrange
            var name = "somename";
            var value = "somevalue";
            var expected = value;
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, value);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Fact]
        public void WithHeader_object()
        {
            // arrange
            var name = "somename";
            var value = 123;
            var expected = "123";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, value);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData(null, InclusionRule.IncludeAlways, "")]
        [InlineData("", InclusionRule.IncludeAlways, "")]
        [InlineData("abc", InclusionRule.IncludeAlways, "abc")]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData("", InclusionRule.ExcludeIfNull, "")]
        [InlineData("abc", InclusionRule.ExcludeIfNull, "abc")]
        [InlineData(null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("", InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("abc", InclusionRule.ExcludeIfNullOrEmpty, "abc")]
        public void WithHeader_string_InclusionRule(string value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData(null, InclusionRule.IncludeAlways, "")]
        [InlineData("", InclusionRule.IncludeAlways, "")]
        [InlineData("abc", InclusionRule.IncludeAlways, "abc")]
        [InlineData(123, InclusionRule.IncludeAlways, "123")]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData("", InclusionRule.ExcludeIfNull, "")]
        [InlineData("abc", InclusionRule.ExcludeIfNull, "abc")]
        [InlineData(123, InclusionRule.ExcludeIfNull, "123")]
        [InlineData(null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("", InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("abc", InclusionRule.ExcludeIfNullOrEmpty, "abc")]
        [InlineData(123, InclusionRule.ExcludeIfNullOrEmpty, "123")]
        public void WithHeader_object_InclusionRule(object value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData("#{0}", null, "#")]
        [InlineData("#{0}", "", "#")]
        [InlineData("#{0}", "abc", "#abc")]
        [InlineData("#{0}", 123, "#123")]
        public void WithHeader_format_object(string format, object value, string expected)
        {
            // arrange
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData("#{0}", null, InclusionRule.IncludeAlways, "#")]
        [InlineData("#{0}", "", InclusionRule.IncludeAlways, "#")]
        [InlineData("#{0}", "abc", InclusionRule.IncludeAlways, "#abc")]
        [InlineData("#{0}", 123, InclusionRule.IncludeAlways, "#123")]
        [InlineData("#{0}", null, InclusionRule.ExcludeIfNull, null)]
        [InlineData("#{0}", "", InclusionRule.ExcludeIfNull, "#")]
        [InlineData("#{0}", "abc", InclusionRule.ExcludeIfNull, "#abc")]
        [InlineData("#{0}", 123, InclusionRule.ExcludeIfNull, "#123")]
        [InlineData("#{0}", null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("#{0}", "", InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("#{0}", "abc", InclusionRule.ExcludeIfNullOrEmpty, "#abc")]
        [InlineData("#{0}", 123, InclusionRule.ExcludeIfNullOrEmpty, "#123")]
        public void WithHeader_format_object_InclusionRule(string format, object value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData("#{0}.{1}", 123, 456, InclusionRule.IncludeAlways, "#123.456")]
        [InlineData("#{0}.{1}", 123, 456, InclusionRule.ExcludeIfNull, "#123.456")]
        [InlineData("#{0}.{1}", 123, 456, InclusionRule.ExcludeIfNullOrEmpty, "#123.456")]
        public void WithHeader_format_object_array_InclusionRule(string format, object value1, object value2, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new[] { value1, value2 };
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData("foo", InclusionRule.IncludeAlways, "foo")]
        [InlineData("foo", InclusionRule.ExcludeIfNull, "foo")]
        [InlineData("foo", InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithHeader_format_empty_object_array_InclusionRule(string format, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new object[] { };
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Theory]
        [InlineData("foo", InclusionRule.IncludeAlways, "foo")]
        [InlineData("foo", InclusionRule.ExcludeIfNull, null)]
        [InlineData("foo", InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithHeader_format_null_object_array_InclusionRule(string format, InclusionRule inclusionRule, string expected)
        {
            // arrange
            object[] value = null;
            var name = "somename";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Fact]
        public void WithHeader_format_object_array()
        {
            // arrange
            var format = "#{0}.{1}";
            var value = new object[] { 123, 456 };
            var name = "somename";
            var expected = "#123.456";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Fact]
        public void WithHeader_format_empty_object_array()
        {
            // arrange
            var format = "foo";
            var value = new object[] { };
            var name = "somename";
            var expected = "foo";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Fact]
        public void WithHeader_format_null_object_array()
        {
            // arrange
            var format = "foo";
            object[] value = null;
            var name = "somename";
            var expected = "foo";
            var request = new HttpRequestMessage();

            // act
            request.WithHeader(name, format, value);

            // assert
            Assert.Equal(expected, request.GetHeaderValue(name));
        }

        [Fact]
        public void WithHeaderAccept()
        {
            // arrange
            var mediaType = "foo/bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAccept(mediaType);

            // assert
            Assert.Equal(mediaType, request.Headers.Accept.Single().ToString());
        }

        [Fact]
        public void WithHeaderAccept_multiple()
        {
            // arrange
            var mediaType1 = "foo/bar";
            var mediaType2 = "fizz/buzz";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAccept(mediaType1).WithHeaderAccept(mediaType2);

            // assert
            Assert.Equal(mediaType1, request.Headers.Accept.First().ToString());
            Assert.Equal(mediaType2, request.Headers.Accept.Last().ToString());
        }

        [Fact]
        public void WithHeaderAcceptApplicationJson()
        {
            // arrange
            var mediaType = "application/json";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptApplicationJson();

            // assert
            Assert.Equal(mediaType, request.Headers.Accept.Single().ToString());
        }

        [Fact]
        public void WithHeaderAcceptTextXml()
        {
            // arrange
            var mediaType = "text/xml";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptTextXml();

            // assert
            Assert.Equal(mediaType, request.Headers.Accept.Single().ToString());
        }

        [Fact]
        public void WithHeaderAcceptCharset()
        {
            // arrange
            var charset = "foo-bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptCharset(charset);

            // assert
            Assert.Equal(charset, request.Headers.AcceptCharset.Single().ToString());
        }

        [Fact]
        public void WithHeaderAcceptCharset_multiple()
        {
            // arrange
            var mediaType1 = "foo-bar";
            var mediaType2 = "fizz-buzz";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptCharset(mediaType1).WithHeaderAcceptCharset(mediaType2);

            // assert
            Assert.Equal(mediaType1, request.Headers.AcceptCharset.First().ToString());
            Assert.Equal(mediaType2, request.Headers.AcceptCharset.Last().ToString());
        }

        [Fact]
        public void WithHeaderAcceptEncoding()
        {
            // arrange
            var charset = "foo-bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptEncoding(charset);

            // assert
            Assert.Equal(charset, request.Headers.AcceptEncoding.Single().ToString());
        }

        [Fact]
        public void WithHeaderAcceptEncoding_multiple()
        {
            // arrange
            var mediaType1 = "foo-bar";
            var mediaType2 = "fizz-buzz";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptEncoding(mediaType1).WithHeaderAcceptEncoding(mediaType2);

            // assert
            Assert.Equal(mediaType1, request.Headers.AcceptEncoding.First().ToString());
            Assert.Equal(mediaType2, request.Headers.AcceptEncoding.Last().ToString());
        }

        [Fact]
        public void WithHeaderAcceptLanguage()
        {
            // arrange
            var charset = "foo-bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptLanguage(charset);

            // assert
            Assert.Equal(charset, request.Headers.AcceptLanguage.Single().ToString());
        }

        [Fact]
        public void WithHeaderAcceptLanguage_multiple()
        {
            // arrange
            var mediaType1 = "foo-bar";
            var mediaType2 = "fizz-buzz";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAcceptLanguage(mediaType1).WithHeaderAcceptLanguage(mediaType2);

            // assert
            Assert.Equal(mediaType1, request.Headers.AcceptLanguage.First().ToString());
            Assert.Equal(mediaType2, request.Headers.AcceptLanguage.Last().ToString());
        }

        [Fact]
        public void WithHeaderAuthorization_scheme_only()
        {
            // arrange
            var scheme = "foo";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAuthorization(scheme);

            // assert
            Assert.Equal(scheme, request.Headers.Authorization.ToString());
        }

        [Fact]
        public void WithHeaderAuthorization()
        {
            // arrange
            var scheme = "foo";
            var value = "myvalue";
            var expected = $"{scheme} {value}";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderAuthorization(scheme, value);

            // assert
            Assert.Equal(expected, request.Headers.Authorization.ToString());
        }

        [Fact]
        public void WithHeaderCacheControl()
        {
            // arrange
            var cacheControl = "foo-bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderCacheControl(cacheControl);

            // assert
            Assert.Equal(cacheControl, request.Headers.CacheControl.ToString());
        }

        [Fact]
        public void WithHeaderCacheControlNoCache()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderCacheControlNoCache();

            // assert
            Assert.Equal("no-cache", request.Headers.CacheControl.ToString());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        public void WithHeaderConnectionClose(bool? value)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderConnectionClose(value);

            // assert
            Assert.Equal(value, request.Headers.ConnectionClose);
        }

        [Fact]
        public void WithHeaderDate()
        {
            // arrange
            var value = DateTimeOffset.Now;
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderDate(value);

            // assert
            Assert.Equal(value, request.Headers.Date);
        }

        [Fact]
        public void WithHeaderIfMatch()
        {
            // arrange
            var value = "\"foo\"";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfMatch(value);

            // assert
            Assert.Equal(value, request.Headers.IfMatch.Single().Tag);
        }

        [Theory]
        [InlineData("\"foo\"", true)]
        [InlineData("\"foo\"", false)]
        public void WithHeaderIfMatch_isweak(string value, bool isWeak)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfMatch(value, isWeak);

            // assert
            Assert.Equal(value, request.Headers.IfMatch.Single().Tag);
            Assert.Equal(isWeak, request.Headers.IfMatch.Single().IsWeak);
        }

        [Fact]
        public void WithHeaderIfMatch_multiple()
        {
            // arrange
            var value1 = "\"foo\"";
            var value2 = "\"bar\"";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfMatch(value1).WithHeaderIfMatch(value2);

            // assert
            Assert.Equal(value1, request.Headers.IfMatch.First().Tag);
            Assert.Equal(value2, request.Headers.IfMatch.Last().Tag);
        }

        [Fact]
        public void WithHeaderIfNoneMatch()
        {
            // arrange
            var value = "\"foo\"";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfNoneMatch(value);

            // assert
            Assert.Equal(value, request.Headers.IfNoneMatch.Single().Tag);
        }

        [Theory]
        [InlineData("\"foo\"", true)]
        [InlineData("\"foo\"", false)]
        public void WithHeaderIfNoneMatch_isweak(string value, bool isWeak)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfNoneMatch(value, isWeak);

            // assert
            Assert.Equal(value, request.Headers.IfNoneMatch.Single().Tag);
            Assert.Equal(isWeak, request.Headers.IfNoneMatch.Single().IsWeak);
        }

        [Fact]
        public void WithHeaderIfNoneMatch_multiple()
        {
            // arrange
            var value1 = "\"foo\"";
            var value2 = "\"bar\"";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfNoneMatch(value1).WithHeaderIfNoneMatch(value2);

            // assert
            Assert.Equal(value1, request.Headers.IfNoneMatch.First().Tag);
            Assert.Equal(value2, request.Headers.IfNoneMatch.Last().Tag);
        }

        [Fact]
        public void WithHeaderIfRange_date()
        {
            // arrange
            var lastModified = DateTimeOffset.Now;
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfRange(lastModified);

            // assert
            Assert.Equal(lastModified, request.Headers.IfRange.Date);
        }

        [Theory]
        [InlineData("\"foo\"", "\"foo\"")]
        [InlineData("foo", "\"foo\"")]
        public void WithHeaderIfRange_entityTag(string entityTag, string expected)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfRange(entityTag);

            // assert
            Assert.Equal(expected, request.Headers.IfRange.EntityTag.ToString());
        }

        [Theory]
        [InlineData("\"foo\"", true, "W/\"foo\"")]
        [InlineData("\"foo\"", false, "\"foo\"")]
        [InlineData("foo", true, "W/\"foo\"")]
        [InlineData("foo", false, "\"foo\"")]
        public void WithHeaderIfRange_entityTag_isweak(string entityTag, bool isWeak, string expected)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfRange(entityTag, isWeak);

            // assert
            Assert.Equal(expected, request.Headers.IfRange.EntityTag.ToString());
        }

        [Fact]
        public void WithHeaderIfUnmodifiedSince()
        {
            // arrange
            var lastModified = DateTimeOffset.Now;
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfUnmodifiedSince(lastModified);

            // assert
            Assert.Equal(lastModified, request.Headers.IfUnmodifiedSince);
        }

        [Fact]
        public void WithHeaderIfModifiedSince()
        {
            // arrange
            var lastModified = DateTimeOffset.Now;
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderIfModifiedSince(lastModified);

            // assert
            Assert.Equal(lastModified, request.Headers.IfModifiedSince);
        }

        [Fact]
        public void WithHeaderPragma()
        {
            // arrange
            var pragma = "foo-bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderPragma(pragma);

            // assert
            Assert.Equal(pragma, request.Headers.Pragma.ToString());
        }

        [Fact]
        public void WithHeaderPragma_multiple()
        {
            // arrange
            var pragma1 = "foo-bar";
            var pragma2 = "foo-bar";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderPragma(pragma1).WithHeaderPragma(pragma2);

            // assert
            Assert.Equal(pragma1, request.Headers.Pragma.First().Name);
            Assert.Equal(pragma2, request.Headers.Pragma.Last().Name);
        }

        [Fact]
        public void WithHeaderPragma_name_value()
        {
            // arrange
            var pragmaName = "foo-bar";
            var pragmaValue = "fizz-buzz";
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderPragma(pragmaName, pragmaValue);

            // assert
            Assert.Equal(pragmaName, request.Headers.Pragma.Single().Name);
            Assert.Equal(pragmaValue, request.Headers.Pragma.Single().Value);
        }

        [Fact]
        public void WithHeaderPragmaNoCache()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderPragmaNoCache();

            // assert
            Assert.Equal("no-cache", request.Headers.Pragma.ToString());
        }

        [Theory]
        [InlineData(null, 2)]
        [InlineData(2, null)]
        [InlineData(1, 2)]
        [InlineData(3, long.MaxValue)]
        public void WithHeaderRange(long? from, long? to)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderRange(from, to);

            // assert
            Assert.Equal(from, request.Headers.Range.Ranges.Single().From);
            Assert.Equal(to, request.Headers.Range.Ranges.Single().To);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("product/version (comment)")]
        [InlineData("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36")]
        public void WithHeaderUserAgent(string userAgent)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderUserAgent(userAgent);

            // assert
            Assert.Equal(userAgent, request.Headers.UserAgent.ToString());
        }

        [Theory]
        [InlineData("foo", "bar", "foo/bar")]
        [InlineData("Mozilla", "5.0", "Mozilla/5.0")]
        public void WithHeaderUserAgent_product_version(string product, string version, string expected)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderUserAgent(product, version);

            // assert
            Assert.Equal(expected, request.Headers.UserAgent.ToString());
        }

        [Theory]
        [InlineData("foo", "bar", "comment", "foo/bar (comment)")]
        [InlineData("foo", "bar", "(comment)", "foo/bar (comment)")]
        [InlineData("foo", "bar", null, "foo/bar")]
        [InlineData("Mozilla", "5.0", "X11; Linux x86_64", "Mozilla/5.0 (X11; Linux x86_64)")]
        public void WithHeaderUserAgent_product_version_comment(string product, string version, string comment, string expected)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithHeaderUserAgent(product, version, comment);

            // assert
            Assert.Equal(expected, request.Headers.UserAgent.ToString());
        }

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

        [Fact]
        public void WithUriQueryParameter_string_without_RequestUri()
        {
            // arrange
            var name = "hello";
            var value = "world";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            Assert.Equal("?hello=world", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_string_with_RequestUri()
        {
            // arrange
            var name = "hello";
            var value = "world";
            var request = new HttpRequestMessage().WithUri("http://example.com");

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            Assert.Equal("http://example.com/?hello=world", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQueryParameter_object_without_RequestUri()
        {
            // arrange
            var name = "hello";
            object value = 123;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            Assert.Equal("?hello=123", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_object_with_RequestUri()
        {
            // arrange
            var name = "hello";
            object value = 123;
            var request = new HttpRequestMessage().WithUri("http://example.com");

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            Assert.Equal("http://example.com/?hello=123", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQueryParameter_string()
        {
            // arrange
            var name = "hello";
            var value = "world";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(value, queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_object()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter("hello", "wo{0}", "rld");

            // assert
            Assert.Equal("?hello=world", request.RequestUri.OriginalString);
        }

        [Theory]
        [InlineData("wor{0}", null, InclusionRule.ExcludeIfNull, null)]
        [InlineData("wor{0}", null, InclusionRule.IncludeAlways, "?hello=wor")]
        [InlineData("wor{0}", "ld", InclusionRule.ExcludeIfNull, "?hello=world")]
        [InlineData("wor{0}", "ld", InclusionRule.IncludeAlways, "?hello=world")]
        public void WithUriQueryParameter_format_object_InclusionRule(string format, object value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter("hello", format, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.RequestUri?.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_format_objectarray()
        {
            // arrange
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter("hello", "wo{0}{1}", new object[] { "rld", 1 });

            // assert
            Assert.Equal("?hello=world1", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_format_objectarray_ExcludeIfNull()
        {
            // arrange
            object[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter("hello", "wor{0}", values, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_object()
        {
            // arrange
            var name = "hello";
            object value = 123;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            Assert.Equal("?hello=123", request.RequestUri.OriginalString);
        }

        [Theory]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(null, InclusionRule.IncludeAlways, "?hello=")]
        [InlineData("world", InclusionRule.ExcludeIfNull, "?hello=world")]
        [InlineData("world", InclusionRule.IncludeAlways, "?hello=world")]
        public void WithUriQueryParameter_object_InclusionRule(object value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            Assert.Equal(expected, request.RequestUri?.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_objectarray_null()
        {
            // arrange
            var name = "hello";
            object[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableObject_null()
        {
            // arrange
            var name = "hello";
            IEnumerable<object> values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_stringarray_null()
        {
            // arrange
            var name = "hello";
            string[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableString_null()
        {
            // arrange
            var name = "hello";
            IEnumerable<string> values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_stringarray_without_RequestUri()
        {
            // arrange
            var name = "hello";
            var values = new string[] { "fizz", "buzz" };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=fizz&hello=buzz", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableString_without_RequestUri()
        {
            // arrange
            var name = "hello";
            IEnumerable<string> values = new string[] { "fizz", "buzz" };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=fizz&hello=buzz", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_objectarray_without_RequestUri()
        {
            // arrange
            var name = "hello";
            var values = new object[] { 123, 456 };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=123&hello=456", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableObject_without_RequestUri()
        {
            // arrange
            var name = "hello";
            IEnumerable<object> values = new object[] { 123, 456 };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("?hello=123&hello=456", request.RequestUri.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_stringarray_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            string[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableString_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            IEnumerable<string> values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_stringarray_with_RequestUri()
        {
            // arrange
            var name = "hello";
            var values = new string[] { "fizz", "buzz" };
            var request = new HttpRequestMessage().WithUri("http://example.com");

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("http://example.com/?hello=fizz&hello=buzz", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableString_with_RequestUri()
        {
            // arrange
            var name = "hello";
            IEnumerable<string> values = new string[] { "fizz", "buzz" };
            var request = new HttpRequestMessage().WithUri("http://example.com");

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("http://example.com/?hello=fizz&hello=buzz", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQueryParameter_objectarray_with_RequestUri()
        {
            // arrange
            var name = "hello";
            var values = new object[] { 123, 456 };
            var request = new HttpRequestMessage().WithUri("http://example.com");

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("http://example.com/?hello=123&hello=456", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableObject_with_RequestUri()
        {
            // arrange
            var name = "hello";
            IEnumerable<object> values = new object[] { 123, 456 };
            var request = new HttpRequestMessage().WithUri("http://example.com");

            // act
            request.WithUriQueryParameter(name, values);

            // assert
            Assert.Equal("http://example.com/?hello=123&hello=456", request.RequestUri.AbsoluteUri);
        }

        [Fact]
        public void WithUriQueryParameter_objectarray_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            object[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_IEnumerableObject_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            IEnumerable<object> values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Theory]
        [InlineData(null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(null, null, InclusionRule.IncludeAlways, "?hello=")]
        [InlineData(123, 456, InclusionRule.ExcludeIfNull, "?hello=123&hello=456")]
        [InlineData(123, 456, InclusionRule.IncludeAlways, "?hello=123&hello=456")]
        public void WithUriQueryParameter_objectarray_InclusionRule(object value1, object value2, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";

            object[] values = (value1 == null)
                ? null
                : new[] { value1, value2 };

            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values, inclusionRule);

            // assert
            Assert.Equal(expected, request.RequestUri?.OriginalString);
        }

        [Theory]
        [InlineData(null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(null, null, InclusionRule.IncludeAlways, "?hello=")]
        [InlineData(123, 456, InclusionRule.ExcludeIfNull, "?hello=123&hello=456")]
        [InlineData(123, 456, InclusionRule.IncludeAlways, "?hello=123&hello=456")]
        public void WithUriQueryParameter_IEnumerableObject_InclusionRule(object value1, object value2, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";

            IEnumerable<object> values = (value1 == null)
                ? null
                : new[] { value1, value2 };

            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, values, inclusionRule);

            // assert
            Assert.Equal(expected, request.RequestUri?.OriginalString);
        }

        [Fact]
        public void WithUriQueryParameter_null_string_IncludeAlways()
        {
            // arrange
            var name = "hello";
            string value = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.IncludeAlways);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal($"{value}", queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_null_string_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            string value = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_null_string_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            string value = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_empty_string_IncludeAlways()
        {
            // arrange
            var name = "hello";
            string value = string.Empty;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.IncludeAlways);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(value, queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_empty_string_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            string value = string.Empty;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.ExcludeIfNull);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(value, queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_empty_string_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            string value = string.Empty;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_string_IncludeAlways()
        {
            // arrange
            var name = "hello";
            string value = "world";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.IncludeAlways);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(value, queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_string_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            string value = "world";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.ExcludeIfNull);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(value, queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_string_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            string value = "world";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(value, queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_object_IncludeAlways()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object value = 123;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, value, InclusionRule.IncludeAlways);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, value), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_object_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object value = 123;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, value, InclusionRule.ExcludeIfNull);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, value), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_object_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object value = 123;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, value, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, value), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_null_object_IncludeAlways()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object value = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, value, InclusionRule.IncludeAlways);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, value), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_null_object_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object value = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, value, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_format_null_object_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object value = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, value, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_format_object_array_IncludeAlways()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            var values = new object[] { 123 };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, values, InclusionRule.IncludeAlways);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, values), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_object_array_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            var values = new object[] { 123 };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, values, InclusionRule.ExcludeIfNull);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, values), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_object_array_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            var values = new object[] { 123 };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, values, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(string.Format(format, values), queryParameterValue);
        }

        [Fact]
        public void WithUriQueryParameter_format_null_object_array_IncludeAlways()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object[] values = null;
            var request = new HttpRequestMessage();

            // act & assert
            Assert.Throws<ArgumentNullException>(() => request.WithUriQueryParameter(name, format, values, InclusionRule.IncludeAlways));
        }

        [Fact]
        public void WithUriQueryParameter_format_null_object_array_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, values, InclusionRule.ExcludeIfNull);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_format_null_object_array_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object[] values = null;
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, values, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Fact]
        public void WithUriQueryParameter_format_empty_object_array_IncludeAlways()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object[] values = { };
            var request = new HttpRequestMessage();

            // act & assert
            Assert.Throws<FormatException>(() => request.WithUriQueryParameter(name, format, values, InclusionRule.IncludeAlways));
        }

        [Fact]
        public void WithUriQueryParameter_format_empty_object_array_ExcludeIfNull()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object[] values = { };
            var request = new HttpRequestMessage();

            // act & assert
            Assert.Throws<FormatException>(() => request.WithUriQueryParameter(name, format, values, InclusionRule.IncludeAlways));
        }

        [Fact]
        public void WithUriQueryParameter_format_empty_object_array_ExcludeIfNullOrEmpty()
        {
            // arrange
            var name = "hello";
            var format = "{0}";
            object[] values = { };
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, format, values, InclusionRule.ExcludeIfNullOrEmpty);

            // assert
            Assert.Null(request.RequestUri);
        }

        [Theory]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void WithUriQueryParameter_bool(bool value, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void WithUriQueryParameter_nullable_bool(bool? value, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(true, BooleanFormatting.Default, "True")]
        [InlineData(true, BooleanFormatting.Lower, "true")]
        [InlineData(false, BooleanFormatting.Lower, "false")]
        public void WithUriQueryParameter_bool_BooleanFormatting(bool value, BooleanFormatting formatting, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, BooleanFormatting.Default, "")]
        [InlineData(true, BooleanFormatting.Default, "True")]
        [InlineData(true, BooleanFormatting.Lower, "true")]
        [InlineData(false, BooleanFormatting.Lower, "false")]
        public void WithUriQueryParameter_nullable_bool_BooleanFormatting(bool? value, BooleanFormatting formatting, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(true, InclusionRule.IncludeAlways, "True")]
        [InlineData(false, InclusionRule.IncludeAlways, "False")]
        [InlineData(true, InclusionRule.ExcludeIfNull, "True")]
        [InlineData(true, InclusionRule.ExcludeIfNullOrEmpty, "True")]
        public void WithUriQueryParameter_bool_InclusionRule(bool value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, InclusionRule.IncludeAlways, "")]
        [InlineData(true, InclusionRule.IncludeAlways, "True")]
        [InlineData(false, InclusionRule.IncludeAlways, "False")]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(true, InclusionRule.ExcludeIfNull, "True")]
        [InlineData(null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData(true, InclusionRule.ExcludeIfNullOrEmpty, "True")]
        public void WithUriQueryParameter_nullable_bool_InclusionRule(bool? value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(true, BooleanFormatting.Default, InclusionRule.IncludeAlways, "True")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "true")]
        [InlineData(false, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "false")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.ExcludeIfNull, "true")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.ExcludeIfNullOrEmpty, "true")]
        public void WithUriQueryParameter_bool_InclusionRule_BooleanFormatting(bool value, BooleanFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting, inclusionRule);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, BooleanFormatting.Default, InclusionRule.IncludeAlways, "")]
        [InlineData(true, BooleanFormatting.Default, InclusionRule.IncludeAlways, "True")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "true")]
        [InlineData(false, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "false")]
        [InlineData(null, BooleanFormatting.Lower, InclusionRule.ExcludeIfNull, null)]
        [InlineData(null, BooleanFormatting.Lower, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithUriQueryParameter_nullable_bool_InclusionRule_BooleanFormatting(bool? value, BooleanFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, "2020-07-04")]
        public void WithUriQueryParameter_DateTime(int year, int month, int day, string expected)
        {
            // arrange
            var name = "hello";
            var value = new DateTime(year, month, day);
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(2020, 7, 4, "2020-07-04")]
        [InlineData(null, null, null, "")]
        public void WithUriQueryParameter_nullable_DateTime(int? year, int? month, int? day, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithUriQueryParameter_DateTime_DateTimeFormatting(int year, int month, int day, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = new DateTime(year, month, day);
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, null, null, DateTimeFormatting.Default, "")]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithUriQueryParameter_nullable_DateTime_DateTimeFormatting(int? year, int? month, int? day, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNull, "2020-07-04")]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04")]
        public void WithUriQueryParameter_DateTime_InclusionRule(int year, int month, int day, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTime(year, month, day);
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(2020, 7, 4, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(null, null, null, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNull, "2020-07-04")]
        [InlineData(null, null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04")]
        [InlineData(null, null, null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithUriQueryParameter_nullable_DateTime_InclusionRule(int? year, int? month, int? day, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200705")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, "20200704")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, "20200704")]
        public void WithUriQueryParameter_DateTime_InclusionRule_DateTimeFormatting(int year, int month, int day, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTime(year, month, day);
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting, inclusionRule);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, null, null, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200705")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, "20200704")]
        [InlineData(null, null, null, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, "20200704")]
        [InlineData(null, null, null, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithUriQueryParameter_nullable_DateTime_InclusionRule_DateTimeFormatting(int? year, int? month, int? day, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, "2020-07-04T00:00:00.0000000+00:00")]
        public void WithUriQueryParameter_DateTimeOffset(int year, int month, int day, int offset, string expected)
        {
            // arrange
            var name = "hello";
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, "")]
        public void WithUriQueryParameter_nullable_DateTimeOffset(int? year, int? month, int? day, int? offset, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithUriQueryParameter_DateTimeOffset_DateTimeFormatting(int year, int month, int day, int offset, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, null, null, null, DateTimeFormatting.Default, "")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithUriQueryParameter_nullable_DateTimeOffset_DateTimeFormatting(int? year, int? month, int? day, int? offset, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNull, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04T00:00:00.0000000+00:00")]
        public void WithUriQueryParameter_DateTimeOffset_InclusionRule(int year, int month, int day, int offset, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNull, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithUriQueryParameter_nullable_DateTimeOffset_InclusionRule(int? year, int? month, int? day, int? offset, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200705")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, "20200704")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, "20200704")]
        public void WithUriQueryParameter_DateTimeOffset_InclusionRule_DateTimeFormatting(int year, int month, int day, int offset, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting, inclusionRule);

            // assert
            var query = Url.GetQuery(request.RequestUri.OriginalString);
            var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
            Assert.Equal(expected, queryParameterValue);
        }

        [Theory]
        [InlineData(null, null, null, null, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200705")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, "20200704")]
        [InlineData(null, null, null, null, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, "20200704")]
        [InlineData(null, null, null, null, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithUriQueryParameter_nullable_DateTimeOffset_InclusionRule_DateTimeFormatting(int? year, int? month, int? day, int? offset, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var request = new HttpRequestMessage();

            // act
            request.WithUriQueryParameter(name, value, formatting, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Null(request.RequestUri);
            }
            else
            {
                var queryParameterValue = Url.GetQueryValue(request.RequestUri.OriginalString, name);
                Assert.Equal(expected, queryParameterValue);
            }
        }

        [Fact]
        public async Task WithJsonContent_object()
        {
            // arrange
            string someValue = "myvalue";
            var value = new { mykey = someValue };
            var request = new HttpRequestMessage();

            // act
            request.WithJsonContent(value);

            // assert
            var content = Assert.IsType<StringContent>(request.Content);
            var contentAsString = await content.ReadAsStringAsync();
            var anonymousPrototype = new { mykey = default(string) };
            var deserialized = JsonConvert.DeserializeAnonymousType(contentAsString, anonymousPrototype);
            Assert.Equal(someValue, deserialized.mykey);
        }

        [Fact]
        public async Task WithJsonContent_string()
        {
            // arrange
            string someValue = "myvalue";
            var value = "{\"mykey\":\"myvalue\"}";
            var request = new HttpRequestMessage();

            // act
            request.WithJsonContent(value);

            // assert
            var content = Assert.IsType<StringContent>(request.Content);
            var contentAsString = await content.ReadAsStringAsync();
            var anonymousPrototype = new { mykey = default(string) };
            var deserialized = JsonConvert.DeserializeAnonymousType(contentAsString, anonymousPrototype);
            Assert.Equal(someValue, deserialized.mykey);
        }

        [Fact]
        public async Task WithUrlEncodedFormContent_builder()
        {
            // arrange
            string someName = "mykey";
            string someValue = "myvalue";
            var request = new HttpRequestMessage();

            // act
            request.WithUrlEncodedFormContent(content =>
            {
                content.WithValue(someName, someValue);
            });

            // assert
            var content = Assert.IsType<FormUrlEncodedContent>(request.Content);
            var contentAsString = await content.ReadAsStringAsync();
            var contentAsDictionary = QueryString.Deserialize(contentAsString);
            Assert.Equal(someValue, contentAsDictionary[someName].Single());
        }

        [Fact]
        public async Task WithUrlEncodedFormContent_object()
        {
            // arrange
            string someName = "mykey";
            string someValue = "myvalue";
            var value = new { mykey = someValue };
            var request = new HttpRequestMessage();

            // act
            request.WithUrlEncodedFormContent(value);

            // assert
            var content = Assert.IsType<StringContent>(request.Content);
            var contentAsString = await content.ReadAsStringAsync();
            var contentAsDictionary = QueryString.Deserialize(contentAsString);
            Assert.Equal(someValue, contentAsDictionary[someName].Single());
        }

        [Fact]
        public async Task WithUrlEncodedFormContent_string()
        {
            // arrange
            string someName = "mykey";
            string someValue = "myvalue";
            var value = QueryString.Serialize(new { mykey = someValue });
            var request = new HttpRequestMessage();

            // act
            request.WithUrlEncodedFormContent(value);

            // assert
            var content = Assert.IsType<StringContent>(request.Content);
            var contentAsString = await content.ReadAsStringAsync();
            var contentAsDictionary = QueryString.Deserialize(contentAsString);
            Assert.Equal(someValue, contentAsDictionary[someName].Single());
        }

        [Fact]
        public async Task WithUrlEncodedFormContent_FormUrlEncodedContent()
        {
            // arrange
            string someName = "mykey";
            string someValue = "myvalue";
            var value = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>(someName, someValue) });
            var request = new HttpRequestMessage();

            // act
            request.WithUrlEncodedFormContent(value);

            // assert
            var content = Assert.IsType<FormUrlEncodedContent>(request.Content);
            var contentAsString = await content.ReadAsStringAsync();
            var contentAsDictionary = QueryString.Deserialize(contentAsString);
            Assert.Equal(someValue, contentAsDictionary[someName].Single());
        }

        [Fact]
        public async Task WithStringContent_with_media_type()
        {
            // arrange
            string mediaType = "my/mediatype";
            var value = "hello world";
            var request = new HttpRequestMessage();

            // act
            request.WithStringContent(value, mediaType);

            // assert
            var content = Assert.IsType<StringContent>(request.Content);
            Assert.Equal(content.Headers.ContentType.MediaType, mediaType);

            var contentAsString = await content.ReadAsStringAsync();
            Assert.Equal(contentAsString, value);
        }

        [Fact]
        public async Task WithStringContent()
        {
            // arrange
            var value = "hello world";
            var request = new HttpRequestMessage();

            // act
            request.WithStringContent(value);

            // assert
            var content = Assert.IsType<StringContent>(request.Content);
            Assert.Equal("text/plain", content.Headers.ContentType.MediaType);

            var contentAsString = await content.ReadAsStringAsync();
            Assert.Equal(contentAsString, value);
        }

        [Fact]
        public async Task WithByteArrayContent_with_media_type()
        {
            // arrange
            string mediaType = "my/mediatype";
            var value = "hello world";
            var bytes = Encoding.UTF8.GetBytes(value);
            var request = new HttpRequestMessage();

            // act
            request.WithByteArrayContent(bytes, mediaType);

            // assert
            var content = Assert.IsType<ByteArrayContent>(request.Content);
            Assert.Equal(content.Headers.ContentType.MediaType, mediaType);

            var contentAsString = await content.ReadAsStringAsync();
            Assert.Equal(contentAsString, value);
        }

        [Fact]
        public async Task WithByteArrayContent()
        {
            // arrange
            var value = "hello world";
            var bytes = Encoding.UTF8.GetBytes(value);
            var request = new HttpRequestMessage();

            // act
            request.WithByteArrayContent(bytes);

            // assert
            var content = Assert.IsType<ByteArrayContent>(request.Content);
            Assert.Null(content.Headers.ContentType);

            var contentAsString = await content.ReadAsStringAsync();
            Assert.Equal(contentAsString, value);
        }

        [Fact]
        public async Task WithStreamContent_with_media_type()
        {
            // arrange
            string mediaType = "my/mediatype";
            var value = "hello world";
            var bytes = Encoding.UTF8.GetBytes(value);
            using (var memoryStream = new MemoryStream(bytes))
            {
                var request = new HttpRequestMessage();

                // act
                request.WithStreamContent(memoryStream, mediaType);

                // assert
                var content = Assert.IsType<StreamContent>(request.Content);
                Assert.Equal(content.Headers.ContentType.MediaType, mediaType);

                var contentAsString = await content.ReadAsStringAsync();
                Assert.Equal(contentAsString, value);
            }
        }

        [Fact]
        public async Task WithStreamContent()
        {
            // arrange
            var value = "hello world";
            var bytes = Encoding.UTF8.GetBytes(value);
            var request = new HttpRequestMessage();
            using (var memoryStream = new MemoryStream(bytes))
            {
                // act
                request.WithStreamContent(memoryStream);

                // assert
                var content = Assert.IsType<StreamContent>(request.Content);
                Assert.Null(content.Headers.ContentType);

                var contentAsString = await content.ReadAsStringAsync();
                Assert.Equal(contentAsString, value);
            }
        }

        [Fact]
        public async Task WithMultipartFormDataContent_builder()
        {
            // arrange
            string someName = "mykey";
            string someValue = "myvalue";
            var request = new HttpRequestMessage();

            // act
            request.WithMultipartFormDataContent(content =>
            {
                content.WithTextContent(someName, someValue);
            });

            // assert
            var content = Assert.IsType<MultipartFormDataContent>(request.Content);
            var stringContent = content.Single();
            var stringContentName = stringContent.Headers.ContentDisposition.Name;
            Assert.Equal(someName, stringContentName);

            var contentAsString = await stringContent.ReadAsStringAsync();
            Assert.Equal(someValue, contentAsString);
        }

        [Fact]
        public async Task WithMultipartFormDataContent()
        {
            // arrange
            string someName = "mykey";
            string someValue = "myvalue";
            var request = new HttpRequestMessage();

            // act
            request.WithMultipartFormDataContent(new MultipartFormDataContent().WithTextContent(someName, someValue));

            // assert
            var content = Assert.IsType<MultipartFormDataContent>(request.Content);
            var stringContent = content.Single();
            var stringContentName = stringContent.Headers.ContentDisposition.Name;
            Assert.Equal(someName, stringContentName);

            var contentAsString = await stringContent.ReadAsStringAsync();
            Assert.Equal(someValue, contentAsString);
        }
    }
}
