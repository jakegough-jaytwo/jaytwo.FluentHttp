//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace jaytwo.FluentHttp.HttpClientWrappers
//{
//    public class CancellationTokenWrapper : HttpClientWrapper, IHttpClient
//    {
//        private readonly CancellationToken _cancellationToken;

//        public CancellationTokenWrapper(HttpClient httpClient, CancellationToken cancellationToken)
//            : base(httpClient)
//        {
//            _cancellationToken = cancellationToken;
//        }

//        public CancellationTokenWrapper(IHttpClient httpClient, CancellationToken cancellationToken)
//            : base(httpClient)
//        {
//            _cancellationToken = cancellationToken;
//        }

//        public CancellationToken CancellationToken => _cancellationToken;

//        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationToken))
//            {
//                return await base.SendAsync(request, completionOption, linkedTokenSource.Token);
//            }
//        }
//    }
//}
