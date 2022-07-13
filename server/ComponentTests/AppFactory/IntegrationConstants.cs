namespace Component.AppFactory
{
    public static class IntegrationConstants
    {
        public const string DefaultArea = "area-234";
        public const string EmailAddress = "test.palavyr@gmail.com";
        public const string Password = "12345";
        public const string DbPassword = "0987654321";
        public const string DbUser = "postgres";
        public const string Host = "127.0.0.1";
        public const string Port = "5432";
        public const string Localhost = "http://localhost/";
        public const string BasePath = "api/";
        public static string BaseUri => Localhost + BasePath;
        
        public static string AccountDbConnString = $"Server={Host};Port={Port};Database=PalavyrIntegrationTestAccounts;User Id={DbUser};Password={DbPassword}";
        public static string DashDbConnString = $"Server={Host};Port={Port};Database=PalavyrIntegrationTestConfiguration;User Id={DbUser};Password={DbPassword}";
        public static string ConvoDbConnString = $"Server={Host};Port={Port};Database=PalavyrIntegrationTestConversations;User Id={DbUser};Password={DbPassword}";
    }
}