using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using jaytwo.Http.Authentication;

namespace jaytwo.FluentHttp.Authentication;

public static class HttpMessageHandlerExtensions
{
    public static HttpMessageHandler WithAuthentication(this HttpMessageHandler innerHandler, IAuthenticationProvider authenticationProvider)
        => new AuthenticationHttpMessageHandler(authenticationProvider, innerHandler);

    public static HttpMessageHandler WithBasicAuthentication(this HttpMessageHandler innerHandler, string user, string pass)
        => innerHandler.WithAuthentication(new BasicAuthenticationProvider(user, pass));

    public static HttpMessageHandler WithTokenAuthentication(this HttpMessageHandler innerHandler, string token)
        => innerHandler.WithAuthentication(new TokenAuthenticationProvider(token));

    public static HttpMessageHandler WithTokenAuthentication(this HttpMessageHandler innerHandler, Func<string> tokenDelegate)
        => innerHandler.WithAuthentication(new TokenAuthenticationProvider(tokenDelegate));

    public static HttpMessageHandler WithTokenAuthentication(this HttpMessageHandler innerHandler, ITokenProvider tokenProvider)
        => innerHandler.WithAuthentication(new TokenAuthenticationProvider(tokenProvider));
}
