using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.API.Controllers.Enquiries
{
    public class RetrieveFileLinkController : PalavyrBaseController
    {
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;
        private readonly IS3KeyResolver s3KeyResolver;

        public RetrieveFileLinkController(
            IConfiguration configuration,
            ILinkCreator linkCreator,
            IS3KeyResolver s3KeyResolver
        )
        {
            this.configuration = configuration;
            this.linkCreator = linkCreator;
            this.s3KeyResolver = s3KeyResolver;
        }

        [HttpGet("enquiries/link/{fileId}")]
        public string Get(
            [FromHeader] string accountId,
            [FromRoute] string fileId)
        {
            var s3Key = s3KeyResolver.ResolvePreviewKey(accountId, fileId);
            var previewBucket = configuration.GetPreviewBucket();
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, previewBucket);
            return preSignedUrl;
        }
    }
}