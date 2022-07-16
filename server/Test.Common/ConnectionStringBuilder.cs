using System;
using System.Text;

namespace Test.Common
{
    public static class ConnectionStringBuilder
    {
        public static string BuildConnectionString()
        {
            return BuildConnectionString("PalavyrIntegrationTest");
        }

        private static string BuildConnectionString(string databaseName)
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