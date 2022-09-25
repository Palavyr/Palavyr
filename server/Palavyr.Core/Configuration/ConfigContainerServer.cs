using Microsoft.Extensions.Configuration;

namespace Palavyr.Core.Configuration;

public class ConfigContainerMigrator
{
    public ConfigContainerMigrator(IConfiguration configuration)
    {
        DbConnectionString = configuration.CorrectConnectionString();
    }

    public string DbConnectionString { get; set; }
}

public class ConfigContainerServer
{
    // This is the one place where we'll be handling all of the env variables
    // so help me god if I find a call to 'configuration.get' outside of this class....
    public ConfigContainerServer(IConfiguration configuration)
    {
        StripeSecret = configuration.GetStripeKey();
        StripeWebhookSecret = configuration.GetStripeWebhookKey();
        AwsAccessKey = configuration.GetAccessKey();
        AwsSecretKey = configuration.GetSecretKey();
        AwsRegion = configuration.GetRegion();
        AwsUserDataBucket = configuration.GetUserDataBucket();
        AwsPdfUrl = configuration.GetPdfUrl();
        AwsS3ServiceUrl = configuration.GetAwsS3ServiceUrl();
        AwsSesServiceUrl = configuration.GetAwsSesServiceUrl();
        DbConnectionString = configuration.CorrectConnectionString();
        Environment = configuration.GetCurrentEnvironment();
        JwtSecretKey = configuration.GetJwtKey();
        SeqUrl = configuration.GetSeqUrl();
    }

    public string SeqUrl { get; set; }
    public string JwtSecretKey { get; set; }

    public string Environment { get; set; }

    public string DbConnectionString { get; set; }

    public string AwsSesServiceUrl { get; set; }

    public string AwsS3ServiceUrl { get; set; }

    public string AwsPdfUrl { get; set; }

    public string AwsUserDataBucket { get; set; }

    public string AwsRegion { get; set; }

    public string AwsSecretKey { get; set; }

    public string AwsAccessKey { get; set; }

    public string StripeSecret { get; set; }

    public string StripeWebhookSecret { get; set; }
}