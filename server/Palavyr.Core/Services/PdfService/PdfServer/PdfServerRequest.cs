using System.Text.Json.Serialization;

namespace Palavyr.Core.Services.PdfService.PdfServer
{
    public class PdfServerRequest
    {
        [JsonPropertyName("bucket")] public string Bucket { get; set; }

        [JsonPropertyName("key")] public string Key { get; set; }

        [JsonPropertyName("html")] public string Html { get; set; }

        [JsonPropertyName("identifier")] public string Identifier { get; set; }

        [JsonPropertyName("region")] public string Region { get; set; }

        [JsonPropertyName("accesskey")] public string AccessKey { get; set; }

        [JsonPropertyName("secretkey")] public string SecretKey { get; set; }

        [JsonPropertyName("endpoint")] public string Endpoint { get; set; }

        [JsonPropertyName("paper")] public Paper Paper { get; set; }
    }
}