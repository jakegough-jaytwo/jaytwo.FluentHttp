//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using jaytwo.FluentHttp.Exceptions;

//namespace jaytwo.FluentHttp.HttpClientWrappers
//{
//    public class TimeoutWrapper : HttpClientWrapper, IHttpClient
//    {
//        private readonly TimeSpan _timeout;

//        public TimeoutWrapper(HttpClient httpClient, TimeSpan timeout)
//            : base(httpClient)
//        {
//            _timeout = timeout;
//        }

//        public TimeoutWrapper(IHttpClient httpClient, TimeSpan timeout)
//            : base(httpClient)
//        {
//            _timeout = timeout;
//        }

//        public TimeSpan Timeout => _timeout;

//        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            using (var timeoutTokenSource = new CancellationTokenSource(_timeout))
//            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutTokenSource.Token))
//            {
//                try
//                {
//                    return await base.SendAsync(request, completionOption, linkedTokenSource.Token);
//                }
//                catch (TaskCanceledException taskCanceledException)
//                {
//                    if (timeoutTokenSource.IsCancellationRequested)
//                    {
//                        throw new RequestTimeoutException(request, taskCanceledException);
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }
//        }
//    }
//}