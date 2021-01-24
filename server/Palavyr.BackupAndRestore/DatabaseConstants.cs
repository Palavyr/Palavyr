namespace Palavyr.BackupAndRestore
{
    public static class DatabaseConstants
    {
        public const string CompanyName = "Palavyr";
        private const string Accounts = "Accounts";
        private const string  Configuration = "Configuration";
        private const string  Conversation = "Conversations";
        public const string CheckSuffix = "Check";
        
        public static string[] Databases => new[] {Accounts, Configuration, Conversation};
        public static string FormCheckTableName(string database) => database + CheckSuffix;
        
    }
}