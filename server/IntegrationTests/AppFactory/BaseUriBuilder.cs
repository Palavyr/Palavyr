using Palavyr.API.Controllers;

namespace IntegrationTests.AppFactory
{
    public static class BaseUriBuilder
    {
        public static string BuildBaseUri()
        {
            return $"http://localhost/{PalavyrBaseController.BaseRoute}/";
        }
    }
}