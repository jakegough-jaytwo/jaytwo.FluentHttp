using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace jaytwo.FluentHttp
{
    public static class MultipartFormDataContentExtensions
    {
        public static MultipartFormDataContent WithContent(this MultipartFormDataContent multipartFormDataContent, HttpContent content, string name)
        {
            multipartFormDataContent.Add(content, name);
            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithContent(this MultipartFormDataContent multipartFormDataContent, HttpContent content, string name, string fileName)
        {
            multipartFormDataContent.Add(content, name, fileName);
            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, object value)
            => WithTextContent(multipartFormDataContent, name, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, object value, InclusionRule inclusionRule)
            => WithTextContent(multipartFormDataContent, name, "{0}", value, inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string format, object value)
            => WithTextContent(multipartFormDataContent, name, format, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string format, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                return multipartFormDataContent.WithTextContent(name, string.Format(format, value), inclusionRule);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string format, object[] value)
            => WithTextContent(multipartFormDataContent, name, format, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string format, object[] value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                return multipartFormDataContent.WithTextContent(name, string.Format(format, value ?? new object[] { }), inclusionRule);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string value)
        {
            return multipartFormDataContent.WithTextContent(name, value, InclusionRule.IncludeAlways);
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                var content = new StringContent(value ?? string.Empty, Encoding.UTF8, "text/plain");
                return multipartFormDataContent.WithContent(content, name);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool? value)
            => multipartFormDataContent.WithTextContent(key, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool? value, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, value, BooleanFormatting.Default, inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool value)
            => multipartFormDataContent.WithTextContent(key, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool value, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, value, BooleanFormatting.Default, inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool? value, BooleanFormatting formatting)
            => multipartFormDataContent.WithTextContent(key, value, formatting, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool? value, BooleanFormatting formatting, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool value, BooleanFormatting formatting)
            => multipartFormDataContent.WithTextContent(key, value, formatting, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool value, BooleanFormatting formatting, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime? value)
            => multipartFormDataContent.WithTextContent(key, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime? value, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, value, DateTimeFormatting.Default, inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime value)
            => multipartFormDataContent.WithTextContent(key, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime value, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, value, DateTimeFormatting.Default, inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime? value, DateTimeFormatting formatting)
            => multipartFormDataContent.WithTextContent(key, value, formatting, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime value, DateTimeFormatting formatting)
            => multipartFormDataContent.WithTextContent(key, value, formatting, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset? value)
            => multipartFormDataContent.WithTextContent(key, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset? value, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, value, DateTimeFormatting.Default, inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset? value, DateTimeFormatting formatting)
            => multipartFormDataContent.WithTextContent(key, value, formatting, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset value, DateTimeFormatting formatting)
            => multipartFormDataContent.WithTextContent(key, value, formatting, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, object value)
            => multipartFormDataContent.WithJsonContent(name, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                var json = JsonConvert.SerializeObject(value);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return multipartFormDataContent.WithContent(content, name);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, object value)
            => multipartFormDataContent.WithJsonContent(name, fileName, value, InclusionRule.IncludeAlways);

        public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                var json = JsonConvert.SerializeObject(value);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return multipartFormDataContent.WithContent(content, name, fileName);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithStreamContent(this MultipartFormDataContent multipartFormDataContent, string name, Stream stream)
        {
            var content = new StreamContent(stream);
            return multipartFormDataContent.WithContent(content, name);
        }

        public static MultipartFormDataContent WithStreamContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, Stream stream)
        {
            var content = new StreamContent(stream);
            return multipartFormDataContent.WithContent(content, name, fileName);
        }

        public static MultipartFormDataContent WithByteArrayContent(this MultipartFormDataContent multipartFormDataContent, string name, byte[] bytes)
        {
            return multipartFormDataContent.WithByteArrayContent(name, bytes, InclusionRule.IncludeAlways);
        }

        public static MultipartFormDataContent WithByteArrayContent(this MultipartFormDataContent multipartFormDataContent, string name, byte[] bytes, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(bytes, inclusionRule))
            {
                var content = new ByteArrayContent(bytes ?? new byte[] { });
                return multipartFormDataContent.WithContent(content, name);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithByteArrayContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, byte[] bytes)
        {
            return multipartFormDataContent.WithByteArrayContent(name, fileName, bytes, InclusionRule.IncludeAlways);
        }

        public static MultipartFormDataContent WithByteArrayContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, byte[] bytes, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(bytes, inclusionRule))
            {
                var content = new ByteArrayContent(bytes ?? new byte[] { });
                return multipartFormDataContent.WithContent(content, name, fileName);
            }

            return multipartFormDataContent;
        }
    }
}
