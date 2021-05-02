#nullable enable
using System.Linq;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoRetriever
    {
        string? GetLogo(string accountId);
    }

    public class LogoRetriever : ILogoRetriever
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;
        private readonly ILinkCreator linkCreator;

        public LogoRetriever(
            IConfiguration configuration,
            DashContext dashContext,
            ILinkCreator linkCreator
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.linkCreator = linkCreator;
        }

        public string? GetLogo(string accountId)
        {
            var userDataBucket = configuration.GetUserDataSection();
            var fileNameMap = dashContext.FileNameMaps.SingleOrDefault(x => x.AccountId == accountId && x.AreaIdentifier == "logo");

            if (fileNameMap == null)
            {
                return null;
            }

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(fileNameMap.S3Key, userDataBucket);
            return preSignedUrl;
        }
    }
}