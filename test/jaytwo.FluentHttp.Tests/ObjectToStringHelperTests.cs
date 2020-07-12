using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class ObjectToStringHelperTests
    {
        [Fact]
        public void GetString_null()
        {
            // arrange
            object value = null;
            object expected = null;

            // act
            var actual = ObjectToStringHelper.GetString(value);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetString_DateTime()
        {
            // arrange
            DateTime value = DateTime.Now;
            var expected = DateTimeFormattingHelper.Format(value, DateTimeFormatting.Default);

            // act
            var actual = ObjectToStringHelper.GetString(value);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetString_DateTimeOffset()
        {
            // arrange
            DateTimeOffset value = DateTimeOffset.Now;
            var expected = DateTimeFormattingHelper.Format(value, DateTimeFormatting.Default);

            // act
            var actual = ObjectToStringHelper.GetString(value);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetString_Bool()
        {
            // arrange
            bool value = true;
            var expected = BooleanFormattingHelper.Format(value, BooleanFormatting.Default);

            // act
            var actual = ObjectToStringHelper.GetString(value);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetString_random_object()
        {
            // arrange
            object value = new { foo = "bar" };

            // act
            var actual = ObjectToStringHelper.GetString(value);

            // assert
            Assert.NotNull(actual);
        }
    }
}
