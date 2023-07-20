using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Exceptions;

public class UnexpectedStatusCodeException : Exception
{
    public UnexpectedStatusCodeException(HttpStatusCode statusCode, HttpResponseMessage response)
        : base(GetMessage(statusCode))
    {
        Response = response;
        StatusCode = response.StatusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public HttpResponseMessage Response { get; }

    private static string GetMessage(HttpStatusCode statusCode)
    {
        return $"Unexpected status code: {(int)statusCode} ({statusCode})";
    }
}
