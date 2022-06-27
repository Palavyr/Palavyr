using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Palavyr.Client
{
    internal static class ClientCallExtensionMethods
    {
        public static async Task<HttpResponseMessage> PostAsyncWithoutContent(this HttpClient client, string route)
        {
            return await client.PostAsync(route, new StringContent(""));
        }

        public static async Task<TResponse> PostWithApiKey<TResponse>(this HttpClient client, string requestUri, object data, CancellationToken cancellationToken = default)
        {
            if (!client.DefaultRequestHeaders.TryGetValues("test-only-apikey", out var enumerable))
            {
                throw new InvalidConstraintException("You need to use the ClientApiKey instead of the Client for apikey authorized endpoints in integration tests.");
            }

            var apikey = enumerable.ToArray().First();
            var requestUriWithApiKey = $"{requestUri}?key={apikey}";
            return await client.PostWithContent<TResponse>(requestUriWithApiKey, data, cancellationToken);
        }

        public static async Task<TResponse> PostWithContent<TResponse>(this HttpClient client, string requestUri, object data, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await client.PostAsync(requestUri, content, cancellationToken);
            return await message.ReadResponse<TResponse>();
        }

        public static async Task<TResponse> PutWithContent<TResponse>(this HttpClient client, string route, object data, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await client.PutAsync(route, content, cancellationToken);
            return await message.ReadResponse<TResponse>();
        }

        public static async Task<T> ReadResponse<T>(this HttpResponseMessage message)
        {
            var objectString = await message.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<T>(objectString);
            if (deserialized == null)
            {
                throw new SerializationException($"Failed to deserialize the provided type. Possible reason due to status code: {message.StatusCode} - {message.ReasonPhrase}");
            }

            return deserialized;
        }

        public static async Task<T> GetAsync<T>(this HttpClient client, string routeUri)
        {
            var response = await client.GetAsync(routeUri);
            return await response.ReadResponse<T>();
        }
    }
}