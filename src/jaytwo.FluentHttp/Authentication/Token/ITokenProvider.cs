using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Token
{
    public interface ITokenProvider
    {
        Task<string> GetTokenAsync();
    }
}
