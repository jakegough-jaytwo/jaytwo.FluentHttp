//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace jaytwo.FluentHttp.HttpClientWrappers
//{
//    public class HttpClientWrapper : IHttpClient
//    {
//        private static readonly HttpClientWrapper _default = new HttpClientWrapper(new HttpClient());

//        private readonly HttpClient _httpClient;
//        private readonly IHttpClient _ihttpClient;

//        public HttpClientWrapper(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public HttpClientWrapper(IHttpClient httpClient)
//        {
//            _ihttpClient = httpClient;
//        }

//        public static HttpClientWrapper Default => _default;

//        protected IHttpClient HttpClient => _ihttpClient ?? _httpClient.Wrap();

//        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//           => InternalSendAsync(request, completionOption, cancellationToken);

//        public virtual void Dispose()
//            => InternalDispose();

//        internal Task<HttpResponseMessage> InternalSendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            if (_ihttpClient != null)
//            {
//                return _ihttpClient.SendAsync(request, completionOption, cancellationToken);
//            }

//            return _httpClient.SendAsync(request, completionOption, cancellationToken);
//        }

//        internal void InternalDispose()
//        {
//            if (_ihttpClient != null)
//            {
//                _ihttpClient.Dispose();
//            }

//            _httpClient.Dispose();
//        }
//    }
//}
