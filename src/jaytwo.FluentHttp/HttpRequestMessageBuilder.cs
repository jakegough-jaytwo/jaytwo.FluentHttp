using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Formatting;

namespace jaytwo.FluentHttp;

public class HttpRequestMessageBuilder
{
    private List<Func<HttpRequestMessage, Task>> _actions = new List<Func<HttpRequestMessage, Task>>();

    public HttpRequestMessageBuilder WithAction(Func<HttpRequestMessage, Task> action)
    {
        _actions.Add(action);
        return this;
    }

    public HttpRequestMessageBuilder WithAction(Action<HttpRequestMessage> action)
        => WithAction(x =>
        {
            action(x);
            return Task.CompletedTask;
        });

    public HttpRequestMessageBuilder WithMethod(HttpMethod method)
        => WithAction(x => x.WithMethod(method));

    public HttpRequestMessageBuilder WithMethod(string method)
        => WithAction(x => x.WithMethod(method));

    public HttpRequestMessageBuilder WithHeader(string name, string value)
        => WithAction(x => x.WithHeader(name, value));

    public HttpRequestMessageBuilder WithHeader(string name, string value, InclusionRule inclusionRule)
        => WithAction(x => x.WithHeader(name, value, inclusionRule));

    public HttpRequestMessageBuilder WithHeader(string name, object value)
        => WithAction(x => x.WithHeader(name, value));

    public HttpRequestMessageBuilder WithHeader(string name, object value, InclusionRule inclusionRule)
        => WithAction(x => x.WithHeader(name, value, inclusionRule));

    public HttpRequestMessageBuilder WithHeader(string name, string format, object value)
        => WithAction(x => x.WithHeader(name, format, value));

    public HttpRequestMessageBuilder WithHeader(string name, string format, object value, InclusionRule inclusionRule)
        => WithAction(x => x.WithHeader(name, format, value, inclusionRule));

    public HttpRequestMessageBuilder WithHeader(string name, string format, object[] values)
        => WithAction(x => x.WithHeader(name, format, values));

    public HttpRequestMessageBuilder WithHeader(string name, string format, object[] values, InclusionRule inclusionRule)
        => WithAction(x => x.WithHeader(name, format, values, inclusionRule));

    public HttpRequestMessageBuilder WithHeaderAccept(string mediaType)
        => WithAction(x => x.WithHeaderAccept(mediaType));

    public HttpRequestMessageBuilder WithHeaderAcceptApplicationJson()
        => WithAction(x => x.WithHeaderAcceptApplicationJson());

    public HttpRequestMessageBuilder WithHeaderAcceptTextXml()
        => WithAction(x => x.WithHeaderAcceptTextXml());

    public HttpRequestMessageBuilder WithHeaderAcceptCharset(string value)
        => WithAction(x => x.WithHeaderAcceptCharset(value));

    public HttpRequestMessageBuilder WithHeaderAcceptEncoding(string value)
        => WithAction(x => x.WithHeaderAcceptEncoding(value));

    public HttpRequestMessageBuilder WithHeaderAcceptLanguage(string value)
        => WithAction(x => x.WithHeaderAcceptLanguage(value));

    public HttpRequestMessageBuilder WithHeaderAuthorization(string scheme)
        => WithAction(x => x.WithHeaderAuthorization(scheme));

    public HttpRequestMessageBuilder WithHeaderAuthorization(string scheme, string value)
        => WithAction(x => x.WithHeaderAuthorization(scheme, value));

    public HttpRequestMessageBuilder WithHeaderCacheControl(Action<CacheControlHeaderValue> builder)
        => WithAction(x => x.WithHeaderCacheControl(builder));

    public HttpRequestMessageBuilder WithHeaderCacheControl(string value)
        => WithAction(x => x.WithHeaderCacheControl(value));

    public HttpRequestMessageBuilder WithHeaderCacheControlNoCache()
        => WithAction(x => x.WithHeaderCacheControlNoCache());

    public HttpRequestMessageBuilder WithHeaderConnectionClose(bool? value)
        => WithAction(x => x.WithHeaderConnectionClose(value));

    public HttpRequestMessageBuilder WithHeaderDate(DateTimeOffset? value)
        => WithAction(x => x.WithHeaderDate(value));

    public HttpRequestMessageBuilder WithHeaderIfMatch(string tag)
        => WithAction(x => x.WithHeaderIfMatch(tag));

    public HttpRequestMessageBuilder WithHeaderIfMatch(string tag, bool isWeak)
        => WithAction(x => x.WithHeaderIfMatch(tag, isWeak));

