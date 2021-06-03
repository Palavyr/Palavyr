using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;

namespace Palavyr.Core.Common.Environment
{
    public interface IDetermineCurrentEnvironment
    {
        bool IsDevelopment();
        bool IsStaging();
        bool IsProduction();
        string GetCurrentEnvironment();
    }

    public class DetermineCurrentEnvironment : IDetermineCurrentEnvironment
    {
        private readonly IConfiguration configuration;

        private const string Development = "Development";
        private const string Staging = "Staging";
        private const string Production = "Production";

        public DetermineCurrentEnvironment(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool IsDevelopment()
        {
            var currentEnv = configuration.GetCurrentEnvironment();
            return currentEnv == Development;
        }
        public bool IsStaging()
        {
            var currentEnv = configuration.GetCurrentEnvironment();
            return currentEnv == Staging;
        }

        public bool IsProduction()
        {
            var currentEnv = configuration.GetCurrentEnvironment();
            return currentEnv == Production;
        }

        public string GetCurrentEnvironment()
        {
            var currentEnv = configuration.GetCurrentEnvironment();
            return currentEnv;
        }
        
    }
}