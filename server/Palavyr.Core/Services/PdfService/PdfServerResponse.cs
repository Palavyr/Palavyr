namespace Palavyr.Core.Services.PdfService
{
    public class PdfServerResponse
    {
        public string S3Key { get; set; }
        public string FileNameWithExtension { get; set; }
        public string FileStem { get; set; }
    }
}