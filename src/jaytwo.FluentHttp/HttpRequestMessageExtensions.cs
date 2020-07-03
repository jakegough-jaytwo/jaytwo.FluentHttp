using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Authentication;
using jaytwo.FluentHttp.Authentication.Basic;
using jaytwo.FluentHttp.Authentication.Digest;
using jaytwo.FluentHttp.Authentication.Token;
using jaytwo.FluentUri;
using jaytwo.MimeHelper;
using jaytwo.UrlHelper;
using Newtonsoft.Json;

namespace jaytwo.FluentHttp
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage Invoke(this HttpRequestMessage httpRequestMessage, Action<HttpRequestMessage> callback)
        {
            callback.Invoke(httpRequestMessage);
            return httpRequestMessage;
        }

        public static async Task<HttpRequestMessage> Invoke(this HttpRequestMessage httpRequestMessage, Func<HttpRequestMessage, Task> callback)
        {
            await callback.Invoke(httpRequestMessage);
            return httpRequestMessage;
        }

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

        public static HttpRequestMessage WithHeaderAccept(this HttpRequestMessage httpRequestMessage, string mediaType)
        {
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptApplicationJson(this HttpRequestMessage httpRequestMessage)
        {
            return httpRequestMessage.WithHeaderAccept(MediaType.application_json);
        }

        public static HttpRequestMessage WithHeaderAcceptTextXml(this HttpRequestMessage httpRequestMessage)
        {
            return httpRequestMessage.WithHeaderAccept(MediaType.text_xml);
        }

        public static HttpRequestMessage WithHeaderAccept(this HttpRequestMessage httpRequestMessage, string mediaType, double quality)
        {
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType, quality));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptCharset(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue(value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptCharset(this HttpRequestMessage httpRequestMessage, string value, double quality)
        {
            httpRequestMessage.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue(value, quality));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptEncoding(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptEncoding(this HttpRequestMessage httpRequestMessage, string value, double quality)
        {
            httpRequestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(value, quality));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptLanguage(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(value));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderAcceptLanguage(this HttpRequestMessage httpRequestMessage, string value, double quality)
        {
            httpRequestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(value, quality));
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

        public static HttpRequestMessage WithHeaderCacheControl(this HttpRequestMessage httpRequestMessage, string value)
        {
            httpRequestMessage.Headers.CacheControl = CacheControlHeaderValue.Parse(value);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderCacheControlNoCache(this HttpRequestMessage httpRequestMessage)
        {
            return httpRequestMessage.WithHeaderCacheControl("no-cache");
        }

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
            httpRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(tag));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfMatch(this HttpRequestMessage httpRequestMessage, string tag, bool isWeak)
        {
            httpRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(tag, isWeak));
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

        public static HttpRequestMessage WithHeaderIfRange(this HttpRequestMessage httpRequestMessage, DateTimeOffset value)
        {
            httpRequestMessage.Headers.IfRange = new RangeConditionHeaderValue(value);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfRange(this HttpRequestMessage httpRequestMessage, string entityTag)
        {
            httpRequestMessage.Headers.IfRange = new RangeConditionHeaderValue(entityTag);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderIfRange(this HttpRequestMessage httpRequestMessage, string tag, bool isWeak)
        {
            httpRequestMessage.Headers.IfRange = new RangeConditionHeaderValue(new EntityTagHeaderValue(tag, isWeak));
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

        public static HttpRequestMessage WithHeaderUserAgent(this HttpRequestMessage httpRequestMessage, string comment)
        {
            httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue(comment));
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithHeaderUserAgent(this HttpRequestMessage httpRequestMessage, string productName, string productVersion)
        {
            httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue(productName, productVersion));
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

        public static HttpRequestMessage WithUri(this HttpRequestMessage httpRequestMessage, string pathFormat, params string[] formatArgs)
        {
            return httpRequestMessage.WithUri(Url.Format(pathFormat, formatArgs));
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

        public static HttpRequestMessage WithUriPath(this HttpRequestMessage httpRequestMessage, string pathFormat, params string[] formatArgs)
        {
            return httpRequestMessage.WithUriPath(Url.Format(pathFormat, formatArgs));
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

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object value)
            => httpRequestMessage.WithUriQueryParameter(key, value, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object value, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(value, inclusionRule))
            {
                if (httpRequestMessage.RequestUri != null)
                {
                    return httpRequestMessage.WithUri(httpRequestMessage.RequestUri.WithQueryParameter(key, value));
                }
                else
                {
                    return httpRequestMessage.WithUriQuery(new Dictionary<string, object>() { { key, value } });
                }
            }

            return httpRequestMessage;
        }

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

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object[] values)
            => httpRequestMessage.WithUriQueryParameter(key, values, InclusionRule.IncludeAlways);

        public static HttpRequestMessage WithUriQueryParameter(this HttpRequestMessage httpRequestMessage, string key, object[] values, InclusionRule inclusionRule)
        {
            if (InclusionRuleHelper.IncludeContent(values, inclusionRule))
            {
                if (httpRequestMessage.RequestUri != null)
                {
                    return httpRequestMessage.WithUri(httpRequestMessage.RequestUri.WithQueryParameter(key, values));
                }
                else
                {
                    return httpRequestMessage.WithUriQuery(new Dictionary<string, object[]>() { { key, values } });
                }
            }

            return httpRequestMessage;
        }

        public static HttpRequestMessage WithAuthentication(this HttpRequestMessage httpRequestMessage, IAuthenticationProvider authenticationProvider)
        {
            authenticationProvider.Authenticate(httpRequestMessage);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithBasicAuthentication(this HttpRequestMessage httpRequestMessage, string user, string pass)
        {
            return httpRequestMessage.WithAuthentication(new BasicAuthenticationProvider(user, pass));
        }

        public static HttpRequestMessage WithTokenAuthentication(this HttpRequestMessage httpRequestMessage, string token)
        {
            return httpRequestMessage.WithAuthentication(new TokenAuthenticationProvider(token));
        }

        public static HttpRequestMessage WithTokenAuthentication(this HttpRequestMessage httpRequestMessage, Func<string> tokenDelegate)
        {
            return httpRequestMessage.WithAuthentication(new TokenAuthenticationProvider(tokenDelegate));
        }

        public static HttpRequestMessage WithTokenAuthentication(this HttpRequestMessage httpRequestMessage, ITokenProvider tokenProvider)
        {
            return httpRequestMessage.WithAuthentication(new TokenAuthenticationProvider(tokenProvider));
        }

#if !NETSTANDARD1_1
        public static HttpRequestMessage WithDigestAuthentication(this HttpRequestMessage httpRequestMessage, HttpClient httpClient, string user, string pass)
        {
            return httpRequestMessage.WithAuthentication(new DigestAuthenticationProvider(httpClient, user, pass));
        }
#endif

        public static HttpRequestMessage WithJsonContent(this HttpRequestMessage httpRequestMessage, string json)
        {
            return httpRequestMessage.WithStringContent(json, MediaType.application_json);
        }

        public static HttpRequestMessage WithJsonContent(this HttpRequestMessage httpRequestMessage, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return httpRequestMessage.WithJsonContent(json);
        }

        public static HttpRequestMessage WithUrlEncodedFormContent(this HttpRequestMessage httpRequestMessage, string urlEncodedFormData)
        {
            return httpRequestMessage.WithStringContent(urlEncodedFormData, MediaType.application_x_www_form_urlencoded);
        }

        public static HttpRequestMessage WithUrlEncodedFormContent(this HttpRequestMessage httpRequestMessage, object data)
        {
            var urlEncodedFormData = QueryString.Serialize(data);
            return httpRequestMessage.WithUrlEncodedFormContent(urlEncodedFormData);
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
            return httpRequestMessage.Headers?.GetHeaderValue(key) ?? httpRequestMessage?.Content?.Headers?.GetHeaderValue(key);
        }

        public static string GetHeaderValue(this HttpRequestMessage httpRequestMessage, string key, StringComparison stringComparison)
        {
            return httpRequestMessage.Headers?.GetHeaderValue(key, stringComparison) ?? httpRequestMessage?.Content?.Headers?.GetHeaderValue(key, stringComparison);
        }
    }
}
