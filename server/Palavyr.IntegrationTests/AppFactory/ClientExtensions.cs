using System.Net.Http;
using System.Threading.Tasks;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class ClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsyncWithoutContent(this HttpClient client, string route)
        {
            return await client.PostAsync(route, new StringContent(""));
        }
    }
}