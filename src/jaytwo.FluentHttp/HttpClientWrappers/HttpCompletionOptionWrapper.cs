//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace jaytwo.FluentHttp.HttpClientWrappers
//{
//    public class HttpCompletionOptionWrapper : HttpClientWrapper, IHttpClient
//    {
//        private readonly HttpCompletionOption _completionOption;

//        public HttpCompletionOptionWrapper(HttpClient httpClient, HttpCompletionOption completionOption)
//            : base(httpClient)
//        {
//            _completionOption = completionOption;
//        }

//        public HttpCompletionOptionWrapper(IHttpClient httpClient, HttpCompletionOption completionOption)
//            : base(httpClient)
//        {
//            _completionOption = completionOption;
//        }

//        public HttpCompletionOption CompletionOption => _completionOption;

//        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            return base.SendAsync(request, _completionOption, cancellationToken);
//        }
//    }
//}
