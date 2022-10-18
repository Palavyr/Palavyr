using System.Text.Json.Serialization;

namespace Palavyr.Core.Services.PdfService
{
    public class PdfServerResponse
    {
        [JsonPropertyName("s3Key")]
        public string S3Key { get; set; }

        [JsonPropertyName("fileNameWithExtension")]
        public string FileNameWithExtension { get; set; }

        [JsonPropertyName("fileStem")]
        public string FileStem { get; set; }
    }
}