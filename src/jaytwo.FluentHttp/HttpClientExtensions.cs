using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.FluentHttp
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithBaseAddress(this HttpClient httpClient, string baseUri)
        {
            return httpClient.WithBaseAddress(new Uri(baseUri, UriKind.Absolute));
        }

        public static HttpClient WithBaseAddress(this HttpClient httpClient, Uri baseUri)
        {
            httpClient.BaseAddress = baseUri;

            return httpClient;
        }

        public static HttpClient WithTimeout(this HttpClient httpClient, TimeSpan timeout)
        {
            httpClient.Timeout = timeout;

            return httpClient;
        }

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, Uri uri)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(uri));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string pathOrUri)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(pathOrUri));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string pathFormat, params string[] formatArgs)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(pathFormat, formatArgs));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string pathFormat, params object[] formatArgs)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(pathFormat, formatArgs));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, Uri uri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string pathOrUri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, Uri uri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string pathOrUri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Delete).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri uri)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(uri));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string pathOrUri)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(pathOrUri));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string pathFormat, params string[] formatArgs)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(pathFormat, formatArgs));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string pathFormat, params object[] formatArgs)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(pathFormat, formatArgs));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri uri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string pathOrUri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri uri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string pathOrUri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Get).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri uri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string pathOrUri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri uri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string pathOrUri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, Uri uri, object payload)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).WithUri(uri).WithJsonContent(payload));

        //public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, string pathOrUri, object payload)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Post).WithUri(pathOrUri).WithJsonContent(payload));

        //public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri uri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string pathOrUri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri uri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string pathOrUri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PutJsonAsync(this HttpClient httpClient, Uri uri, object payload)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).WithUri(uri).WithJsonContent(payload));

        //public static Task<HttpResponseMessage> PutJsonAsync(this HttpClient httpClient, string pathOrUri, object payload)
        //    => httpClient.SendAsync(x => x.WithMethod(HttpMethod.Put).WithUri(pathOrUri).WithJsonContent(payload));

        //public static Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Uri uri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string pathOrUri, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Uri uri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").WithUri(uri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string pathOrUri, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").WithUri(pathOrUri).Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").Invoke(requestBuilder));

        //public static Task<HttpResponseMessage> PatchJsonAsync(this HttpClient httpClient, Uri uri, object payload)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").WithUri(uri).WithJsonContent(payload));

        //public static Task<HttpResponseMessage> PatchJsonAsync(this HttpClient httpClient, string pathOrUri, object payload)
        //    => httpClient.SendAsync(x => x.WithMethod("PATCH").WithUri(pathOrUri).WithJsonContent(payload));

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, CancellationToken cancellationToken, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, cancellationToken);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, CancellationToken cancellationToken, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, cancellationToken);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Action<HttpRequestMessage> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption, cancellationToken);
            }
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken cancellationToken, Func<HttpRequestMessage, Task> requestBuilder)
        {
            using (var request = new HttpRequestMessage())
            {
                await requestBuilder.Invoke(request);
                return await httpClient.SendAsync(request, completionOption, cancellationToken);
            }
        }

        //private static Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpMethod method, Action<HttpRequestMessage> requestBuilder)
        //{
        //    return httpClient.SendAsync(request =>
        //    {
        //        request.WithMethod(method);
        //        requestBuilder.Invoke(request);
        //    });
        //}

        //private static async Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpMethod method, Func<HttpRequestMessage, Task> requestBuilder)
        //{
        //    return await httpClient.SendAsync(async request =>
        //    {
        //        request.WithMethod(method);
        //        await requestBuilder.Invoke(request);
        //    });
        //}
    }
}
