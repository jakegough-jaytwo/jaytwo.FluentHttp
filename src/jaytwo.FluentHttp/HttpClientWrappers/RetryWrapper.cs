using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Exceptions;
using jaytwo.Http;
using jaytwo.Http.Wrappers;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class RetryWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public RetryWrapper(IHttpClient httpClient)
        : base(httpClient)
    {
    }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
    {
        HttpResponseMessage response;

        int attempts = 0;
        do
        {
            response = await base.SendAsync(request, completionOption, cancellationToken);
            attempts++;

            if (ShouldRetry(response, attempts))
            {
                response.Dispose();

                var delay = GetRetryDelay(response, attempts);
                await Task.Delay(delay, cancellationToken ?? CancellationToken.None);
            }
            else
            {
                return response;
            }
        }
        while (true);
    }

    protected virtual bool ShouldRetry(HttpResponseMessage response, int attempts)
        => attempts < 5;

    protected virtual TimeSpan GetRetryDelay(HttpResponseMessage response, int attempts)
        => TimeSpan.FromMilliseconds(200);
}
