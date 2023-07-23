using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.HttpClientWrappers;
using jaytwo.Http.Authentication;

namespace jaytwo.FluentHttp;

public static class IHttpClientExtensions
{
    public static IHttpClient WithAuthentication(this IHttpClient httpClient, IAuthenticationProvider authenticationProvider)
        => new AuthenticationWrapper(httpClient, authenticationProvider);

    public static IHttpClient WithBasicAuthentication(this IHttpClient httpClient, string user, string pass)
        => httpClient.WithAuthentication(new BasicAuthenticationProvider(user, pass));

    public static IHttpClient WithTokenAuthentication(this IHttpClient httpClient, string token)
        => httpClient.WithAuthentication(new TokenAuthenticationProvider(token));

    public static IHttpClient WithTokenAuthentication(this IHttpClient httpClient, Func<string> tokenDelegate)
        => httpClient.WithAuthentication(new TokenAuthenticationProvider(tokenDelegate));

    public static IHttpClient WithTokenAuthentication(this IHttpClient httpClient, ITokenProvider tokenProvider)
       => httpClient.WithAuthentication(new TokenAuthenticationProvider(tokenProvider));

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
