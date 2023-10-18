using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.Http;
using jaytwo.Http.Wrappers;
using Microsoft.Extensions.Logging;

namespace jaytwo.FluentHttp.HttpClientWrappers;

public class LoggingWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public LoggingWrapper(IHttpClient httpClient, ILogger logger)
        : base(httpClient)
    {
        Logger = logger;
    }

    public ILogger Logger { get; private set; }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = default, CancellationToken? cancellationToken = default)
    {
        // TODO: some kind of abstracted logging formatter

        var requestId = Guid.NewGuid().ToString();
        var loggerScopeValues = new Dictionary<string, object> { { "RequestId", requestId } };
        using var loggerScope = Logger?.BeginScope(loggerScopeValues);

        var shortRequestId = requestId.Substring(0, 7);
        string requestLog = null;
        string responseLog = null;
        HttpResponseMessage response = null;
        Exception exception = null;

        var stopwatch = Stopwatch.StartNew();
        try
        {
            loggerScopeValues.Add("RequestMethod", request.Method);
            loggerScopeValues.Add("RequestUri", request.RequestUri.OriginalString);
            // TODO: request body

            requestLog = string.Format(
                "[{0}] REQ: {1} {2}",
                shortRequestId,
                request.Method,
                request.RequestUri);

            var contentType = request.GetHeaderValue("Content-Type");
            if (!string.IsNullOrEmpty(contentType))
            {
                loggerScopeValues.Add("RequestContentType", contentType);
                requestLog += $"; {contentType}";
            }

            response = await base.SendAsync(request, completionOption, cancellationToken);

            // after just in case the content length isn't known until after the stream is read or something
            var contentLength = request.GetHeaderValue("Content-Length");
            if (!string.IsNullOrEmpty(contentLength) && long.TryParse(contentLength, out long longContentLength))
            {
                loggerScopeValues.Add("RequestContentLength", contentLength);
                requestLog += $"; {longContentLength:n0} bytes";
            }

            return response;
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            loggerScopeValues.Add("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds);
            // TODO: response body

            var toLog = requestLog;

            if (exception != null)
            {
                loggerScopeValues.Add("ExceptionType", exception.GetType());
                loggerScopeValues.Add("ExceptionMessage", exception.Message);

                responseLog = string.Format(
                    "[{0}] ERR: ({1:n0}ms) {2}; {3}",
                    shortRequestId,
                    stopwatch.ElapsedMilliseconds,
                    exception.GetType(),
                    exception.Message);
            }

            if (response != null)
            {
                loggerScopeValues.Add("ResponseStatusCode", (int)response.StatusCode);

                responseLog = string.Format(
                    "[{0}] RES: ({1:n0} ms) {2}",
                    shortRequestId,
                    stopwatch.ElapsedMilliseconds,
                    response.StatusCode);

                var contentType = response.GetHeaderValue("Content-Type");
                if (!string.IsNullOrEmpty(contentType))
                {
                    loggerScopeValues.Add("ResponseContentType", contentType);
                    responseLog += $"; {contentType}";
                }

                var contentLength = response.GetHeaderValue("Content-Length");
                if (!string.IsNullOrEmpty(contentLength) && long.TryParse(contentLength, out long longContentLength))
                {
                    loggerScopeValues.Add("ResponseContentLength", contentLength);
                    responseLog += $"; {longContentLength:n0} bytes";
                }
            }

            if (!string.IsNullOrEmpty(responseLog))
            {
                toLog += "\n" + responseLog;
            }

            if (exception != null)
            {
                Logger.LogError(toLog);
            }
            else
            {
                Logger.LogDebug(toLog);
            }
        }
    }
}
