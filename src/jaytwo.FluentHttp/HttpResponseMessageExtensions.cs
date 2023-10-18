using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Exceptions;

namespace jaytwo.FluentHttp;

public static class HttpResponseMessageExtensions
{
    public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeAsync(this Task<HttpResponseMessage> responseTask)
    {
        var httpResponse = await responseTask;
        httpResponse.EnsureSuccessStatusCode();
        return httpResponse;
    }

    public static HttpResponseMessage EnsureExpectedStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode)
    {
        if (response.StatusCode != statusCode)
        {
            throw new UnexpectedStatusCodeException(response.StatusCode, response);
        }

        return response;
    }

    public static async Task EnsureExpectedStatusCodeAsync(this Task<HttpResponseMessage> responseTask, HttpStatusCode statusCode)
    {
        var response = await responseTask;
        response.EnsureExpectedStatusCode(statusCode);
    }

    public static HttpResponseMessage EnsureExpectedStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode, params HttpStatusCode[] additionalStatusCodes)
    {
        if (response.StatusCode != statusCode && !additionalStatusCodes.Contains(response.StatusCode))
        {
            throw new UnexpectedStatusCodeException(response.StatusCode, response);
        }

        return response;
    }

    public static async Task EnsureExpectedStatusCodeAsync(this Task<HttpResponseMessage> responseTask, HttpStatusCode statusCode, params HttpStatusCode[] additionalStatusCodes)
    {
        var response = await responseTask;
        response.EnsureExpectedStatusCode(statusCode, additionalStatusCodes);
    }

    public static async Task<T> AsAnonymousTypeAsync<T>(this Task<HttpResponseMessage> httpResponseTask, T anonymousPrototype)
    {
        var httpResponse = await httpResponseTask;
        var result = await httpResponse.AsAnonymousTypeAsync(anonymousPrototype);
        return result;
    }

    public static Task<T> AsAnonymousTypeAsync<T>(this HttpResponseMessage httpResponse, T anonymousPrototype)
    {
        return httpResponse.AsAsync<T>();
    }

    public static async Task<byte[]> AsByteArrayAsync(this Task<HttpResponseMessage> httpResponseTask)
    {
        var httpResponse = await httpResponseTask;
        var result = await httpResponse.AsByteArrayAsync();
        return result;
    }

    public static async Task<byte[]> AsByteArrayAsync(this HttpResponseMessage httpResponse)
    {
        using (httpResponse)
        {
            return await httpResponse.Content?.ReadAsByteArrayAsync();
        }
    }

    public static async Task<Stream> AsStreamAsync(this Task<HttpResponseMessage> httpResponseTask)
    {
        var httpResponse = await httpResponseTask;
        var result = await httpResponse.AsStreamAsync();
        return result;
    }

    public static async Task<Stream> AsStreamAsync(this HttpResponseMessage httpResponse)
    {
        // not disposing httpResponse because that only disposes the stream anyway
        return await httpResponse.Content?.ReadAsStreamAsync();
    }

    public static async Task<string> AsStringAsync(this Task<HttpResponseMessage> httpResponseTask)
    {
        var httpResponse = await httpResponseTask;
        var result = await httpResponse.AsStringAsync();
        return result;
    }

    public static async Task<string> AsStringAsync(this HttpResponseMessage httpResponse)
    {
        using (httpResponse)
        {
            return await httpResponse.Content?.ReadAsStringAsync();
        }
    }

    public static async Task<T> AsAsync<T>(this HttpResponseMessage httpResponse)
    {
        var isJson = false;
        var contentType = httpResponse?.Content?.Headers?.ContentType;

        var asString = default(string);
        if (ContentTypeEvaluator.IsJsonMediaType(contentType))
        {
            isJson = true;
            asString = await httpResponse.AsStringAsync();
        }
        else if (!ContentTypeEvaluator.IsBinaryMediaType(contentType))
        {
            asString = await httpResponse.AsStringAsync();

            if (ContentTypeEvaluator.CouldBeJsonString(asString))
            {
                isJson = true;
            }
        }

        if (isJson)
        {
            return JsonSerializer.Deserialize<T>(asString);
        }

        throw new InvalidOperationException("Data must be JSON to automatically deserialize.");
    }

    public static async Task<T> AsAsync<T>(this Task<HttpResponseMessage> httpResponseTask)
    {
        var httpResponse = await httpResponseTask;
        var result = await httpResponse.AsAsync<T>();
        return result;
    }

    public static async Task<T> ParseWithAsync<T>(this HttpResponseMessage httpResponse, Func<string, T> parseDelegate)
    {
        var asString = await httpResponse.AsStringAsync();
        return parseDelegate.Invoke(asString);
    }

    public static async Task<T> ParseWithAsync<T>(this Task<HttpResponseMessage> httpResponseTask, Func<string, T> parseDelegate)
    {
        var httpResponse = await httpResponseTask;
        var result = await httpResponse.ParseWithAsync<T>(parseDelegate);
        return result;
    }

    public static string GetHeaderValue(this HttpResponseMessage httpResponseMessage, string key)
    {
        return httpResponseMessage.Headers.GetHeaderValue(key) ?? httpResponseMessage.Content?.Headers.GetHeaderValue(key);
    }

    public static string GetHeaderValue(this HttpResponseMessage httpResponseMessage, string key, StringComparison stringComparison)
    {
        return httpResponseMessage.Headers.GetHeaderValue(key, stringComparison) ?? httpResponseMessage.Content?.Headers.GetHeaderValue(key, stringComparison);
    }
}
