using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.UrlHelper;
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
        public void WithValue_nullable_bool(bool? value, string expected)
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
        public void WithValue_nullable_DateTime(int? year, int? month, int? day, string expected)
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
        [InlineData(2020, 7, 4, 0, "2020-07-04")]
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
        [InlineData(2020, 7, 4, 0, "2020-07-04")]
        [InlineData(null, null, null, null, "")]
        public void WithValue_nullable_DateTimeOffset(int? year, int? month, int? day, int? offset, string expected)
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
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, "2020-07-04")]
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
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, "2020-07-04")]
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
        [InlineData(2020, 7, 4, 0, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNull, "2020-07-04")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04")]
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
        [InlineData(2020, 7, 4, 0, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(null, null, null, null, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNull, "2020-07-04")]
        [InlineData(null, null, null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04")]
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
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04")]
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
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04")]
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
    }
}
