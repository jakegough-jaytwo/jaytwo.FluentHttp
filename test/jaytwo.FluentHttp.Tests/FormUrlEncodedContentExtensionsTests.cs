using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jaytwo.FluentHttp.Formatting;
using Xunit;

namespace jaytwo.FluentHttp.Tests
{
    public class FormUrlEncodedContentExtensionsTests
    {
        [Theory]
        [InlineData("abc", "abc")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void WithValue_string(string value, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData("abc", InclusionRule.IncludeAlways, "abc")]
        [InlineData("", InclusionRule.IncludeAlways, "")]
        [InlineData(null, InclusionRule.IncludeAlways, "")]
        [InlineData("abc", InclusionRule.ExcludeIfNull, "abc")]
        [InlineData("", InclusionRule.ExcludeIfNull, "")]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData("abc", InclusionRule.ExcludeIfNullOrEmpty, "abc")]
        [InlineData("", InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData(null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_string_InclusionRule(string value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(123, "123")]
        [InlineData("abc", "abc")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void WithValue_object(object value, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(123, InclusionRule.IncludeAlways, "123")]
        [InlineData("abc", InclusionRule.IncludeAlways, "abc")]
        [InlineData("", InclusionRule.IncludeAlways, "")]
        [InlineData(null, InclusionRule.IncludeAlways, "")]
        [InlineData(123, InclusionRule.ExcludeIfNull, "123")]
        [InlineData("abc", InclusionRule.ExcludeIfNull, "abc")]
        [InlineData("", InclusionRule.ExcludeIfNull, "")]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(123, InclusionRule.ExcludeIfNullOrEmpty, "123")]
        [InlineData("abc", InclusionRule.ExcludeIfNullOrEmpty, "abc")]
        [InlineData("", InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData(null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_object_InclusionRule(object value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData("#{0}", 123, "#123")]
        [InlineData("#{0}", "abc", "#abc")]
        [InlineData("#{0}", "", "#")]
        [InlineData("#{0}", null, "#")]
        public void WithValue_format_object(string format, object value, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData("#{0}", 123, InclusionRule.IncludeAlways, "#123")]
        [InlineData("#{0}", "abc", InclusionRule.IncludeAlways, "#abc")]
        [InlineData("#{0}", "", InclusionRule.IncludeAlways, "#")]
        [InlineData("#{0}", null, InclusionRule.IncludeAlways, "#")]
        [InlineData("#{0}", 123, InclusionRule.ExcludeIfNull, "#123")]
        [InlineData("#{0}", "abc", InclusionRule.ExcludeIfNull, "#abc")]
        [InlineData("#{0}", "", InclusionRule.ExcludeIfNull, "#")]
        [InlineData("#{0}", null, InclusionRule.ExcludeIfNull, null)]
        [InlineData("#{0}", 123, InclusionRule.ExcludeIfNullOrEmpty, "#123")]
        [InlineData("#{0}", "abc", InclusionRule.ExcludeIfNullOrEmpty, "#abc")]
        [InlineData("#{0}", "", InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData("#{0}", null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_format_object_InclusionRule(string format, object value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData("#{0}.{1}", 123, 456, "#123.456")]
        [InlineData("#{0}.{1}", 123, "abc", "#123.abc")]
        public void WithValue_format_object_array(string format, object value1, object value2, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, new[] { value1, value2 });

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Fact]
        public void WithValue_format_null_object_array()
        {
            // arrange
            var name = "hello";
            var format = "foo";
            object[] value = null;
            var expected = "foo";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Fact]
        public void WithValue_format_empty_object_array()
        {
            // arrange
            var name = "hello";
            var format = "foo";
            object[] value = { };
            var expected = "foo";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData("#{0}.{1}", 123, 456, InclusionRule.IncludeAlways, "#123.456")]
        [InlineData("#{0}.{1}", 123, "abc", InclusionRule.IncludeAlways, "#123.abc")]
        [InlineData("#{0}.{1}", 123, 456, InclusionRule.ExcludeIfNull, "#123.456")]
        [InlineData("#{0}.{1}", 123, "abc", InclusionRule.ExcludeIfNull, "#123.abc")]
        [InlineData("#{0}.{1}", 123, 456, InclusionRule.ExcludeIfNullOrEmpty, "#123.456")]
        [InlineData("#{0}.{1}", 123, "abc", InclusionRule.ExcludeIfNullOrEmpty, "#123.abc")]
        public void WithValue_format_object_array_InclusionRule(string format, object value1, object value2, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, new[] { value1, value2 }, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(InclusionRule.IncludeAlways, "foo")]
        [InlineData(InclusionRule.ExcludeIfNull, null)]
        [InlineData(InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_format_null_object_array_InclusionRule(InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var format = "foo";
            object[] value = null;
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(InclusionRule.IncludeAlways, "foo")]
        [InlineData(InclusionRule.ExcludeIfNull, "foo")]
        [InlineData(InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_format_empty_object_array_InclusionRule(InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var format = "foo";
            object[] value = { };
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, format, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void WithValue_bool(bool value, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void WithValue_nullable_bool(bool? value, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(true, BooleanFormatting.Default, "True")]
        [InlineData(true, BooleanFormatting.Lower, "true")]
        [InlineData(false, BooleanFormatting.Lower, "false")]
        public void WithValue_bool_BooleanFormatting(bool value, BooleanFormatting formatting, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(null, BooleanFormatting.Default, "")]
        [InlineData(true, BooleanFormatting.Default, "True")]
        [InlineData(true, BooleanFormatting.Lower, "true")]
        [InlineData(false, BooleanFormatting.Lower, "false")]
        public void WithValue_nullable_bool_BooleanFormatting(bool? value, BooleanFormatting formatting, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(true, InclusionRule.IncludeAlways, "True")]
        [InlineData(false, InclusionRule.IncludeAlways, "False")]
        [InlineData(true, InclusionRule.ExcludeIfNull, "True")]
        [InlineData(true, InclusionRule.ExcludeIfNullOrEmpty, "True")]
        public void WithValue_bool_InclusionRule(bool value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(null, InclusionRule.IncludeAlways, "")]
        [InlineData(true, InclusionRule.IncludeAlways, "True")]
        [InlineData(false, InclusionRule.IncludeAlways, "False")]
        [InlineData(null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(true, InclusionRule.ExcludeIfNull, "True")]
        [InlineData(null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        [InlineData(true, InclusionRule.ExcludeIfNullOrEmpty, "True")]
        public void WithValue_nullable_bool_InclusionRule(bool? value, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(true, BooleanFormatting.Default, InclusionRule.IncludeAlways, "True")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "true")]
        [InlineData(false, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "false")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.ExcludeIfNull, "true")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.ExcludeIfNullOrEmpty, "true")]
        public void WithValue_bool_InclusionRule_BooleanFormatting(bool value, BooleanFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting, inclusionRule);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(null, BooleanFormatting.Default, InclusionRule.IncludeAlways, "")]
        [InlineData(true, BooleanFormatting.Default, InclusionRule.IncludeAlways, "True")]
        [InlineData(true, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "true")]
        [InlineData(false, BooleanFormatting.Lower, InclusionRule.IncludeAlways, "false")]
        [InlineData(null, BooleanFormatting.Lower, InclusionRule.ExcludeIfNull, null)]
        [InlineData(null, BooleanFormatting.Lower, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_nullable_bool_InclusionRule_BooleanFormatting(bool? value, BooleanFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, "2020-07-04")]
        public void WithValue_DateTime(int year, int month, int day, string expected)
        {
            // arrange
            var name = "hello";
            var value = new DateTime(year, month, day);
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
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
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithValue_DateTime_DateTimeFormatting(int year, int month, int day, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = new DateTime(year, month, day);
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(null, null, null, DateTimeFormatting.Default, "")]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithValue_nullable_DateTime_DateTimeFormatting(int? year, int? month, int? day, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNull, "2020-07-04")]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04")]
        public void WithValue_DateTime_InclusionRule(int year, int month, int day, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTime(year, month, day);
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(2020, 7, 4, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(null, null, null, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNull, "2020-07-04")]
        [InlineData(null, null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04")]
        [InlineData(null, null, null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_nullable_DateTime_InclusionRule(int? year, int? month, int? day, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200704")]
        [InlineData(2020, 7, 5, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200705")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, "20200704")]
        [InlineData(2020, 7, 4, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, "20200704")]
        public void WithValue_DateTime_InclusionRule_DateTimeFormatting(int year, int month, int day, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTime(year, month, day);
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting, inclusionRule);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
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
        public void WithValue_nullable_DateTime_InclusionRule_DateTimeFormatting(int? year, int? month, int? day, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTime(year.Value, month.Value, day.Value)
                : default(DateTime?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, "2020-07-04T00:00:00.0000000+00:00")]
        public void WithValue_DateTimeOffset(int year, int month, int day, int offset, string expected)
        {
            // arrange
            var name = "hello";
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, "")]
        public void WithValue_nullable_DateTimeOffset(int? year, int? month, int? day, int? offset, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithValue_DateTimeOffset_DateTimeFormatting(int year, int month, int day, int offset, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(null, null, null, null, DateTimeFormatting.Default, "")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, "20200705")]
        public void WithValue_nullable_DateTimeOffset_DateTimeFormatting(int? year, int? month, int? day, int? offset, DateTimeFormatting formatting, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNull, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04T00:00:00.0000000+00:00")]
        public void WithValue_DateTimeOffset_InclusionRule(int year, int month, int day, int offset, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, InclusionRule.IncludeAlways, "")]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNull, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, InclusionRule.ExcludeIfNull, null)]
        [InlineData(2020, 7, 4, 0, InclusionRule.ExcludeIfNullOrEmpty, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(null, null, null, null, InclusionRule.ExcludeIfNullOrEmpty, null)]
        public void WithValue_nullable_DateTimeOffset_InclusionRule(int? year, int? month, int? day, int? offset, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }

        [Theory]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.Default, InclusionRule.IncludeAlways, "2020-07-04T00:00:00.0000000+00:00")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200704")]
        [InlineData(2020, 7, 5, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.IncludeAlways, "20200705")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNull, "20200704")]
        [InlineData(2020, 7, 4, 0, DateTimeFormatting.YYYYMMDD, InclusionRule.ExcludeIfNullOrEmpty, "20200704")]
        public void WithValue_DateTimeOffset_InclusionRule_DateTimeFormatting(int year, int month, int day, int offset, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(offset));
            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting, inclusionRule);

            // assert
            Assert.NotEmpty(form);

            var singleContent = form.Single();
            Assert.Equal(name, singleContent.Key);
            Assert.Equal(expected, singleContent.Value);
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
        public void WithValue_nullable_DateTimeOffset_InclusionRule_DateTimeFormatting(int? year, int? month, int? day, int? offset, DateTimeFormatting formatting, InclusionRule inclusionRule, string expected)
        {
            // arrange
            var value = (year != null)
                ? new DateTimeOffset(year.Value, month.Value, day.Value, 0, 0, 0, TimeSpan.FromHours(offset.Value))
                : default(DateTimeOffset?);

            var name = "hello";
            var form = new List<KeyValuePair<string, string>>();

            // act
            form.WithValue(name, value, formatting, inclusionRule);

            // assert
            if (expected == null)
            {
                Assert.Empty(form);
            }
            else
            {
                Assert.NotEmpty(form);

                var singleContent = form.Single();
                Assert.Equal(name, singleContent.Key);
                Assert.Equal(expected, singleContent.Value);
            }
        }
    }
}
