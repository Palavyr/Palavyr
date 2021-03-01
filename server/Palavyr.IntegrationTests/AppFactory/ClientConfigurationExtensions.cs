using System.Net.Http;
using Palavyr.Common.RequestsTools;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class ClientConfigurationExtensions
    {
        public static void AddCustomHeaders(this HttpClient client)
        {
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
        }
    }
}