namespace Palavyr.BackupAndRestore
{
    public static class DatabaseConstants
    {
        private const string Accounts = "Accounts";
        private const string  Configuration = "Configuration";
        private const string  Conversation = "Conversations";
        
        public static string[] Databases => new[] {Accounts, Configuration, Conversation};
        
        // alternate temp names
        private const string AccountsCheck = "AccountsCheck";
        private const string  ConfigurationCheck = "ConfigurationCheck";
        private const string  ConversationsCheck = "ConversationsCheck";
        
        public static string[] DatabasesRestoreCheck => new[] {AccountsCheck, ConfigurationCheck, ConversationsCheck};

        
    }
}