//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using jaytwo.FluentHttp.Authentication;

//namespace jaytwo.FluentHttp.HttpClientWrappers
//{
//    public class AuthenticationWrapper<T> : HttpClientWrapper, IHttpClient
//        where T : IAuthenticationProvider
//    {
//        private readonly T _authenticationProvider;

//        public AuthenticationWrapper(HttpClient httpClient, T authenticationProvider)
//            : base(httpClient)
//        {
//            _authenticationProvider = authenticationProvider;
//        }

//        public AuthenticationWrapper(IHttpClient httpClient, T authenticationProvider)
//            : base(httpClient)
//        {
//            _authenticationProvider = authenticationProvider;
//        }

//        public T AuthenticationProvider => _authenticationProvider;

//        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            await _authenticationProvider.AuthenticateAsync(request);
//            return await base.SendAsync(request, completionOption, cancellationToken);
//        }
//    }
//}