    public HttpRequestMessageBuilder WithHeaderIfModifiedSince(DateTimeOffset? value)
        => WithAction(x => x.WithHeaderIfModifiedSince(value));

    public HttpRequestMessageBuilder WithHeaderIfNoneMatch(string tag)
        => WithAction(x => x.WithHeaderIfNoneMatch(tag));

    public HttpRequestMessageBuilder WithHeaderIfNoneMatch(string tag, bool isWeak)
        => WithAction(x => x.WithHeaderIfNoneMatch(tag, isWeak));

    public HttpRequestMessageBuilder WithHeaderIfRange(DateTimeOffset lastModified)
        => WithAction(x => x.WithHeaderIfRange(lastModified));

    public HttpRequestMessageBuilder WithHeaderIfRange(string entityTag)
        => WithAction(x => x.WithHeaderIfRange(entityTag));

    public HttpRequestMessageBuilder WithHeaderIfRange(string entityTag, bool isWeak)
        => WithAction(x => x.WithHeaderIfRange(entityTag, isWeak));

    public HttpRequestMessageBuilder WithHeaderIfUnmodifiedSince(DateTimeOffset? value)
        => WithAction(x => x.WithHeaderIfUnmodifiedSince(value));

    public HttpRequestMessageBuilder WithHeaderPragma(string name)
        => WithAction(x => x.WithHeaderPragma(name));

    public HttpRequestMessageBuilder WithHeaderPragma(string name, string value)
        => WithAction(x => x.WithHeaderPragma(name, value));

    public HttpRequestMessageBuilder WithHeaderPragmaNoCache()
        => WithAction(x => x.WithHeaderPragmaNoCache());

    public HttpRequestMessageBuilder WithHeaderRange(long? from, long? to)
        => WithAction(x => x.WithHeaderRange(from, to));

    public HttpRequestMessageBuilder WithHeaderUserAgent(string userAgent)
        => WithAction(x => x.WithHeaderUserAgent(userAgent));

    public HttpRequestMessageBuilder WithHeaderUserAgent(string productName, string productVersion)
        => WithAction(x => x.WithHeaderUserAgent(productName, productVersion));

    public HttpRequestMessageBuilder WithHeaderUserAgent(string productName, string productVersion, string comment)
        => WithAction(x => x.WithHeaderUserAgent(productName, productVersion, comment));

    public HttpRequestMessageBuilder WithBaseUri(Uri uri)
        => WithAction(x => x.WithBaseUri(uri));

    public HttpRequestMessageBuilder WithBaseUri(string pathOrUri)
        => WithAction(x => x.WithBaseUri(pathOrUri));

    public HttpRequestMessageBuilder WithUri(Uri uri)
        => WithAction(x => x.WithUri(uri));

    public HttpRequestMessageBuilder WithUri(string pathOrUri)
        => WithAction(x => x.WithUri(pathOrUri));

    public HttpRequestMessageBuilder WithUri(string pathFormat, params object[] formatArgs)
        => WithAction(x => x.WithUri(pathFormat, formatArgs));

    public HttpRequestMessageBuilder WithUriPath(string path)
        => WithAction(x => x.WithUriPath(path));

    public HttpRequestMessageBuilder WithUriPath(string pathFormat, params object[] formatArgs)
        => WithAction(x => x.WithUriPath(pathFormat, formatArgs));

    public HttpRequestMessageBuilder WithUriQuery(string data)
        => WithAction(x => x.WithUriQuery(data));

    public HttpRequestMessageBuilder WithUriQuery(object data)
        => WithAction(x => x.WithUriQuery(data));

    public HttpRequestMessageBuilder WithUriQuery(IDictionary<string, object> data)
        => WithAction(x => x.WithUriQuery(data));

    public HttpRequestMessageBuilder WithUriQuery(IDictionary<string, string[]> data)
        => WithAction(x => x.WithUriQuery(data));

    public HttpRequestMessageBuilder WithUriQuery(IDictionary<string, object[]> data)
        => WithAction(x => x.WithUriQuery(data));

