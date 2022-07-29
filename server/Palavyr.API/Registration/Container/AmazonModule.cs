using System;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.EmailService.SmtpEmail;

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
            var accessKey = configuration.GetAccessKey();
            var secretKey = configuration.GetSecretKey();
            
            // default s3 endpoint is s3.amazonaws.com
            // default ses endpoint is ses.amazonaws.com

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var s3serviceUrl = configuration.GetAwsS3ServiceUrl();
            var s3Config = new AmazonS3Config
            {
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1,
                ForcePathStyle = true,
                ServiceURL = s3serviceUrl
            };

            var sesServiceUrl = configuration.GetAwsSESServiceUrl();
            var sesConfig = new AmazonSimpleEmailServiceConfig()
            {
                Timeout = TimeSpan.FromSeconds(100),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = sesServiceUrl
            };

            builder.Register(
                    context => { return new AmazonS3Client(credentials, s3Config); })
                .As<IAmazonS3>()
                .InstancePerLifetimeScope();

            builder.Register(
                    context => { return new AmazonSimpleEmailServiceClient(credentials, sesConfig); })
                .As<IAmazonSimpleEmailService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SmtpEmailClient>().As<ISmtpEmailClient>();
            base.Load(builder);
        }
    }
}