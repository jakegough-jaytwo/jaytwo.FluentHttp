using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class DelegatingHttpClientWrapper : IHttpClient
{
    private readonly IHttpClient _httpClient;

    public DelegatingHttpClientWrapper(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
        => await _httpClient.SendAsync(request, completionOption, cancellationToken);

    public virtual void Dispose()
        => _httpClient.Dispose();
}
