using System;

namespace Palavyr.Core.Services.PdfService
{
    public static class PdfServerConstants
    {
        public static Func<string, string, string> PdfServiceUrl = (host, port) => $"http://{host}:{port}/create-pdf";
        public const string Bucket = "bucket";
        public const string Key = "key";
        public const string Html = "html";
        public const string Id = "id";

        public const string Region = "region";
        public const string AccessKey = "accesskey";
        public const string SecretKey = "secretkey";

        public const string PaperOrientation = "paperOrientation";
        public const string PaperFormat = "paperFormat";
        public const string PaperBorder = "paperBorder";
        public const string PaperFooterHeight = "paperFooterHeight";
        public const string PaperFooterContentsDefault = "paperFooterContentsDefault";
    }
}