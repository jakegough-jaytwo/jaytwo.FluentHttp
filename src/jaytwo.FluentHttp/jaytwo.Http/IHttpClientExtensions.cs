using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.HttpClientWrappers;
using Microsoft.Extensions.Logging;

namespace jaytwo.Http;

public static class IHttpClientExtensions
{
    public static IHttpClient WithBaseUri(this IHttpClient httpClient, string baseUri, UriKind uriKind = UriKind.RelativeOrAbsolute)
        => new BaseUriWrapper(httpClient, new Uri(baseUri, uriKind));

    public static IHttpClient WithBaseUri(this IHttpClient httpClient, Uri baseUri)
        => new BaseUriWrapper(httpClient, baseUri);

    public static IHttpClient WithCancellationToken(this IHttpClient httpClient, CancellationToken cancellationToken)
        => new CancellationTokenWrapper(httpClient, cancellationToken);

    public static IHttpClient WithCompletionOption(this IHttpClient httpClient, HttpCompletionOption completionOption)
        => new HttpCompletionOptionWrapper(httpClient, completionOption);

    public static IHttpClient WithLogger(this IHttpClient httpClient, ILogger logger)
        => new LoggingWrapper(httpClient, logger);

    public static IHttpClient WithTimeout(this IHttpClient httpClient, TimeSpan timeout)
        => new TimeoutWrapper(httpClient, timeout);

    public static IHttpClient WithRequestHeaders(this IHttpClient httpClient, Func<Task<IDictionary<string, string>>> headersFactory)
        => new RequestHeaderWrapper(httpClient, headersFactory);

    public static IHttpClient WithRequestHeaders(this IHttpClient httpClient, Func<IDictionary<string, string>> headersFactory)
        => WithRequestHeaders(httpClient, () => Task.FromResult(headersFactory.Invoke()));

    public static IHttpClient WithRequestHeaders(this IHttpClient httpClient, IDictionary<string, string> headers)
        => WithRequestHeaders(httpClient, () => headers);

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request)
        => SendAsync(httpClient, request, completionOption: default, cancellationToken: default);

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
        => httpClient.SendAsync(request, completionOption, cancellationToken);

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilderAction)
        => await SendAsync(httpClient, requestBuilderAction, completionOption: default, cancellationToken: default);

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilderAction)
        => await SendAsync(httpClient, requestBuilderAction, completionOption: default, cancellationToken: default);

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilderAction, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
        => await SendAsync(
            httpClient,
            x =>
            {
                requestBuilderAction.Invoke(x);
                return Task.CompletedTask;
            },
            completionOption,
            cancellationToken);

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilderAction, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
    {
        using var request = new HttpRequestMessage();
        await requestBuilderAction.Invoke(request);
        return await httpClient.SendAsync(request, completionOption ?? default, cancellationToken ?? default);
    }
}
