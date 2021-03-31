using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;


//https://stackoverflow.com/questions/59200028/registering-more-amazons3client-with-configurations-on-autofac

namespace Palavyr.BackupAndRestore.Modules
{
    public class AmazonModule : Module
    {
        private readonly IConfiguration configuration;
        private const string AccessKeySection = "AWS:AccessKey";
        private const string SecretKeySection = "AWS:SecretKey";

        public AmazonModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var accessKey = configuration.GetSection(AccessKeySection).Value;
            var secretKey = configuration.GetSection(SecretKeySection).Value;
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

            builder.RegisterType<SesEmail>().As<ISesEmail>();
            builder.RegisterType<S3Saver>().As<IS3Saver>();
        }
    }
}