using System.Net.Http;
using Palavyr.Core.Common.RequestsTools;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods
{
    public static class ClientConfigurationExtensionMethods
    {
        public static void AddCustomHeaders(this HttpClient client)
        {
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
        }
    }
}