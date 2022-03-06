using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public interface ICompilePdfServerRequest
    {
        PdfServerRequest Compile(string bucket, string key, string html, string identifier, Paper paperOptions);
    }

    public class CompilePdfServerRequest : ICompilePdfServerRequest
    {
        private readonly IConfiguration configuration;

        public CompilePdfServerRequest(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public PdfServerRequest Compile(string bucket, string key, string html, string identifier, Paper paperOptions)
        {
            var accessKey = configuration.GetAccessKey();
            var secretKey = configuration.GetSecretKey();
            var region = configuration.GetRegion();

            return new PdfServerRequest
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