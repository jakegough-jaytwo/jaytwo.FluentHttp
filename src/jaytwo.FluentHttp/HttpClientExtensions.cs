using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithBaseAddress(this HttpClient httpClient, string baseUri)
        {
            return httpClient.WithBaseAddress(new Uri(baseUri, UriKind.Absolute));
        }

        public static HttpClient WithBaseAddress(this HttpClient httpClient, Uri baseUri)
        {
            httpClient.BaseAddress = baseUri;

            return httpClient;
        }

        public static HttpClient WithTimeout(this HttpClient httpClient, TimeSpan timeout)
        {
            httpClient.Timeout = timeout;

            return httpClient;
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, CancellationToken cancellationToken, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, cancellationToken);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, CancellationToken cancellationToken, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, cancellationToken);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption, cancellationToken);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption, cancellationToken);
            }
        }
    }
}
