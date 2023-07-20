using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp.Handlers;
using Microsoft.Extensions.Logging;

namespace jaytwo.FluentHttp;

public class FluentHttpClientBuilder
{
    public FluentHttpClientBuilder()
    {
    }

    public List<Func<HttpMessageHandler, DelegatingHandler>> HandlerBuilders { get; } = new List<Func<HttpMessageHandler, DelegatingHandler>>();

    public IWebProxy Proxy { get; set; }

    public CookieContainer CookieContainer { get; set; }

    public bool? AllowAutoRedirect { get; set; }

    public int? MaxAutomaticRedirections { get; set; }

    public DecompressionMethods? AutomaticDecompression { get; set; }

    public Uri BaseAddress { get; set; }

    public TimeSpan? Timeout { get; set; }

    public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

    public FluentHttpClientBuilder WithProxy(string host, int port)
        => WithProxy(new WebProxy(host, port));

    public FluentHttpClientBuilder WithProxy(IWebProxy proxy)
    {
        Proxy = proxy;
        return this;
    }

    public FluentHttpClientBuilder WithCookieContainer(CookieContainer cookieContainer)
    {
        CookieContainer = cookieContainer;
        return this;
    }

    public FluentHttpClientBuilder WithNewCookieContainer(out CookieContainer cookieContainer)
    {
        cookieContainer = new CookieContainer();
        return WithCookieContainer(cookieContainer);
    }

    public FluentHttpClientBuilder WithAllowAutoRedirect(bool allowAutoRedirect)
    {
        AllowAutoRedirect = allowAutoRedirect;
        return this;
    }

    public FluentHttpClientBuilder WithAllowAutoRedirect(bool allowAutoRedirect, int maxAutomaticRedirections)
    {
        AllowAutoRedirect = allowAutoRedirect;
        MaxAutomaticRedirections = maxAutomaticRedirections;
        return this;
    }

    public FluentHttpClientBuilder WithBaseAddress(Uri baseAddress)
    {
        BaseAddress = baseAddress;
        return this;
    }

    public FluentHttpClientBuilder WithTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
        return this;
    }

    public FluentHttpClientBuilder WithAutomaticDecompression(DecompressionMethods automaticDecompression)
    {
        AutomaticDecompression = automaticDecompression;
        return this;
    }

    public FluentHttpClientBuilder WithLogger(ILogger logger)
        => WithHandler(x => new LoggingHttpMessageHandler(x, logger));

    public FluentHttpClientBuilder WithHandler(Func<HttpMessageHandler, DelegatingHandler> handlerBuilder)
    {
        HandlerBuilders.Add(handlerBuilder);
        return this;
    }

    public FluentHttpClientBuilder WithRemoteCertificateValidationCallback(RemoteCertificateValidationCallback remoteCertificateValidationCallback)
    {
        RemoteCertificateValidationCallback = remoteCertificateValidationCallback;
        return this;
    }

    public HttpClient Build()
    {
#if NET5_0_OR_GREATER
        var baseHandler = new SocketsHttpHandler();

        if (RemoteCertificateValidationCallback != null)
        {
            baseHandler.SslOptions = new SslClientAuthenticationOptions()
            {
                RemoteCertificateValidationCallback = RemoteCertificateValidationCallback,
            };
        }
#else
        var baseHandler = new HttpClientHandler();

        if (RemoteCertificateValidationCallback != null)
        {
            baseHandler.ServerCertificateCustomValidationCallback = (req, cert, chain, errros) => RemoteCertificateValidationCallback(req, cert, chain, errros);
        }
#endif

        baseHandler.Proxy = Proxy ?? baseHandler.Proxy;
        baseHandler.CookieContainer = CookieContainer ?? baseHandler.CookieContainer;
        baseHandler.AllowAutoRedirect = AllowAutoRedirect ?? baseHandler.AllowAutoRedirect;
        baseHandler.AutomaticDecompression = AutomaticDecompression ?? baseHandler.AutomaticDecompression;
        baseHandler.MaxAutomaticRedirections = MaxAutomaticRedirections ?? baseHandler.MaxAutomaticRedirections;

        HttpMessageHandler hanlder = baseHandler;
        foreach (var handlerBuilder in HandlerBuilders)
        {
            hanlder = handlerBuilder.Invoke(hanlder);
        }

        var httpClient = new HttpClient(hanlder);
        httpClient.BaseAddress = BaseAddress ?? httpClient.BaseAddress;
        httpClient.Timeout = Timeout ?? httpClient.Timeout;

        return httpClient;
    }
}
