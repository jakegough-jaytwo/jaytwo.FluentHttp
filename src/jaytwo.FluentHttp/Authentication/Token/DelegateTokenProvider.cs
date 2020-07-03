using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Token
{
    public class DelegateTokenProvider : ITokenProvider
    {
        private readonly Func<Task<string>> _tokenDelegate;

        public DelegateTokenProvider(Func<Task<string>> tokenDelegate)
        {
            _tokenDelegate = tokenDelegate;
        }

        public DelegateTokenProvider(Func<string> tokenDelegate)
            : this(() => Task.FromResult(tokenDelegate.Invoke()))
        {
        }

        public Task<string> GetTokenAsync() => _tokenDelegate.Invoke();
    }
}
