using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.AsyncHelper;

namespace jaytwo.FluentHttp.Authentication
{
    public abstract class AuthenticationProviderBase : IAuthenticationProvider
    {
        // override either of these methods, but don't do both

        public virtual Task AuthenticateAsync(HttpRequestMessage request)
        {
            Authenticate(request);

#if NETFRAMEWORK || NETSTANDARD1_1
            return Task.FromResult(0);
#else
            return Task.CompletedTask;
#endif
        }

        public virtual void Authenticate(HttpRequestMessage request)
        {
            AuthenticateAsync(request).AwaitSynchronously();
        }
    }
}
