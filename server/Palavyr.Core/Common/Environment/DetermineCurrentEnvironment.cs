using System.Collections.Generic;
using Palavyr.Core.Configuration;

namespace Palavyr.Core.Common.Environment
{
    public interface IDetermineCurrentEnvironment
    {
        bool IsDevelopment();
        bool IsStaging();
        bool IsProduction();
        string GetCurrentEnvironment();

        public string Environment { get; set; }
    }

    public class DetermineCurrentEnvironment : IDetermineCurrentEnvironment
    {
        private readonly ConfigContainerServer config;

        public static List<string> Development = new() { "Development", "Dev", "development", "dev" };
        public static List<string> Staging = new() { "Staging", "staging", "test", "Test" };
        public static List<string> Production = new() { "Production", "production", "Prod", "prod" };

        public DetermineCurrentEnvironment(ConfigContainerServer config)
        {
            this.config = config;
        }

        public bool IsDevelopment()
        {
            return Development.Contains(Environment);
        }

        public bool IsStaging()
        {
            return Staging.Contains(Environment);
        }

        public bool IsProduction()
        {
            return Production.Contains(Environment);
        }

        public string GetCurrentEnvironment()
        {
            return config.Environment;
        }

        public string Environment
        {
            get => GetCurrentEnvironment();
            set => GetCurrentEnvironment();
        }
    }
}