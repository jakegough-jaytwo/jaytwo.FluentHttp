using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class ContentTypeEvaluatorTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("hello world", false)]
        [InlineData("{ \"hello\":\"world\" }", true)]
        [InlineData(" { \"hello\":\"world\" }", true)]
        [InlineData("{ \"hello\":\"world\" } ", true)]
        [InlineData("\n { \"hello\":\"world\" } \n ", true)]
        [InlineData("\n [ \"hello\", \"world\" ] \n ", true)]
        [InlineData("[]", true)]
        [InlineData("{}", true)]
        public void CouldBeJsonString_works(string input, bool expected)
        {
            // arrange

            // act
            var actual = ContentTypeEvaluator.CouldBeJsonString(input);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("hello/world", false)]
        [InlineData("text/plain", false)]
        [InlineData("text/json", true)]
        [InlineData("application/json", true)]
        [InlineData("application/hal+json", true)]
        [InlineData("application/json+hal", false)]
        public void IsJsonContent_works(string input, bool expected)
        {
            // arrange

            // act
            var actual = ContentTypeEvaluator.IsJsonMediaType(input);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsJsonContent_null_MediaTypeHeaderValue()
        {
            // arrange
            var value = default(MediaTypeHeaderValue);
            var expected = false;

            // act
            var actual = ContentTypeEvaluator.IsJsonMediaType(value);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("hello/world", false)]
        [InlineData("text/plain", false)]
        [InlineData("text/json", false)]
        [InlineData("application/json", false)]
        [InlineData("application/octet-stream", true)]
        [InlineData("image/jpeg", true)]
        [InlineData("image/foo", true)]
        [InlineData("audio/mp3", true)]
        [InlineData("audio/foo", true)]
        [InlineData("video/mp4", true)]
        [InlineData("video/foo", true)]
        [InlineData("font/woff", true)]
        [InlineData("font/foo", true)]
        [InlineData("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", true)]
        [InlineData("application/vnd.ms-excel", true)]
        [InlineData("font/zip", true)]
        [InlineData("foo/zip", true)]
        [InlineData("application/pdf", true)]
        [InlineData("application/x-pdf", true)]
        [InlineData("foo/pdf", true)]
        [InlineData("foo/x-pdf", true)]
        [InlineData("application/x-7z-compressed", true)]
        [InlineData("application/foo-compressed", true)]
        public void IsBinaryMediaType_works(string input, bool expected)
        {
            // arrange

            // act
            var actual = ContentTypeEvaluator.IsBinaryMediaType(input);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsBinaryMediaType_null_MediaTypeHeaderValue()
        {
            // arrange
            var value = default(MediaTypeHeaderValue);
            var expected = false;

            // act
            var actual = ContentTypeEvaluator.IsBinaryMediaType(value);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
