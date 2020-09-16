using System;
using System.IO;


namespace DashboardServer.API
{
    public static class MagicString
    {
        // Magic Strings
        public const string DataFolder = "PalavyrData";
        public const string LoginAction = "login";
        public const string LogoutAction = "logout";
        public const string SessionAction = "tubmcgubs";
        public const string Action = "action";
        public const string AccountId = "accountId";
        public const string SessionId = "sessionId";
        public const string DevAccess = "secretDevAccess";
        public const string DevAccount = "dashboardDev";
        public const string WidgetAccess = "widgetAccess";

        public const string AWSPreviewBucket = "convo-builder-previews";

        // private const string Databases = "Databases";
        public const string UserData = "UserData";
        public const string Attachments = "Attachments";
        public const string ResponsePDF = "ResponsePDF";
        public const string PreviewPDF = "PreviewPDF";
        public const string AreaData = "AreaData";
        public static string InstallationRoot => Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), DataFolder);
    }
}