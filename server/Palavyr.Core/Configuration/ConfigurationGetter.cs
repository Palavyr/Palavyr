using System.IO;
using Microsoft.Extensions.Configuration;

namespace Palavyr.Core.Configuration;

public static class ConfigurationGetter
{
    public static ConfigContainerServer GetConfiguration()
    {
        var envFilePath = "../../local.env";

        // We'll need to get the env vars set on lambda in the terraform.
        if (File.Exists(envFilePath))
        {
            DotEnv.Load(envFilePath);
        }

        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "Palavyr_")
            .Build();

        return new ConfigContainerServer(config);
    }

    public static ConfigContainerMigrator GetConfigurationForMigrator()
    {
        var envFilePath = "../../../../../local.env";

        // We'll need to get the env vars set on lambda in the terraform.
        if (File.Exists(envFilePath))
        {
            DotEnv.Load(envFilePath);
        }

        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "Palavyr_")
            .Build();

        return new ConfigContainerMigrator(config);
    }
}