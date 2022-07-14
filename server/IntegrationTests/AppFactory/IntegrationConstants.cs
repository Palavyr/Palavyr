using System;
using System.Text;
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

    public static class ConnectionStringBuilder
    {
        public static string BuildAccountConnectionString()
        {
            return BuildConnectionString("PalavyrIntegrationTestAccounts");
        }

        public static string BuildConfigurationConnectionString()
        {
            return BuildConnectionString("PalavyrIntegrationTestConfiguration");
        }

        public static string BuildConversationConnectionString()
        {
            return BuildConnectionString("PalavyrIntegrationTestConversations");
        }

        static string BuildConnectionString(string databaseName)
        {
            var user = Environment.GetEnvironmentVariable("Palavyr__DB_User") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("Palavyr__DB_Password") ?? "Password01!";
            var host = Environment.GetEnvironmentVariable("Palavyr__DB__Host") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("Palavyr__DB__Port") ?? "5432";

            var builder = new StringBuilder();

            builder.Append($"Server={host};");
            builder.AppendLine($"Port={port};");
            builder.Append($"Database={databaseName};");
            builder.Append($"User Id={user};");
            builder.Append($"Password={password}");
            return builder.ToString();
        }
    }
}