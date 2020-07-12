using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class DateTimeFormattingHelperTests
    {
        [Theory]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.Default, "2020-07-04")]
        [InlineData(2020, 07, 04, 6, DateTimeFormatting.Default, "2020-07-04T06:00:00.0000000")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.ISO, "2020-07-04T00:00:00.0000000")]
        [InlineData(2020, 07, 04, 6, DateTimeFormatting.ISO, "2020-07-04T06:00:00.0000000")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.UnixTime, "1593820800")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.UnixTimeMilliseconds, "1593820800000")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MMDDYY, "070420")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MMDDYYYY, "07042020")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MM_DD_YY, "07-04-20")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MM_DD_YYYY, "07-04-2020")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYMMDD, "200704")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYYY_MM_DD, "2020-07-04")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YY_MM_DD, "20-07-04")]
        public void Format_with_DateTime(int year, int month, int day, int hours, DateTimeFormatting formatting, string expected)
        {
            // arrange
            DateTime input = new DateTime(year, month, day, hours, 0, 0);

            // act
            var actual = DateTimeFormattingHelper.Format(input, formatting);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null, null, DateTimeFormatting.Default, null)]
        [InlineData(2020, 07, 04, DateTimeFormatting.Default, "2020-07-04")]
        [InlineData(2020, 07, 04, DateTimeFormatting.ISO, "2020-07-04T00:00:00.0000000")]
        [InlineData(2020, 07, 04, DateTimeFormatting.YYYY_MM_DD, "2020-07-04")]
        [InlineData(2020, 07, 04, DateTimeFormatting.YY_MM_DD, "20-07-04")]
        public void Format_with_nullable_DateTime(int? year, int? month, int? day, DateTimeFormatting formatting, string expected)
        {
            // arrange
            DateTime? input = year.HasValue
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            // act
            var actual = DateTimeFormattingHelper.Format(input, formatting);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 07, 04, 6, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+06:00")]
        [InlineData(2020, 07, 04, -6, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000-06:00")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.ISO, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 07, 04, 6, DateTimeFormatting.ISO, "2020-07-04T00:00:00.0000000+06:00")]
        [InlineData(2020, 07, 04, -6, DateTimeFormatting.ISO, "2020-07-04T00:00:00.0000000-06:00")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.UnixTime, "1593820800")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.UnixTimeMilliseconds, "1593820800000")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MMDDYY, "070420")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MMDDYYYY, "07042020")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MM_DD_YY, "07-04-20")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.MM_DD_YYYY, "07-04-2020")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYMMDD, "200704")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYYY_MM_DD, "2020-07-04")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YY_MM_DD, "20-07-04")]
        public void Format_with_DateTimeOffset(int year, int month, int day, int offset, DateTimeFormatting formatting, string expected)
        {
            // arrange
            DateTimeOffset input = new DateTimeOffset(year, month, day, 0, 0, 0, 0, TimeSpan.FromHours(offset));

            // act
            var actual = DateTimeFormattingHelper.Format(input, formatting);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null, null, null, DateTimeFormatting.Default, null)]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.ISO, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YYYY_MM_DD, "2020-07-04")]
        [InlineData(2020, 07, 04, 0, DateTimeFormatting.YY_MM_DD, "20-07-04")]
        public void Format_with_nullable_DateTimeOffset(int? year, int? month, int? day, int? offset, DateTimeFormatting formatting, string expected)
        {
            // arrange
            DateTimeOffset? input = year.HasValue
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            // act
            var actual = DateTimeFormattingHelper.Format(input, formatting);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
