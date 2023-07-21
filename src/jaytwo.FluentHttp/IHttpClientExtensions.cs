using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp;

public static class IHttpClientExtensions
{
    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request)
        => httpClient.SendAsync(request, completionOption: null, CancellationToken.None);

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request, CancellationToken cancellationToken)
        => httpClient.SendAsync(request, completionOption: null, cancellationToken);

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption)
        => httpClient.SendAsync(request, completionOption, CancellationToken.None);

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, CancellationToken cancellationToken, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Action<HttpRequestMessageBuilder> requestBuilderAction, HttpCompletionOption completionOption)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, CancellationToken cancellationToken, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        var requestBuilder = new HttpRequestMessageBuilder();
        await requestBuilderAction.Invoke(requestBuilder);
        using (var request = await requestBuilder.BuildHttpRequestMessageAsync())
        {
            return await httpClient.SendAsync(request, completionOption, cancellationToken);
        }
    }

    private static async Task<HttpResponseMessage> SendAsync(IHttpClient httpClient, HttpMethod method, Action<HttpRequestMessageBuilder> requestBuilderAction)
    {
        return await httpClient.SendAsync(request =>
        {
            request.WithMethod(method);
            requestBuilderAction.Invoke(request);
        });
    }

    private static async Task<HttpResponseMessage> SendAsync(IHttpClient httpClient, HttpMethod method, Func<HttpRequestMessageBuilder, Task> requestBuilderAction)
    {
        return await httpClient.SendAsync(async request =>
        {
            request.WithMethod(method);
            await requestBuilderAction.Invoke(request);
        });
    }
}
