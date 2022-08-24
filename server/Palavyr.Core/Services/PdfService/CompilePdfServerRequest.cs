using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
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
        private readonly ConfigurationContainer configuration;

        public CompilePdfServerRequest(ConfigurationContainer configuration)
        {
            this.configuration = configuration;
        }

        public PdfServerRequest Compile(string key, string html, string identifier, Paper paperOptions)
        {
            var accessKey = configuration.AwsAccessKey;
            var secretKey = configuration.AwsSecretKey;
            var bucket = configuration.AwsUserDataBucket;
            var region = configuration.AwsRegion;

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