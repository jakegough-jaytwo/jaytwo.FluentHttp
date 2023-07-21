using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.HttpClientWrappers;

internal class HttpClientWrapper : IHttpClient
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
       => await _httpClient.SendAsync(
           request,
           completionOption ?? HttpCompletionOption.ResponseHeadersRead,
           cancellationToken ?? CancellationToken.None);

    public virtual void Dispose()
        => _httpClient.Dispose();
}
