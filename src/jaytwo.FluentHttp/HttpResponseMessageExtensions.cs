using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.AsyncHelper;
using jaytwo.FluentHttp.Exceptions;
using Newtonsoft.Json;

namespace jaytwo.FluentHttp
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<HttpResponseMessage> EnsureSuccessStatusCode(this Task<HttpResponseMessage> responseTask)
        {
            var httpResponse = await responseTask;
            httpResponse.EnsureSuccessStatusCode();
            return httpResponse;
        }

        public static HttpResponseMessage EnsureExpectedStatusCode(this HttpResponseMessage response, params HttpStatusCode[] statusCodes)
        {
            if (!statusCodes.Contains(response.StatusCode))
            {
                throw new UnexpectedStatusCodeException(response.StatusCode, response);
            }

            return response;
        }

        public static async Task EnsureExpectedStatusCodeAsync(this Task<HttpResponseMessage> responseTask, params HttpStatusCode[] statusCodes)
        {
            var response = await responseTask;
            response.EnsureExpectedStatusCode(statusCodes);
        }

        public static T AsAnonymousType<T>(this HttpResponseMessage httpResponse, T anonymousTypeObject)
        {
            return httpResponse.As<T>();
        }

        public static Task<T> AsAnonymousTypeAsync<T>(this HttpResponseMessage httpResponse, T anonymousTypeObject)
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

        public static byte[] AsByteArray(this HttpResponseMessage httpResponse)
        {
            return httpResponse.AsByteArrayAsync().AwaitSynchronously();
        }

        public static async Task<Stream> AsStreamAsync(this Task<HttpResponseMessage> httpResponseTask)
        {
            var httpResponse = await httpResponseTask;
            var result = await httpResponse.AsStreamAsync();
            return result;
        }

        public static async Task<Stream> AsStreamAsync(this HttpResponseMessage httpResponse)
        {
            using (httpResponse)
            {
                return await httpResponse.Content?.ReadAsStreamAsync();
            }
        }

        public static Stream AsStream(this HttpResponseMessage httpResponse)
        {
            return httpResponse.AsStreamAsync().AwaitSynchronously();
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

        public static string AsString(this HttpResponseMessage httpResponse)
        {
            return httpResponse.AsStringAsync().AwaitSynchronously();
        }

        public static async Task<T> AsAnonymousType<T>(this Task<HttpResponseMessage> httpResponseTask, T anonymousTypeObject)
        {
            var httpResponse = await httpResponseTask;
            return httpResponse.AsAnonymousType<T>(anonymousTypeObject);
        }

        public static T As<T>(this HttpResponseMessage httpResponse)
        {
            return AsAsync<T>(httpResponse).AwaitSynchronously();
        }

        public static async Task<T> AsAsync<T>(this HttpResponseMessage httpResponse)
        {
            var isJson = false;
            var contentType = httpResponse?.Content?.Headers?.ContentType;

            var asString = default(string);
            if (ContentTypeEvaluator.IsJsonContentType(contentType))
            {
                isJson = true;
                asString = await httpResponse.AsStringAsync();
            }
            else if (!ContentTypeEvaluator.IsBinaryContent(contentType))
            {
                asString = await httpResponse.AsStringAsync();

                if (ContentTypeEvaluator.CouldBeJsonString(asString))
                {
                    isJson = true;
                }
            }

            if (isJson)
            {
                return JsonConvert.DeserializeObject<T>(asString);
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

        public static T ParseWith<T>(this HttpResponseMessage httpResponse, Func<string, T> parseDelegate)
        {
            return httpResponse.ParseWithAsync<T>(parseDelegate).AwaitSynchronously();
        }

        public static string GetHeaderValue(this HttpResponseMessage httpResponseMessage, string key)
        {
            return httpResponseMessage.Headers?.GetHeaderValue(key) ?? httpResponseMessage.Content.Headers.GetHeaderValue(key);
        }

        public static string GetHeaderValue(this HttpResponseMessage httpResponseMessage, string key, StringComparison stringComparison)
        {
            return httpResponseMessage.Headers?.GetHeaderValue(key, stringComparison) ?? httpResponseMessage.Content.Headers.GetHeaderValue(key, stringComparison);
        }
    }
}
