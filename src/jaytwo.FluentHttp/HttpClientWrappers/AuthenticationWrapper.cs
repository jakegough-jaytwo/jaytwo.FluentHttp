using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.Http.Authentication;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class AuthenticationWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public AuthenticationWrapper(IHttpClient httpClient, IAuthenticationProvider authenticationProvider)
        : base(httpClient)
    {
        AuthenticationProvider = authenticationProvider;
    }

    protected IAuthenticationProvider AuthenticationProvider { get; private set; }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null)
    {
        await AuthenticationProvider.AuthenticateAsync(request);
        return await base.SendAsync(request, completionOption, cancellationToken);
    }
}
