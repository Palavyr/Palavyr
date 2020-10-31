using System;
using System.IO;

namespace Palavyr.Common.FileSystem.FormPaths
{
    public static class MagicPathStrings
    {
        public const string DataFolder = "PalavyrData";

        public static string InstallationRoot =>
            Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), DataFolder);
        
        public const string UserData = "UserData";
        public const string Attachments = "Attachments";
        public const string ResponsePDF = "ResponsePDF";
        public const string PreviewPDF = "PreviewPDF";
        public const string AreaData = "AreaData";
        public const string Logo = "AccountLogo";
    }
}