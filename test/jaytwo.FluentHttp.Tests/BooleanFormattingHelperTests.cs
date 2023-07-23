using System;
using System.Collections.Generic;
using System.Text;
using jaytwo.FluentHttp.Formatting;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class BooleanFormattingHelperTests
    {
        [Theory]
        [InlineData(true, BooleanFormatting.Default, "True")]
        [InlineData(false, BooleanFormatting.Default, "False")]
        [InlineData(true, BooleanFormatting.Lower, "true")]
        [InlineData(false, BooleanFormatting.Lower, "false")]
        [InlineData(true, BooleanFormatting.OneZero, "1")]
        [InlineData(false, BooleanFormatting.OneZero, "0")]
        [InlineData(true, BooleanFormatting.TF, "T")]
        [InlineData(false, BooleanFormatting.TF, "F")]
        [InlineData(true, BooleanFormatting.TFLower, "t")]
        [InlineData(false, BooleanFormatting.TFLower, "f")]
        [InlineData(true, BooleanFormatting.Upper, "TRUE")]
        [InlineData(false, BooleanFormatting.Upper, "FALSE")]
        [InlineData(true, BooleanFormatting.YesNo, "Yes")]
        [InlineData(false, BooleanFormatting.YesNo, "No")]
        [InlineData(true, BooleanFormatting.YesNoLower, "yes")]
        [InlineData(false, BooleanFormatting.YesNoLower, "no")]
        [InlineData(true, BooleanFormatting.YesNoUpper, "YES")]
        [InlineData(false, BooleanFormatting.YesNoUpper, "NO")]
        [InlineData(true, BooleanFormatting.YN, "Y")]
        [InlineData(false, BooleanFormatting.YN, "N")]
        [InlineData(true, BooleanFormatting.YNLower, "y")]
        [InlineData(false, BooleanFormatting.YNLower, "n")]
        public void Format_with_bool(bool input, BooleanFormatting formatting, string expected)
        {
            // arrange

            // act
            var actual = BooleanFormattingHelper.Format(input, formatting);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, BooleanFormatting.Default, null)]
        [InlineData(true, BooleanFormatting.Default, "True")]
        [InlineData(true, BooleanFormatting.Lower, "true")]
        public void Format_with_nullable_bool(bool? input, BooleanFormatting formatting, string expected)
        {
            // arrange

            // act
            var actual = BooleanFormattingHelper.Format(input, formatting);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
