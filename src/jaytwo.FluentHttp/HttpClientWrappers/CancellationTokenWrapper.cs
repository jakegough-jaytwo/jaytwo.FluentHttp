using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.Http;
using jaytwo.Http.Wrappers;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class CancellationTokenWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public CancellationTokenWrapper(IHttpClient httpClient, CancellationToken cancellationToken)
        : base(httpClient)
    {
        CancellationToken = cancellationToken;
    }

    public CancellationToken CancellationToken { get; private set; }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption, CancellationToken? cancellationToken)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken ?? CancellationToken.None, CancellationToken);
        return await base.SendAsync(request, completionOption, linkedTokenSource.Token);
    }
}
