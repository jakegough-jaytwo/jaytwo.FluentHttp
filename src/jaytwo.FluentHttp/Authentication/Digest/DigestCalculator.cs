#if !NETSTANDARD1_1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using jaytwo.AsyncHelper;

namespace jaytwo.FluentHttp.Authentication.Digest
{
    internal class DigestCalculator
    {
        public static async Task<string> GetResponseAsync(DigestServerParams digestServerParams, HttpRequestMessage request, string uri, string username, string password, string clientNonce, string nonceCount)
        {
            var method = $"{request.Method}";
            var selectedQop = SelectQop(digestServerParams.Qop);

            var ha1 = GetHA1(digestServerParams.Algorithm, digestServerParams.Realm, digestServerParams.Nonce, username, password, clientNonce);
            var ha2 = await GetHA2Async(selectedQop, uri, method, request);
            var response = GetResponse(selectedQop, digestServerParams.Nonce, ha1, ha2, clientNonce, nonceCount);

            return response;
        }

        public static string GetHA1(string algorithm, string realm, string nonce, string username, string password, string clientNonce)
        {
            if (string.IsNullOrEmpty(algorithm) || algorithm == Algorithm.Md5)
            {
                return GetMd5String(username, realm, password);
            }
            else if (algorithm == Algorithm.Md5Sess) // rare-ish
            {
                return GetMd5String(GetMd5String(username, realm, password), nonce, clientNonce);
            }

            throw new NotSupportedException("Unsupported algorithm directive: " + algorithm);
        }

        public static async Task<string> GetHA2Async(string qop, string uri, string method, HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(qop) || qop == Qop.Auth)
            {
                return GetMd5String(method, uri);
            }
            else if (qop == Qop.AuthInt) // super rare
            {
                byte[] bodyBytes;
                if (request.Content != null)
                {
                    await request.Content.LoadIntoBufferAsync();
                    bodyBytes = await request.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    bodyBytes = new byte[] { };
                }

                return GetMd5String(method, uri, GetMd5String(bodyBytes));
            }

            throw new NotSupportedException("Unsupported qop directive: " + qop);
        }

        public static string GetResponse(string qop, string nonce, string ha1, string ha2, string clientNonce, string nonceCount)
        {
            if (string.IsNullOrEmpty(qop))
            {
                return GetMd5String(ha1, nonce, ha2);
            }
            else if (qop == Qop.Auth || qop == Qop.AuthInt)
            {
                return GetMd5String(ha1, nonce, nonceCount, clientNonce, qop, ha2);
            }

            throw new NotSupportedException("Unsupported qop directive: " + qop);
        }

        public static string SelectQop(string qop)
        {
            var normalizedQopOptions = (qop ?? string.Empty).Split(',').Select(x => x.ToLower()).ToList();

            if (!normalizedQopOptions.Any())
            {
                return string.Empty;
            }
            else if (normalizedQopOptions.Contains(Qop.Auth))
            {
                return Qop.Auth;
            }
            else if (normalizedQopOptions.Contains(Qop.AuthInt))
            {
                return Qop.AuthInt;
            }
            else
            {
                return qop;
            }
        }

        public static string GetMd5String(params string[] values) => GetMd5String(string.Join(":", values));

        public static string GetMd5String(string stringToHash) => GetMd5String(Encoding.UTF8.GetBytes(stringToHash));

        public static string GetMd5String(byte[] bytes) => BitConverter.ToString(GetMd5Bytes(bytes)).Replace("-", string.Empty).ToLower();

        public static byte[] GetMd5Bytes(byte[] bytes)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(bytes);
            }
        }
    }
}
#endif
