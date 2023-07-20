using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace jaytwo.FluentHttp;

internal static class ContentTypeEvaluator
{
    public static bool IsJsonMediaType(string mediaType)
    {
        if (!string.IsNullOrEmpty(mediaType))
        {
            return IsJsonMediaType(MediaTypeHeaderValue.Parse(mediaType));
        }

        return false;
    }

    public static bool IsJsonMediaType(MediaTypeHeaderValue mediaTypeHeader)
    {
        if (mediaTypeHeader != null)
        {
            if (mediaTypeHeader.MediaType == "application/json"
                || mediaTypeHeader.MediaType.EndsWith("/json")
                || mediaTypeHeader.MediaType.EndsWith("+json"))
            {
                return true;
            }
        }

        return false;
    }

    public static bool CouldBeJsonString(string content)
    {
        if (content != null)
        {
            var trimmed = content.Trim();
            return (trimmed.StartsWith("{") && trimmed.EndsWith("}"))
                || (trimmed.StartsWith("[") && trimmed.EndsWith("]"));
        }

        return false;
    }

    public static bool IsBinaryMediaType(string mediaType)
    {
        if (!string.IsNullOrEmpty(mediaType))
        {
            return IsBinaryMediaType(MediaTypeHeaderValue.Parse(mediaType));
        }

        return false;
    }

    public static bool IsBinaryMediaType(MediaTypeHeaderValue mediaTypeHeader)
    {
        var knownBinaryMediaTypes = new[]
        {
            "application/octet-stream",
        };

        var binaryMediaTypePrefixes = new[]
        {
            "image/",
            "audio/",
            "video/",
            "font/",
            "application/vnd.openxmlformats-officedocument.",
            "application/vnd.ms-",
        };

        var binaryMediaTypeSuffixes = new[]
        {
            "/zip",
            "/pdf",
            "/x-pdf",
            "-compressed",
        };

        if (mediaTypeHeader != null)
        {
            var isKnownBinaryMediaType = knownBinaryMediaTypes.Contains(mediaTypeHeader.MediaType);
            var hasBinaryMediaTypePrefix = binaryMediaTypePrefixes.Any(x => mediaTypeHeader.MediaType.StartsWith(x));
            var hasBinaryMediaTypeSuffix = binaryMediaTypeSuffixes.Any(x => mediaTypeHeader.MediaType.EndsWith(x));

            var result = isKnownBinaryMediaType || hasBinaryMediaTypePrefix || hasBinaryMediaTypeSuffix;
            return result;
        }

        return false;
    }
}