    public HttpRequestMessageBuilder WithUriQuery(IDictionary<string, string> data)
        => WithAction(x => x.WithUriQuery(data));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string format, object value)
        => WithAction(x => x.WithUriQueryParameter(key, format, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string format, object value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, format, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, object value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, object value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, IEnumerable<string> values)
        => WithAction(x => x.WithUriQueryParameter(key, values));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, IEnumerable<string> values, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, values, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string[] values)
        => WithAction(x => x.WithUriQueryParameter(key, values));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string[] values, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, values, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, IEnumerable<object> values)
        => WithAction(x => x.WithUriQueryParameter(key, values));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, IEnumerable<object> values, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, values, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, object[] values)
        => WithAction(x => x.WithUriQueryParameter(key, values));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, object[] values, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, values, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string format, object[] values)
        => WithAction(x => x.WithUriQueryParameter(key, format, values));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, string format, object[] values, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, format, values, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool? value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool? value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool? value, BooleanFormatting formatting)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool? value, BooleanFormatting formatting, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool value, BooleanFormatting formatting)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, bool value, BooleanFormatting formatting, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime? value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime? value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime? value, DateTimeFormatting formatting)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime value, DateTimeFormatting formatting)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTime value, DateTimeFormatting formatting, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTimeOffset? value)
        => WithAction(x => x.WithUriQueryParameter(key, value));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTimeOffset? value, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTimeOffset? value, DateTimeFormatting formatting)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTimeOffset? value, DateTimeFormatting formatting, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting, inclusionRule));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTimeOffset value, DateTimeFormatting formatting)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting));

    public HttpRequestMessageBuilder WithUriQueryParameter(string key, DateTimeOffset value, DateTimeFormatting formatting, InclusionRule inclusionRule)
        => WithAction(x => x.WithUriQueryParameter(key, value, formatting, inclusionRule));

    public HttpRequestMessageBuilder WithJsonContent(string json)
        => WithAction(x => x.WithJsonContent(json));

    public HttpRequestMessageBuilder WithJsonContent(object data)
        => WithAction(x => x.WithJsonContent(data));

    public HttpRequestMessageBuilder WithUrlEncodedFormContent(string urlEncodedFormData)
        => WithAction(x => x.WithUrlEncodedFormContent(urlEncodedFormData));

    public HttpRequestMessageBuilder WithUrlEncodedFormContent(object data)
        => WithAction(x => x.WithUrlEncodedFormContent(data));

    public HttpRequestMessageBuilder WithUrlEncodedFormContent(FormUrlEncodedContent content)
        => WithAction(x => x.WithUrlEncodedFormContent(content));

    public HttpRequestMessageBuilder WithUrlEncodedFormContent(Action<List<KeyValuePair<string, string>>> contentBuilder)
        => WithAction(x => x.WithUrlEncodedFormContent(contentBuilder));

    public HttpRequestMessageBuilder WithMultipartFormDataContent(Action<MultipartFormDataContent> contentBuilder)
         => WithAction(x => x.WithMultipartFormDataContent(contentBuilder));

    public HttpRequestMessageBuilder WithMultipartFormDataContent(MultipartFormDataContent content)
         => WithAction(x => x.WithMultipartFormDataContent(content));

    public HttpRequestMessageBuilder WithStringContent(string content)
        => WithAction(x => x.WithStringContent(content));

    public HttpRequestMessageBuilder WithStringContent(string content, string mediaType)
        => WithAction(x => x.WithStringContent(content, mediaType));

    public HttpRequestMessageBuilder WithStreamContent(Stream stream)
        => WithAction(x => x.WithStreamContent(stream));

    public HttpRequestMessageBuilder WithStreamContent(Stream stream, string mediaType)
        => WithAction(x => x.WithStreamContent(stream, mediaType));

    public HttpRequestMessageBuilder WithStreamContent(Stream stream, Action<StreamContent> contentBuilder)
        => WithAction(x => x.WithStreamContent(stream, contentBuilder));

    public HttpRequestMessageBuilder WithByteArrayContent(byte[] bytes)
        => WithAction(x => x.WithByteArrayContent(bytes));

    public HttpRequestMessageBuilder WithByteArrayContent(byte[] bytes, string mediaType)
        => WithAction(x => x.WithByteArrayContent(bytes, mediaType));

    public HttpRequestMessageBuilder WithByteArrayContent(byte[] bytes, Action<ByteArrayContent> contentBuilder)
        => WithAction(x => x.WithByteArrayContent(bytes, contentBuilder));

    public HttpRequestMessageBuilder WithContent<T>(T content, Action<T> contentBuilder)
        where T : HttpContent
        => WithAction(x => x.WithContent<T>(content, contentBuilder));

    public HttpRequestMessageBuilder WithContent<T>(T content)
        where T : HttpContent
        => WithAction(x => x.WithContent<T>(content));

    public HttpRequestMessageBuilder WithHttpVersion(Version httpVersion)
        => WithAction(x => x.WithHttpVersion(httpVersion));

    public async Task<HttpRequestMessage> BuildHttpRequestMessageAsync()
    {
        var result = new HttpRequestMessage();

        try
        {
            foreach (var action in _actions)
            {
                await action.Invoke(result);
            }
        }
        catch
        {
            result.Dispose();
            throw;
        }

        return result;
    }
}
