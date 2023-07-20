using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
