using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Exceptions;
using jaytwo.Http;
using jaytwo.Http.Wrappers;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class RequestHeaderWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    private readonly Func<Task<IDictionary<string, string>>> _headersFactory;

    public RequestHeaderWrapper(IHttpClient httpClient, Func<Task<IDictionary<string, string>>> headersFactory)
        : base(httpClient)
    {
        _headersFactory = headersFactory;
    }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
    {
        var headers = await _headersFactory();

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        return await base.SendAsync(request, completionOption, cancellationToken);
    }
}
