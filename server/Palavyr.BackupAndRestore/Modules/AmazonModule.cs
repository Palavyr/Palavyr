using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac;
using EmailService.ResponseEmail;
using Microsoft.Extensions.Configuration;
using Palavyr.Amazon.S3Services;

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

            base.Load(builder);
            builder.Register(
                    context => { return new AmazonS3Client(credentials); })
                .As<IAmazonS3>()
                .InstancePerLifetimeScope();

            builder.Register(
                context => { return new AmazonSimpleEmailServiceClient(credentials); }).As<IAmazonSimpleEmailService>();
            builder.RegisterType<SesEmail>().As<ISesEmail>();
            builder.RegisterType<S3Saver>().As<IS3Saver>();
        }
    }
}