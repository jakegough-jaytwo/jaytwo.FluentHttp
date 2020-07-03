using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Token.OpenIdConnect
{
    public interface IAccessTokenProvider
    {
        Task<AccessTokenResponse> GetAccessTokenAsync();
    }
}
