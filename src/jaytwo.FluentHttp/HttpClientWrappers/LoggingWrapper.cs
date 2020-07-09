//#if !NETFRAMEWORK
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;

//namespace jaytwo.FluentHttp.HttpClientWrappers
//{
//    public class LoggingWrapper : HttpClientWrapper, IHttpClient
//    {
//        private readonly ILogger _logger;

//        public LoggingWrapper(HttpClient httpClient, ILogger logger)
//            : base(httpClient)
//        {
//            _logger = logger;
//        }

//        public LoggingWrapper(IHttpClient httpClient, ILogger logger)
//            : base(httpClient)
//        {
//            _logger = logger;
//        }

//        public ILogger Logger => _logger;

//        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
//        {
//            var requestId = Guid.NewGuid().ToString();
//            var shortRequestId = requestId.Substring(0, 7);
//            string before = null;
//            string after = null;
//            HttpResponseMessage response = null;
//            Exception exception = null;

//            var stopwatch = Stopwatch.StartNew();
//            try
//            {
//                before = string.Format(
//                    "[{0}] REQ: {1} {2}",
//                    shortRequestId,
//                    request.Method,
//                    request.RequestUri);

//                var contentType = request.GetHeaderValue("Content-Type");
//                if (!string.IsNullOrEmpty(contentType))
//                {
//                    before += $"; {contentType}";
//                }

//                response = await base.SendAsync(request, completionOption, cancellationToken);

//                // after just in case the content length isn't known until after the stream is read or something
//                var contentLength = request.GetHeaderValue("Content-Length");
//                if (!string.IsNullOrEmpty(contentLength) && long.TryParse(contentLength, out long longContentLength))
//                {
//                    before += $"; {longContentLength:n0} bytes";
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                throw;
//            }
//            finally
//            {
//                stopwatch.Stop();

//                var toLog = before;

//                if (exception != null)
//                {
//                    after = string.Format(
//                        "[{0}] ERR: ({1:n0}ms) {2}, {3}",
//                        shortRequestId,
//                        stopwatch.ElapsedMilliseconds,
//                        exception.GetType(),
//                        exception.Message);
//                }
//                else if (response != null)
//                {
//                    after = string.Format(
//                        "[{0}] RES: {1:n0} ms; {2}",
//                        shortRequestId,
//                        stopwatch.ElapsedMilliseconds,
//                        response.StatusCode);

//                    var contentType = response.GetHeaderValue("Content-Type");
//                    if (!string.IsNullOrEmpty(contentType))
//                    {
//                        after += $"; {contentType}";
//                    }

//                    var contentLength = response.GetHeaderValue("Content-Length");
//                    if (!string.IsNullOrEmpty(contentLength) && long.TryParse(contentLength, out long longContentLength))
//                    {
//                        after += $"; {longContentLength:n0} bytes";
//                    }
//                }

//                if (!string.IsNullOrEmpty(after))
//                {
//                    toLog += "\n" + after;
//                }

//                if (exception != null)
//                {
//                    _logger.LogError(toLog);
//                }
//                else
//                {
//                    _logger.LogInformation(toLog);
//                }
//            }
//        }
//    }
//}
//#endif
