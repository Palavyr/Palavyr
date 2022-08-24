using System.IO;
using Microsoft.Extensions.Configuration;

namespace Palavyr.Core.Configuration;

public static class ConfigurationGetter
{
    public static ConfigurationContainer GetConfiguration()
    {
        var LocalEnvFile = "../../../../../.env.local";

        // We'll need to get the env vars set on lambda in the terraform.
        if (File.Exists(LocalEnvFile))
        {
            DotEnv.Load(LocalEnvFile);
        }
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "Palavyr_")
            .Build();

        return new ConfigurationContainer(config);
    }
}