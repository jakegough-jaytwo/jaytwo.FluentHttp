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
//    public class BaseUriWrapper : HttpClientWrapper, IHttpClient
//    {
//        private readonly Uri _baseUri;

//        public BaseUriWrapper(HttpClient httpClient, Uri baseUri)
//            : base(httpClient)
//        {
//            _baseUri = baseUri;
//        }

//        public BaseUriWrapper(IHttpClient httpClient, Uri baseUri)
//            : base(httpClient)
//        {
//            _baseUri = baseUri;
//        }

//        public Uri BaseUri => _baseUri;

//        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            request.WithBaseUri(_baseUri);
//            return await base.SendAsync(request, completionOption, cancellationToken);
//        }
//    }
//}
