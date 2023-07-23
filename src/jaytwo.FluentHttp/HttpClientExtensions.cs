using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.HttpClientWrappers;

namespace jaytwo.FluentHttp;

public static class HttpClientExtensions
{
    public static IHttpClient Wrap(this HttpClient httpClient)
        => new HttpClientWrapper(httpClient);

    public static HttpClient WithBaseAddress(this HttpClient httpClient, string baseUri, UriKind uriKind = UriKind.Absolute)
        => httpClient.WithBaseAddress(new Uri(baseUri, uriKind));

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

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilderAction)
        => await SendAsync(httpClient, requestBuilderAction, cancellationToken: default, completionOption: default);

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilderAction)
        => await SendAsync(httpClient, requestBuilderAction, cancellationToken: default, completionOption: default);

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilderAction, CancellationToken cancellationToken = default, HttpCompletionOption completionOption = default)
        => await SendAsync(
            httpClient,
            x =>
            {
                requestBuilderAction(x);
                return Task.CompletedTask;
            },
            cancellationToken,
            completionOption);

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilderAction, CancellationToken cancellationToken = default, HttpCompletionOption completionOption = default)
    {
        using var request = new HttpRequestMessage();
        await requestBuilderAction.Invoke(request);
        return await httpClient.SendAsync(request, completionOption, cancellationToken);
    }
}
