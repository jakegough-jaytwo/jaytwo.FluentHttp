using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jaytwo.DateTimeHelper;
using Newtonsoft.Json;

namespace jaytwo.FluentHttp.Authentication.Token.OpenIdConnect
{
    public class AccessTokenResponse
    {
        public AccessTokenResponse()
        {
            Created = NowDelegate();
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        internal DateTimeOffset Created { get; set; }

        internal TimeSpan Threshold { get; set; } = TimeSpan.FromSeconds(60);

        internal Func<DateTimeOffset> NowDelegate { get; set; } = () => DateTimeOffset.Now;

        public bool IsFresh()
        {
            var expiration = Created
                .AddSeconds(ExpiresIn)
                .Subtract(Threshold);

            var now = NowDelegate();

            return now.IsBefore(expiration);
        }
    }
}
