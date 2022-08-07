using Microsoft.Extensions.Configuration;

namespace Palavyr.Core.Configuration;

public static class ConfigurationGetter
{
    public static IConfiguration GetConfiguration()
    {
        // We'll need to get the env vars set on lambda in the terraform.
        
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "Palavyr_")
            .Build();
        return config;
    }
}