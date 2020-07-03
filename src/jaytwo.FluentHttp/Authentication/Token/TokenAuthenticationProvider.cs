using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Token
{
    public class TokenAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
    {
        private readonly ITokenProvider _tokenProvider;

        public TokenAuthenticationProvider(string token)
            : this(() => token)
        {
        }

        public TokenAuthenticationProvider(Func<string> tokenDelegate)
            : this(new DelegateTokenProvider(tokenDelegate))
        {
        }

        public TokenAuthenticationProvider(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public override async Task AuthenticateAsync(HttpRequestMessage request)
        {
            var token = await _tokenProvider.GetTokenAsync();
            request.WithHeaderAuthorization("Bearer", token);
        }
    }
}
