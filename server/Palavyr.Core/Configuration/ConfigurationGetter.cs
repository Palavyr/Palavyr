using System.IO;
using Microsoft.Extensions.Configuration;

namespace Palavyr.Core.Configuration;

public static class ConfigurationGetter
{
    public static ConfigurationContainer GetConfiguration(string envFilePath = "../../local.env")
    {

        // We'll need to get the env vars set on lambda in the terraform.
        if (File.Exists(envFilePath))
        {
            DotEnv.Load(envFilePath);
        }
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "Palavyr_")
            .Build();

        return new ConfigurationContainer(config);
    }
}