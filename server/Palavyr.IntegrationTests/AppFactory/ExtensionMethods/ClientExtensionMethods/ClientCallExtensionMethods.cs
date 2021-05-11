using System.Net.Http;
using System.Threading.Tasks;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods
{
    public static class ClientCallExtensionMethods
    {
        public static async Task<HttpResponseMessage> PostAsyncWithoutContent(this HttpClient client, string route)
        {
            return await client.PostAsync(route, new StringContent(""));
        }
    }
}