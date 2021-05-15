using System.Net.Http;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods
{
    public static class ClientConfigurationExtensionMethods
    {
        public static void AddCustomHeaders(this HttpClient client)
        {
            client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.Action, ApplicationConstants.MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
        }
    }
}