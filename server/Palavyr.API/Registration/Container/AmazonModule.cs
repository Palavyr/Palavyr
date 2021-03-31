using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.GlobalConstants;

//https://stackoverflow.com/questions/59200028/registering-more-amazons3client-with-configurations-on-autofac

namespace Palavyr.API.Registration.Container
{
    public class AmazonModule : Module
    {
        private readonly IConfiguration configuration;
        
        public AmazonModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var accessKey = configuration.GetSection(ConfigSections.AccessKeySection).Value;
            var secretKey = configuration.GetSection(ConfigSections.SecretKeySection).Value;
            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var s3Config = new AmazonS3Config()
            {
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1
            };

            var sesConfig = new AmazonSimpleEmailServiceConfig()
            {
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1
            };

            base.Load(builder);
            builder.Register(
                    context => { return new AmazonS3Client(credentials, s3Config); })
                .As<IAmazonS3>()
                .InstancePerLifetimeScope();

            builder.Register(
                    context => { return new AmazonSimpleEmailServiceClient(credentials, sesConfig); })
                .As<IAmazonSimpleEmailService>()
                .InstancePerLifetimeScope();
        }
    }
}