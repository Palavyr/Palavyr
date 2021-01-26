namespace Palavyr.BackupAndRestore.Postgres
{
    public static class DatabaseConstants
    {
        public const string CompanyName = "Palavyr";
        private const string Accounts = "Accounts";
        private const string Configuration = "Configuration";
        private const string Conversation = "Conversations";
        public const string CheckSuffix = "Check";

        public static string[] Databases => new[] {Accounts, Configuration, Conversation};
        public static string FormCheckTableName(string database) => database + CheckSuffix;

        public const string BackupsSection = "Backups";
        public const string PostgresHost = "Postgres:host";
        public const string PostgresPort = "Postgres:port";
        public const string PostgresPassword = "Postgres:password";
    }
}