using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp.Authentication.Digest
{
    internal class DigestServerParams
    {
        public string Qop { get; set; }

        public string Nonce { get; set; }

        public string Realm { get; set; }

        public string Opaque { get; set; }

        public string Algorithm { get; set; }

        public string Stale { get; set; }

        public static DigestServerParams Parse(string wwwAuthenticateHeader)
        {
            var headerValues = DeserializeDictionary(wwwAuthenticateHeader);

            var result = new DigestServerParams();

            if (headerValues.TryGetValue("qop", out string qop))
            {
                result.Qop = qop;
            }

            if (headerValues.TryGetValue("nonce", out string nonce))
            {
                result.Nonce = nonce;
            }

            if (headerValues.TryGetValue("realm", out string realm))
            {
                result.Realm = realm;
            }

            if (headerValues.TryGetValue("opaque", out string opaque))
            {
                result.Opaque = opaque;
            }

            if (headerValues.TryGetValue("algorithm", out string algorithm))
            {
                result.Algorithm = algorithm;
            }

            if (headerValues.TryGetValue("stale", out string stale))
            {
                result.Stale = stale;
            }

            return result;
        }

        internal static IDictionary<string, string> DeserializeDictionary(string headerValue)
        {
            if (!headerValue.StartsWith("Digest "))
            {
                throw new InvalidOperationException("Not a digest header!");
            }

            headerValue = headerValue.Substring(7);

            var result = headerValue
                .Split(',')
                .Select(x => x.Trim())
                .Select(x => x.Split('='))
                .ToDictionary(x => x.First(), x => x.Last().TrimStart('"').TrimEnd('"'));

            return result;
        }

        internal static string SerializeDictionary(IDictionary<string, string> data)
        {
            var result = "Digest " + string.Join(", ", data.Select(x => $"{x.Key}=\"{x.Value}\""));

            return result;
        }
    }
}
