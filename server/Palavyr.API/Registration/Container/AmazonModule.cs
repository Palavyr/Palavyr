using System;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

            // var loggerFactory = new LoggerFactory().AddLambdaLogger();
            // var logger = loggerFactory.CreateLogger<AmazonModule>();
            // logger.LogDebug("LOGGING!");

            // logger.LogDebug("====================================");
            // Console.WriteLine("====================================");
            //
            // logger.LogDebug($"Access Key: {accessKey}");
            // logger.LogDebug($"Secret Key: {string.Join("", secretKey.ToCharArray().Take(4).ToArray())}");
            //
            // Console.WriteLine($"Access Key: {accessKey}");
            // Console.WriteLine($"Secret Key: {string.Join("", secretKey.ToCharArray().Take(4).ToArray())}");
            //
            // logger.LogDebug("====================================");
            // Console.WriteLine("====================================");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var s3Config = new AmazonS3Config()
            {
                Timeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1,
                ForcePathStyle = true
            };

            var sesConfig = new AmazonSimpleEmailServiceConfig()
            {
                Timeout = TimeSpan.FromSeconds(100),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 5,
                RegionEndpoint = RegionEndpoint.USEast1,
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