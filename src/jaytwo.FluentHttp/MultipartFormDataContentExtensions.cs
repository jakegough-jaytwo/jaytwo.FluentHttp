using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using jaytwo.FluentHttp.Formatting;

namespace jaytwo.FluentHttp;

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

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, object value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => WithTextContent(multipartFormDataContent, name, ObjectToStringHelper.GetString(value), inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string format, object value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
        {
            return multipartFormDataContent.WithTextContent(name, string.Format(format, value), inclusionRule);
        }

        return multipartFormDataContent;
    }

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string format, object[] value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
        {
            return multipartFormDataContent.WithTextContent(name, string.Format(format, value ?? new object[] { }), inclusionRule);
        }

        return multipartFormDataContent;
    }

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
        {
            var content = new StringContent(value ?? string.Empty, Encoding.UTF8, "text/plain");
            return multipartFormDataContent.WithContent(content, name);
        }

        return multipartFormDataContent;
    }

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool? value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, value, BooleanFormatting.Default, inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, value, BooleanFormatting.Default, inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool? value, BooleanFormatting formatting, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, bool value, BooleanFormatting formatting, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime? value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, value, DateTimeFormatting.Default, inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, value, DateTimeFormatting.Default, inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime? value, DateTimeFormatting formatting, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTime value, DateTimeFormatting formatting, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset? value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, value, DateTimeFormatting.Default, inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset? value, DateTimeFormatting formatting, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

    public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string key, DateTimeOffset value, DateTimeFormatting formatting, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
        => multipartFormDataContent.WithTextContent(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

    public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, object value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
        {
            var json = JsonSerializer.Serialize(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return multipartFormDataContent.WithContent(content, name);
        }

        return multipartFormDataContent;
    }

    public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, object value, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
        {
            var json = JsonSerializer.Serialize(value);
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

    public static MultipartFormDataContent WithByteArrayContent(this MultipartFormDataContent multipartFormDataContent, string name, byte[] bytes, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(bytes, inclusionRule))
        {
            var content = new ByteArrayContent(bytes ?? new byte[] { });
            return multipartFormDataContent.WithContent(content, name);
        }

        return multipartFormDataContent;
    }

    public static MultipartFormDataContent WithByteArrayContent(this MultipartFormDataContent multipartFormDataContent, string name, string fileName, byte[] bytes, InclusionRule inclusionRule = InclusionRule.IncludeAlways)
    {
        if (InclusionRuleHelper.IncludeContent(bytes, inclusionRule))
        {
            var content = new ByteArrayContent(bytes ?? new byte[] { });
            return multipartFormDataContent.WithContent(content, name, fileName);
        }

        return multipartFormDataContent;
    }
}
