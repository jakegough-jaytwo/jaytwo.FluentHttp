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

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, CancellationToken cancellationToken, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, CancellationToken cancellationToken, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption, cancellationToken);
        }
    }
}
