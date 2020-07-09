using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class InclusionRuleHelperTests
    {
        [Theory]
        [InlineData(default(string), InclusionRule.IncludeAlways, true)]
        [InlineData(default(string), InclusionRule.ExcludeIfNull, false)]
        [InlineData(default(string), InclusionRule.ExcludeIfNullOrEmpty, false)]
        [InlineData("", InclusionRule.IncludeAlways, true)]
        [InlineData("", InclusionRule.ExcludeIfNull, true)]
        [InlineData("", InclusionRule.ExcludeIfNullOrEmpty, false)]
        [InlineData("a", InclusionRule.IncludeAlways, true)]
        [InlineData("a", InclusionRule.ExcludeIfNull, true)]
        [InlineData("a", InclusionRule.ExcludeIfNullOrEmpty, true)]
        public void IncludeContent_string(object value, InclusionRule inclusionRule, bool expected)
        {
            // arrange

            // act
            var actual = InclusionRuleHelper.IncludeContent(value, inclusionRule);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
