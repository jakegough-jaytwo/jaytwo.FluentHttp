using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentUri;
using jaytwo.UrlHelper;
using Newtonsoft.Json;

namespace jaytwo.FluentHttp
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage WithMethod(this HttpRequestMessage httpRequestMessage, HttpMethod method)
        {
            httpRequestMessage.Method = method;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithMethod(this HttpRequestMessage httpRequestMessage, string method)
        {
            return httpRequestMessage.WithMethod(new HttpMethod(method));
        }

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, string value)
            => httpRequestMessage.WithHeader(name, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, string value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                httpRequestMessage.Headers.AddSmartly(name, value);
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, object value)
            => httpRequestMessage.WithHeader(name, ObjectToStringHelper.GetString(value));

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, object value, InclusionRule inclusionRule)
            => httpRequestMessage.WithHeader(name, ObjectToStringHelper.GetString(value), inclusionRule);

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, string format, object value)
            => httpRequestMessage.WithHeader(name, format, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, string format, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                httpRequestMessage.WithHeader(name, string.Format(format, value), inclusionRule);
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, string format, object[] values)
            => httpRequestMessage.WithHeader(name, format, values, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequestMessage, string name, string format, object[] values, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(values, inclusionRule))
            {
                httpRequestMessage.WithHeader(name, string.Format(format, values ?? new object[] { }), inclusionRule);
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAccept(this HttpRequestMessage httpRequestMessage, string mediaType)
        {
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptApplicationJson(this HttpRequestMessage httpRequestMessage)
        {
            return httpRequestMessage.WithHeaderAccept("application/json");
        }

        public static HttpRequestMessage WithHeaderAcceptTextXml(this HttpRequestMessage httpRequestMessage)
        {
            return httpRequestMessage.WithHeaderAccept("text/xml");
        }

        public static HttpRequestMessage WithHeaderAcceptCharset(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue(value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptEncoding(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptLanguage(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAuthorization(this HttpRequestMessage httpRequestMessage, string scheme)
        {
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(scheme);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAuthorization(this HttpRequestMessage httpRequestMessage, string scheme, string value)
        {
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(scheme, value);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderCacheControl(this HttpRequestMessage httpRequestMessage, Action<CacheControlHeaderValue> builder)
        {
            var cacheControl = new CacheControlHeaderValue();
            builder.Invoke(cacheControl);
            httpRequestMessage.Headers.CacheControl = cacheControl;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderCacheControl(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.CacheControl = CacheControlHeaderValue.Parse(value);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderCacheControlNoCache(this HttpRequestMessage httpRequestMessage) =>
            httpRequestMessage.WithHeaderCacheControl(header =>
            {
                header.NoCache = true;
            });

        public static HttpRequestMessage WithHeaderConnectionClose(this HttpRequestMessage httpRequestMessage, bool? value)
        {
            httpRequestMessage.Headers.ConnectionClose = value;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderDate(this HttpRequestMessage httpRequestMessage, DateTimeOffset? value)
        {
            httpRequestMessage.Headers.Date = value;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfMatch(this HttpRequestMessage httpRequestMessage, string tag)
        {
            httpRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(ApplyQuotesIfMissing(tag)));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfMatch(this HttpRequestMessage httpRequestMessage, string tag, bool isWeak)
        {
            httpRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(ApplyQuotesIfMissing(tag), isWeak));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfModifiedSince(this HttpRequestMessage httpRequestMessage, DateTimeOffset? value)
        {
            httpRequestMessage.Headers.IfModifiedSince = value;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfNoneMatch(this HttpRequestMessage httpRequestMessage, string tag)
        {
            httpRequestMessage.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(tag));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfNoneMatch(this HttpRequestMessage httpRequestMessage, string tag, bool isWeak)
        {
            httpRequestMessage.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(tag, isWeak));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfRange(this HttpRequestMessage httpRequestMessage, DateTimeOffset lastModified)
        {
            httpRequestMessage.Headers.IfRange = new RangeConditionHeaderValue(lastModified);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfRange(this HttpRequestMessage httpRequestMessage, string entityTag)
        {
            httpRequestMessage.Headers.IfRange = new RangeConditionHeaderValue(ApplyQuotesIfMissing(entityTag));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfRange(this HttpRequestMessage httpRequestMessage, string entityTag, bool isWeak)
        {
            httpRequestMessage.Headers.IfRange = new RangeConditionHeaderValue(new EntityTagHeaderValue(ApplyQuotesIfMissing(entityTag), isWeak));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfUnmodifiedSince(this HttpRequestMessage httpRequestMessage, DateTimeOffset? value)
        {
            httpRequestMessage.Headers.IfUnmodifiedSince = value;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderPragma(this HttpRequestMessage httpRequestMessage, string name)
        {
            httpRequestMessage.Headers.Pragma.Add(new NameValueHeaderValue(name));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderPragma(this HttpRequestMessage httpRequestMessage, string name, string value)
        {
            httpRequestMessage.Headers.Pragma.Add(new NameValueHeaderValue(name, value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderPragmaNoCache(this HttpRequestMessage httpRequestMessage)
        {
            return httpRequestMessage.WithHeaderPragma("no-cache");
        }

        public static HttpRequestMessage WithHeaderRange(this HttpRequestMessage httpRequestMessage, long? from, long? to)
        {
            httpRequestMessage.Headers.Range = new RangeHeaderValue(from, to);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderUserAgent(this HttpRequestMessage httpRequestMessage, string userAgent)
        {
            httpRequestMessage.Headers.TryAddWithoutValidation("User-Agent", userAgent);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderUserAgent(this HttpRequestMessage httpRequestMessage, string productName, string productVersion)
        {
            httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue(productName, productVersion));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderUserAgent(this HttpRequestMessage httpRequestMessage, string productName, string productVersion, string comment)
        {
            httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue(productName, productVersion));

            if (comment != null)
            {
                httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue(ApplyParenthesesIfMissing(comment)));
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithBaseUri(this HttpRequestMessage httpRequestMessage, Uri uri)
        {
            if (httpRequestMessage.RequestUri == null)
            {
                httpRequestMessage.RequestUri = uri;
            }
            else
            {
                httpRequestMessage.RequestUri = new Uri(uri, httpRequestMessage.RequestUri);
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithBaseUri(this HttpRequestMessage httpRequestMessage, string pathOrUri)
        {
            return httpRequestMessage.WithBaseUri(new Uri(pathOrUri, UriKind.RelativeOrAbsolute));
        }

        public static HttpRequestMessage WithUri(this HttpRequestMessage httpRequestMessage, Uri uri)
        {
            httpRequestMessage.RequestUri = uri;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithUri(this HttpRequestMessage httpRequestMessage, string pathOrUri)
        {
            return httpRequestMessage.WithUri(new Uri(pathOrUri, UriKind.RelativeOrAbsolute));
        }

        public static HttpRequestMessage WithUri(this HttpRequestMessage httpRequestMessage, string pathFormat, params object[] formatArgs)
        {
            return httpRequestMessage.WithUri(Url.Format(pathFormat, formatArgs));
        }

        public static HttpRequestMessage WithUriPath(this HttpRequestMessage httpRequestMessage, string path)
        {
            if (httpRequestMessage.RequestUri != null)
            {
                return httpRequestMessage.WithUri(httpRequestMessage.RequestUri.WithPath(path));
            }
            else
            {
                return httpRequestMessage.WithUri(path);
            }
        }

        public static HttpRequestMessage WithUriPath(this HttpRequestMessage httpRequestMessage, string pathFormat, params object[] formatArgs)
        {
            return httpRequestMessage.WithUriPath(Url.Format(pathFormat, formatArgs));
        }

        public static HttpRequestMessage WithUriQuery(this HttpRequestMessage httpRequestMessage, string data)
        {
            if (httpRequestMessage.RequestUri != null)
            {
                return httpRequestMessage.WithUri(httpRequestMessage.RequestUri.WithQuery(data));
            }
            else
            {
                return httpRequestMessage.WithUri("?" + data.TrimStart('?'));
            }
        }

        public static HttpRequestMessage WithUriQuery(this HttpRequestMessage httpRequestMessage, object data)
        {
            return httpRequestMessage.WithUriQuery(QueryString.Serialize(data));
        }

        public static HttpRequestMessage WithUriQuery(this HttpRequestMessage httpRequestMessage, IDictionary<string, object> data)
        {
            return httpRequestMessage.WithUriQuery(QueryString.Serialize(data));
        }

        public static HttpRequestMessage WithUriQuery(this HttpRequestMessage httpRequestMessage, IDictionary<string, string[]> data)
        {
            return httpRequestMessage.WithUriQuery(QueryString.Serialize(data));
        }

        public static HttpRequestMessage WithUriQuery(this HttpRequestMessage httpRequestMessage, IDictionary<string, object[]> data)
        {
            return httpRequestMessage.WithUriQuery(QueryString.Serialize(data));
        }

        public static HttpRequestMessage WithUriQuery(this HttpRequestMessage httpRequestMessage, IDictionary<string, string> data)
        {
            return httpRequestMessage.WithUriQuery(QueryString.Serialize(data));
        }

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                if (httpRequestMessage.RequestUri != null)
                {
                    return httpRequestMessage.WithUri(httpRequestMessage.RequestUri.WithQueryParameter(key, value));
                }
                else
                {
                    return httpRequestMessage.WithUriQuery(new Dictionary<string, string>() { { key, value } });
                }
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string format, object value)
            => httpRequestMessage.WithUriQueryParameter(key, format, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string format, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                return httpRequestMessage.WithUriQueryParameter(key, string.Format(format, value), inclusionRule);
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object value)
            => httpRequestMessage.WithUriQueryParameter(key, ObjectToStringHelper.GetString(value));

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object value, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, ObjectToStringHelper.GetString(value), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, IEnumerable<string> values)
            => httpRequestMessage.WithUriQueryParameter(key, values?.ToArray(), InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, IEnumerable<string> values, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, values?.ToArray(), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string[] values)
            => httpRequestMessage.WithUriQueryParameter(key, values, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string[] values, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(values, inclusionRule))
            {
                if (httpRequestMessage.RequestUri != null)
                {
                    return httpRequestMessage.WithUri(httpRequestMessage.RequestUri.WithQueryParameter(key, values));
                }
                else
                {
                    return httpRequestMessage.WithUriQuery(new Dictionary<string, string[]>() { { key, values } });
                }
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, IEnumerable<object> values)
            => httpRequestMessage.WithUriQueryParameter(key, values?.Select(ObjectToStringHelper.GetString));

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, IEnumerable<object> values, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, values?.Select(ObjectToStringHelper.GetString), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object[] values)
            => httpRequestMessage.WithUriQueryParameter(key, values?.Select(ObjectToStringHelper.GetString));

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object[] values, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, values?.Select(ObjectToStringHelper.GetString), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string format, object[] values)
            => httpRequestMessage.WithUriQueryParameter(key, format, values, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string format, object[] values, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(values, inclusionRule))
            {
                httpRequestMessage.WithUriQueryParameter(key, string.Format(format, values), inclusionRule);
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool? value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool? value, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, value, BooleanFormatting.Default, inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool value, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, value, BooleanFormatting.Default, inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool? value, BooleanFormatting formatting)
            => httpRequestMessage.WithUriQueryParameter(key, value, formatting, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool? value, BooleanFormatting formatting, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool value, BooleanFormatting formatting)
            => httpRequestMessage.WithUriQueryParameter(key, value, formatting, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, bool value, BooleanFormatting formatting, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, BooleanFormattingHelper.Format(value, formatting), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime? value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime? value, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, value, DateTimeFormatting.Default, inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime value, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, value, DateTimeFormatting.Default, inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime? value, DateTimeFormatting formatting)
            => httpRequestMessage.WithUriQueryParameter(key, value, formatting, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime value, DateTimeFormatting formatting)
            => httpRequestMessage.WithUriQueryParameter(key, value, formatting, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTime value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTimeOffset? value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTimeOffset? value, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, value, DateTimeFormatting.Default, inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTimeOffset? value, DateTimeFormatting formatting)
            => httpRequestMessage.WithUriQueryParameter(key, value, formatting, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTimeOffset? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTimeOffset value, DateTimeFormatting formatting)
            => httpRequestMessage.WithUriQueryParameter(key, value, formatting, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, DateTimeOffset value, DateTimeFormatting formatting, InclusionRule inclusionRule)
            => httpRequestMessage.WithUriQueryParameter(key, DateTimeFormattingHelper.Format(value, formatting), inclusionRule);

        public static HttpRequestMessage WithJsonContent(this HttpRequestMessage httpRequestMessage, string json)
        {
            return httpRequestMessage.WithStringContent(json, "application/json");
        }

        public static HttpRequestMessage WithJsonContent(this HttpRequestMessage httpRequestMessage, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return httpRequestMessage.WithJsonContent(json);
        }

        public static HttpRequestMessage WithUrlEncodedFormContent(this HttpRequestMessage httpRequestMessage, string urlEncodedFormData)
        {
            return httpRequestMessage.WithStringContent(urlEncodedFormData, "application/x-www-form-urlencoded");
        }

        public static HttpRequestMessage WithUrlEncodedFormContent(this HttpRequestMessage httpRequestMessage, object data)
        {
            var urlEncodedFormData = QueryString.Serialize(data);
            return httpRequestMessage.WithUrlEncodedFormContent(urlEncodedFormData);
        }

        public static HttpRequestMessage WithUrlEncodedFormContent(this HttpRequestMessage httpRequestMessage, FormUrlEncodedContent content)
            => httpRequestMessage.WithContent(content);

        public static HttpRequestMessage WithUrlEncodedFormContent(this HttpRequestMessage httpRequestMessage, Action<List<KeyValuePair<string, string>>> contentBuilder)
        {
            var content = new List<KeyValuePair<string, string>>();
            contentBuilder.Invoke(content);
            return httpRequestMessage.WithContent(new FormUrlEncodedContent(content));
        }

        public static HttpRequestMessage WithMultipartFormDataContent(this HttpRequestMessage httpRequestMessage, Action<MultipartFormDataContent> contentBuilder)
             => httpRequestMessage.WithContent(new MultipartFormDataContent(), contentBuilder);

        public static HttpRequestMessage WithMultipartFormDataContent(this HttpRequestMessage httpRequestMessage, MultipartFormDataContent content)
             => httpRequestMessage.WithContent(content);

        public static HttpRequestMessage WithStringContent(this HttpRequestMessage httpRequestMessage, string content)
            => httpRequestMessage.WithContent(new StringContent(content));

        public static HttpRequestMessage WithStringContent(this HttpRequestMessage httpRequestMessage, string content, string mediaType)
            => httpRequestMessage.WithContent(new StringContent(content, Encoding.UTF8, mediaType));

        public static HttpRequestMessage WithStreamContent(this HttpRequestMessage httpRequestMessage, Stream stream)
            => httpRequestMessage.WithContent(new StreamContent(stream));

        public static HttpRequestMessage WithStreamContent(this HttpRequestMessage httpRequestMessage, Stream stream, string mediaType)
        {
            return httpRequestMessage.WithStreamContent(stream, content =>
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            });
        }

        public static HttpRequestMessage WithStreamContent(this HttpRequestMessage httpRequestMessage, Stream stream, Action<StreamContent> contentBuilder)
            => httpRequestMessage.WithContent(new StreamContent(stream), contentBuilder);

        public static HttpRequestMessage WithByteArrayContent(this HttpRequestMessage httpRequestMessage, byte[] bytes)
            => httpRequestMessage.WithContent(new ByteArrayContent(bytes));

        public static HttpRequestMessage WithByteArrayContent(this HttpRequestMessage httpRequestMessage, byte[] bytes, string mediaType)
        {
            return httpRequestMessage.WithByteArrayContent(bytes, content =>
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            });
        }

        public static HttpRequestMessage WithByteArrayContent(this HttpRequestMessage httpRequestMessage, byte[] bytes, Action<ByteArrayContent> contentBuilder)
            => httpRequestMessage.WithContent(new ByteArrayContent(bytes), contentBuilder);

        public static HttpRequestMessage WithContent<T>(this HttpRequestMessage httpRequestMessage, T content, Action<T> contentBuilder)
            where T : HttpContent
        {
            httpRequestMessage.Content = content;
            contentBuilder.Invoke(content);

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithContent<T>(this HttpRequestMessage httpRequestMessage, T content)
            where T : HttpContent
        {
            httpRequestMessage.Content = content;
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHttpVersion(this HttpRequestMessage httpRequestMessage, Version httpVersion)
        {
            httpRequestMessage.Version = httpVersion;
            return httpRequestMessage;
        }

        public static string GetHeaderValue(this HttpRequestMessage httpRequestMessage, string key)
        {
            return httpRequestMessage.Headers.GetHeaderValue(key) ?? httpRequestMessage.Content?.Headers.GetHeaderValue(key);
        }

        public static string GetHeaderValue(this HttpRequestMessage httpRequestMessage, string key, StringComparison stringComparison)
        {
            return httpRequestMessage.Headers.GetHeaderValue(key, stringComparison) ?? httpRequestMessage.Content?.Headers.GetHeaderValue(key, stringComparison);
        }

        private static string ApplyParenthesesIfMissing(string input)
        {
            if (input != null && !input.StartsWith("(") && !input.EndsWith(")"))
            {
                input = $"({input})";
            }

            return input;
        }

        private static string ApplyQuotesIfMissing(string input)
        {
            if (input != null && !input.StartsWith("\"") && !input.EndsWith("\""))
            {
                input = $"\"{input}\"";
            }

            return input;
        }
    }
}
