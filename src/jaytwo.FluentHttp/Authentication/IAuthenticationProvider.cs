using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication
{
    public interface IAuthenticationProvider
    {
        Task AuthenticateAsync(HttpRequestMessage request);

        void Authenticate(HttpRequestMessage request);
    }
}
