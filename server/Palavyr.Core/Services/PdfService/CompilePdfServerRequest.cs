using Palavyr.Core.Configuration;
using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public interface ICompilePdfServerRequest
    {
        PdfServerRequest Compile(string key, string html, string identifier, Paper paperOptions);
    }

    public class CompilePdfServerRequest : ICompilePdfServerRequest
    {
        private readonly ConfigContainerServer config;

        public CompilePdfServerRequest(ConfigContainerServer config)
        {
            this.config = config;
        }

        public PdfServerRequest Compile(string key, string html, string identifier, Paper paperOptions)
        {
            var accessKey = config.AwsAccessKey;
            var secretKey = config.AwsSecretKey;
            var bucket = config.AwsUserDataBucket;
            var region = config.AwsRegion;
            var endpoint = config.AwsS3ServiceUrl;

            return new PdfServerRequest
            {
                Bucket = bucket,
                Key = key,
                Html = html,
                Identifier = identifier,
                Paper = paperOptions,
                AccessKey = accessKey,
                SecretKey = secretKey,
                Region = region,
                Endpoint = endpoint
            };
        }
    }
}