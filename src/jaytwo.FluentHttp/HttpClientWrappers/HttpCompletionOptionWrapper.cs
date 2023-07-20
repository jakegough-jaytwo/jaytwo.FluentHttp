using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class HttpCompletionOptionWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public HttpCompletionOptionWrapper(IHttpClient httpClient, HttpCompletionOption completionOption)
        : base(httpClient)
    {
        CompletionOption = completionOption;
    }

    public HttpCompletionOption CompletionOption { get; private set; }

    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
        => base.SendAsync(request, completionOption ?? CompletionOption, cancellationToken);
}
