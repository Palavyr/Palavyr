namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationConstants
    {
        public const string SessionId = "abc123treu";
        public const string ApiKey = "fgsa-sagasf-asfsf";
        public const string Host = "127.0.0.1";
        public const string Port = "5432";

        public static string AccountDbConnString => $"Server={Host};Port={Port};Database=PalavyrIntegrationTestAccounts;User Id=TestUser;Password=12345";
        public static string DashDbConnString => $"Server={Host};Port={Port};Database=PalavyrIntegrationTestConfiguration;User Id=TestUser;Password=12345";
        public static string ConvoDbConnString => $"Server={Host};Port={Port};Database=PalavyrIntegrationTestConversations;User Id=TestUser;Password=12345";
    }
}