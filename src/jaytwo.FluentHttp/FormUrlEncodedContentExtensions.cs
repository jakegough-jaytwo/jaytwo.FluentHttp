using System;
using System.Collections.Generic;

namespace jaytwo.FluentHttp
{
    public static class FormUrlEncodedContentExtensions
    {
        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, string value)
            => data.WithValue(key, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, string value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                data.Add(new KeyValuePair<string, string>(key, value ?? string.Empty));
            }

            return data;
        }

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, object value)
            => data.WithValue(key, ObjectToStringHelper.GetString(value));

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, object value, InclusionRule inclusionRule)
            => data.WithValue(key, ObjectToStringHelper.GetString(value), inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, string format, object value)
            => data.WithValue(key, format, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, string format, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                data.WithValue(key, string.Format(format, value), inclusionRule);
            }

            return data;
        }

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, string format, object[] values)
            => data.WithValue(key, format, values, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, string format, object[] values, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(values, inclusionRule))
            {
                data.WithValue(key, string.Format(format, values ?? new object[] { }), inclusionRule);
            }

            return data;
        }

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool? value)
            => data.WithValue(key, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool? value, InclusionRule inclusionRule)
            => data.WithValue(key, value, BooleanFormatting.Default, inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool value)
            => data.WithValue(key, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool value, InclusionRule inclusionRule)
            => data.WithValue(key, value, BooleanFormatting.Default, inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool? value, BooleanFormatting formatting)
            => data.WithValue(key, value, formatting, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool? value, BooleanFormatting formatting, InclusionRule inclusionRule)
            => data.WithValue(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool value, BooleanFormatting formatting)
            => data.WithValue(key, value, formatting, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, bool value, BooleanFormatting formatting, InclusionRule inclusionRule)
            => data.WithValue(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime? value)
            => data.WithValue(key, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime? value, InclusionRule inclusionRule)
            => data.WithValue(key, value, DateTimeFormatting.Default, inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime value)
            => data.WithValue(key, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime value, InclusionRule inclusionRule)
            => data.WithValue(key, value, DateTimeFormatting.Default, inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime? value, DateTimeFormatting formatting)
            => data.WithValue(key, value, formatting, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => data.WithValue(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime value, DateTimeFormatting formatting)
            => data.WithValue(key, value, formatting, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTime value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => data.WithValue(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTimeOffset? value)
            => data.WithValue(key, value, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTimeOffset? value, InclusionRule inclusionRule)
            => data.WithValue(key, value, DateTimeFormatting.Default, inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTimeOffset? value, DateTimeFormatting formatting)
            => data.WithValue(key, value, formatting, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTimeOffset? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => data.WithValue(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTimeOffset value, DateTimeFormatting formatting)
            => data.WithValue(key, value, formatting, InclusionRule.IncludeAlways);

        public static List<KeyValuePair<string, string>> WithValue(this List<KeyValuePair<string, string>> data, string key, DateTimeOffset value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => data.WithValue(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);
    }
}
