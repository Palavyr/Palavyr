namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationConstants
    {
        public const string SessionId = "abc123treu";
        public const string ApiKey = "fgsa-sagasf-asfsf";
        public const string AccountId = "345jhgk435";
        public const string EmailAddress = "test@gmail.com";
        public const string Password = "12345";
        public const string DbPassword = "0987654321";
        public const string DbUser = "postgres";
        public const string Host = "127.0.0.1";
        public const string Port = "5432";
        public const string Localhost = "http://localhost/";
        public const string BasePath = "api/";
        public static string BaseUri => Localhost + BasePath;
        
        public static string AccountDbConnString(string id) => $"Server={Host};Port={Port};Database=PalavyrIntegrationTestAccounts-{id};User Id={DbUser};Password={DbPassword}";
        public static string DashDbConnString(string id) => $"Server={Host};Port={Port};Database=PalavyrIntegrationTestConfiguration-{id};User Id={DbUser};Password={DbPassword}";
        public static string ConvoDbConnString(string id) => $"Server={Host};Port={Port};Database=PalavyrIntegrationTestConversations-{id};User Id={DbUser};Password={DbPassword}";
    }
}