using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using jaytwo.MimeHelper;

namespace jaytwo.FluentHttp
{
    internal static class ContentTypeEvaluator
    {
        public static bool IsJsonContent(string mediaType)
            => IsJsonContentType(MediaTypeHeaderValue.Parse(mediaType));

        public static bool IsJsonContentType(MediaTypeHeaderValue mediaTypeHeader)
        {
            if (mediaTypeHeader?.MediaType == MediaType.application_json
                || mediaTypeHeader.MediaType.EndsWith("/json")
                || mediaTypeHeader.MediaType.EndsWith("+json"))
            {
                return true;
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

        public static bool IsBinaryContent(MediaTypeHeaderValue mediaTypeHeader)
        {
            var mediaType = mediaTypeHeader?.MediaType ?? string.Empty;
            return IsBinaryMediaType(mediaType);
        }

        public static bool IsBinaryMediaType(string mediaType)
        {
            var knownBinaryMediaTypes = new[]
            {
                MediaType.application_octet_stream,
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
                "-compressed",
            };

            var isKnownBinaryMediaType = knownBinaryMediaTypes.Contains(mediaType);
            var hasBinaryMediaTypePrefix = binaryMediaTypePrefixes.Any(x => mediaType.StartsWith(x));
            var hasBinaryMediaTypeSuffix = binaryMediaTypeSuffixes.Any(x => mediaType.EndsWith(x));

            var result = isKnownBinaryMediaType || hasBinaryMediaTypePrefix || hasBinaryMediaTypeSuffix;
            return result;
        }
    }
}
