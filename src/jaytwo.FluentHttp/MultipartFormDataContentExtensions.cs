using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using jaytwo.MimeHelper;
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

        public static MultipartFormDataContent WithContent<T>(this MultipartFormDataContent multipartFormDataContent, T content, string name, InclusionRule inclusionRule)
            where T : HttpContent
        {
            if (content != null || inclusionRule == InclusionRule.IncludeAlways)
            {
                multipartFormDataContent.WithContent(content, name);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithContent<T>(this MultipartFormDataContent multipartFormDataContent, T content, string name, Action<T> contentBuilder)
            where T : HttpContent
        {
            multipartFormDataContent.WithContent(content, name);
            contentBuilder.Invoke(content);
            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithContent(this MultipartFormDataContent multipartFormDataContent, HttpContent content, string name, string fileName)
        {
            multipartFormDataContent.Add(content, name, fileName);
            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithContent<T>(this MultipartFormDataContent multipartFormDataContent, T content, string name, string fileName, InclusionRule inclusionRule)
            where T : HttpContent
        {
            if (content != null || inclusionRule == InclusionRule.IncludeAlways)
            {
                multipartFormDataContent.WithContent(content, name, fileName);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithContent<T>(this MultipartFormDataContent multipartFormDataContent, T content, string name, string fileName, Action<T> contentBuilder)
            where T : HttpContent
        {
            multipartFormDataContent.WithContent(content, name, fileName);
            contentBuilder.Invoke(content);
            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, object value)
            => WithTextContent(multipartFormDataContent, name, $"{value}");

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, object value, InclusionRule inclusionRule)
            => WithTextContent(multipartFormDataContent, name, $"{value}", inclusionRule);

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string value)
        {
            return multipartFormDataContent.WithTextContent(name, value, InclusionRule.IncludeAlways);
        }

        public static MultipartFormDataContent WithTextContent(this MultipartFormDataContent multipartFormDataContent, string name, string value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                var content = new StringContent(value, Encoding.UTF8, MediaType.text_plain);
                return multipartFormDataContent.WithContent(content, name);
            }

            return multipartFormDataContent;
        }

        public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, object value)
        {
            return multipartFormDataContent.WithJsonContent(name, value, InclusionRule.IncludeAlways);
        }

        public static MultipartFormDataContent WithJsonContent(this MultipartFormDataContent multipartFormDataContent, string name, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                var json = JsonConvert.SerializeObject(value);
                var content = new StringContent(json, Encoding.UTF8, MediaType.application_json);
                return multipartFormDataContent.WithContent(content, name);
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
                var content = new ByteArrayContent(bytes);
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
                var content = new ByteArrayContent(bytes);
                return multipartFormDataContent.WithContent(content, name, fileName);
            }

            return multipartFormDataContent;
        }
    }
}
