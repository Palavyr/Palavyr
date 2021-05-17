using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public static class CompilePdfServerRequest
    {
        public static PdfServerRequest Compile(string bucket, string key, string accessKey, string secretKey, string region, string html, string identifier, Paper paperOptions)
        {
            return new PdfServerRequest()
            {
                Bucket = bucket,
                Key = key,
                Html = html,
                Id = identifier,
                Paper = paperOptions,
                AccessKey = accessKey,
                SecretKey = secretKey,
                Region = region
            };
        }
    }
}