using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Token.OpenIdConnect
{
    public class ClientCredentialsTokenConfig
    {
        public string TokenUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Resource { get; set; }
    }
}
