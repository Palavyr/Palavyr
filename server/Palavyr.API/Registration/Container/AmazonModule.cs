using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac;
using Palavyr.Core.Configuration;
using Palavyr.Core.Services.EmailService.SmtpEmail;

//https://stackoverflow.com/questions/59200028/registering-more-amazons3client-with-configurations-on-autofac

namespace Palavyr.API.Registration.Container
{
    public class AmazonModule : Module
    {
        private readonly ConfigContainerServer config;

        public AmazonModule(ConfigContainerServer config)
        {
            this.config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var credentials = new BasicAWSCredentials(config.AwsAccessKey, config.AwsSecretKey);

            var s3Config = new AmazonS3Config
            {
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1,
                ForcePathStyle = true,
                ServiceURL = config.AwsS3ServiceUrl
            };

            var sesConfig = new AmazonSimpleEmailServiceConfig()
            {
                Timeout = TimeSpan.FromSeconds(100),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = config.AwsSesServiceUrl
            };

            builder.Register(
                    _ => new AmazonS3Client(credentials, s3Config))
                .As<IAmazonS3>()
                .InstancePerLifetimeScope();

            builder.Register(
                    _ => new AmazonSimpleEmailServiceClient(credentials, sesConfig))
                .As<IAmazonSimpleEmailService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SmtpEmailClient>().As<ISmtpEmailClient>();
            base.Load(builder);
        }
    }
}