using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Extensions
{
    public static class HttpClientPatchExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(
            this HttpClient client, string requestUri, T value, CancellationToken ct = default)
        {
            var msg = new HttpRequestMessage(HttpMethod.Patch, requestUri)
            {
                Content = JsonContent.Create(value)
            };
            return client.SendAsync(msg, ct);
        }
    }
}
