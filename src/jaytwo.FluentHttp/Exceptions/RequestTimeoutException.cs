using System;
using System.Net.Http;

namespace jaytwo.FluentHttp.Exceptions;

public class RequestTimeoutException : Exception
{
    public RequestTimeoutException(HttpRequestMessage request, Exception innerException)
        : base(GetMessage(request), innerException)
    {
        Request = request;
    }

    public HttpRequestMessage Request { get; }

    private static string GetMessage(HttpRequestMessage request)
    {
        return $"Request timed out";
    }
}
