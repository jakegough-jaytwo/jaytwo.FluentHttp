using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using jaytwo.FluentHttp.Authentication;
//using jaytwo.FluentHttp.Authentication.Basic;
//using jaytwo.FluentHttp.Authentication.Token;
using jaytwo.FluentHttp.HttpClientWrappers;
using Microsoft.Extensions.Logging;

namespace jaytwo.FluentHttp;

public static class IHttpClientExtensions
{
    //public static IHttpClient WrapWithCompletionOption(this IHttpClient httpClient, HttpCompletionOption completionOption)
    //    => new HttpCompletionOptionWrapper(httpClient, completionOption);

    //public static IHttpClient WrapWithCancellationToken(this IHttpClient httpClient, CancellationToken cancellationToken)
    //    => new CancellationTokenWrapper(httpClient, cancellationToken);

    public static IHttpClient WrapWithTimeout(this IHttpClient httpClient, TimeSpan timeout)
        => new TimeoutWrapper(httpClient, timeout);

    //public static IHttpClient WrapWithAuthentication<T>(this IHttpClient httpClient, T authenticationProvider)
    //    where T : IAuthenticationProvider
    //    => new AuthenticationWrapper<T>(httpClient, authenticationProvider);

    //public static IHttpClient WrapWithBasicAuthentication(this IHttpClient httpClient, string user, string pass)
    //    => WrapWithAuthentication(httpClient, new BasicAuthenticationProvider(user, pass));

    //public static IHttpClient WrapWithTokenAuthentication(this IHttpClient httpClient, string token)
    //    => WrapWithAuthentication(httpClient, new TokenAuthenticationProvider(token));

    //public static IHttpClient WrapWithTokenAuthentication(this IHttpClient httpClient, ITokenProvider tokenProvider)
    //    => WrapWithAuthentication(httpClient, new TokenAuthenticationProvider(tokenProvider));

    //public static IHttpClient WrapWithLogger(this IHttpClient httpClient, ILogger logger)
    //    => new LoggingWrapper(httpClient, logger);

    //public static IHttpClient WrapWithBaseUri(this IHttpClient httpClient, Uri baseUri)
    //    => new BaseUriWrapper(httpClient, baseUri);

    //public static IHttpClient WrapWithBaseUri(this IHttpClient httpClient, string baseUri)
    //    => new BaseUriWrapper(httpClient, new Uri(baseUri, UriKind.Absolute));

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request)
        => httpClient.SendAsync(request, completionOption: null, CancellationToken.None);

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request, CancellationToken cancellationToken)
        => httpClient.SendAsync(request, completionOption: null, cancellationToken);

    public static Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption)
        => httpClient.SendAsync(request, completionOption, CancellationToken.None);

    public static Task<HttpResponseMessage> DeleteAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Delete, requestBuilder);

    public static Task<HttpResponseMessage> DeleteAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Delete, requestBuilder);

    public static Task<HttpResponseMessage> DeleteAsync(this IHttpClient httpClient, Uri uri)
        => httpClient.DeleteAsync(x => x.WithUri(uri));

    public static Task<HttpResponseMessage> DeleteAsync(this IHttpClient httpClient, string pathOrUri)
        => httpClient.DeleteAsync(x => x.WithUri(pathOrUri));

    public static Task<HttpResponseMessage> DeleteAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
        => httpClient.DeleteAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> DeleteAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
        => httpClient.DeleteAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> GetAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Get, requestBuilder);

    public static Task<HttpResponseMessage> GetAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Get, requestBuilder);

    public static Task<HttpResponseMessage> GetAsync(this IHttpClient httpClient, Uri uri)
        => httpClient.GetAsync(x => x.WithUri(uri));

    public static Task<HttpResponseMessage> GetAsync(this IHttpClient httpClient, string pathOrUri)
        => httpClient.GetAsync(x => x.WithUri(pathOrUri));

    public static Task<HttpResponseMessage> GetAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
        => httpClient.GetAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> GetAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
        => httpClient.GetAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> HeadAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Head, requestBuilder);

    public static Task<HttpResponseMessage> HeadAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Head, requestBuilder);

    public static Task<HttpResponseMessage> HeadAsync(this IHttpClient httpClient, Uri uri)
        => httpClient.HeadAsync(x => x.WithUri(uri));

    public static Task<HttpResponseMessage> HeadAsync(this IHttpClient httpClient, string pathOrUri)
        => httpClient.HeadAsync(x => x.WithUri(pathOrUri));

    public static Task<HttpResponseMessage> HeadAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
        => httpClient.HeadAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> HeadAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
        => httpClient.HeadAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> OptionsAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Options, requestBuilder);

    public static Task<HttpResponseMessage> OptionsAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Options, requestBuilder);

    public static Task<HttpResponseMessage> OptionsAsync(this IHttpClient httpClient, string pathOrUri)
        => httpClient.OptionsAsync(x => x.WithUri(pathOrUri));

    public static Task<HttpResponseMessage> OptionsAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
        => httpClient.OptionsAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> OptionsAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
        => httpClient.OptionsAsync(x => x.WithUri(pathFormat, formatArgs));

    public static Task<HttpResponseMessage> PatchAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, new HttpMethod("PATCH"), requestBuilder);

    public static Task<HttpResponseMessage> PatchAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, new HttpMethod("PATCH"), requestBuilder);

    public static Task<HttpResponseMessage> PostAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Post, requestBuilder);

    public static Task<HttpResponseMessage> PostAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Post, requestBuilder);

    public static Task<HttpResponseMessage> PutAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Put, requestBuilder);

    public static Task<HttpResponseMessage> PutAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Put, requestBuilder);

    public static Task<HttpResponseMessage> TraceAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Trace, requestBuilder);

    public static Task<HttpResponseMessage> TraceAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        => SendAsync(httpClient, HttpMethod.Trace, requestBuilder);

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, CancellationToken cancellationToken, Action<HttpRequestMessage> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, Action<HttpRequestMessage> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Action<HttpRequestMessage> requestBuilder, HttpCompletionOption completionOption)
    {
        using (var request = new HttpRequestMessage())
        {
            requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Action<HttpRequestMessage> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, completionOption, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            await requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, CancellationToken cancellationToken, Func<HttpRequestMessage, Task> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            await requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, Func<HttpRequestMessage, Task> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            await requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, completionOption);
        }
    }

    public static async Task<HttpResponseMessage> SendAsync(this IHttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Func<HttpRequestMessage, Task> requestBuilder)
    {
        using (var request = new HttpRequestMessage())
        {
            await requestBuilder.Invoke(request);
            return await httpClient.SendAsync(request, completionOption, cancellationToken);
        }
    }

    private static async Task<HttpResponseMessage> SendAsync(IHttpClient httpClient, HttpMethod method, Action<HttpRequestMessage> requestBuilder)
    {
        return await httpClient.SendAsync(request =>
        {
            request.WithMethod(method);
            requestBuilder.Invoke(request);
        });
    }

    private static async Task<HttpResponseMessage> SendAsync(IHttpClient httpClient, HttpMethod method, Func<HttpRequestMessage, Task> requestBuilder)
    {
        return await httpClient.SendAsync(async request =>
        {
            request.WithMethod(method);
            await requestBuilder.Invoke(request);
        });
    }
}
