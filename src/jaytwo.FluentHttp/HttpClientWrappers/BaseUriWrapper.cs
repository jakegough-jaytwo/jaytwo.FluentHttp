using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class BaseUriWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public BaseUriWrapper(IHttpClient httpClient, Uri baseUri)
        : base(httpClient)
    {
        BaseUri = baseUri;
    }

    protected Uri BaseUri { get; private set; }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null)
    {
        request.WithBaseUri(BaseUri);
        return await base.SendAsync(request, completionOption, cancellationToken);
    }
}
