namespace Palavyr.Core.Services.PdfService.PdfServer
{
    public class PdfServerRequest
    {
        public string Bucket { get; set; }
        public string Key { get; set; }
        public string Html { get; set; }
        public string Id { get; set; }
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public Paper Paper { get; set; }
    }
}