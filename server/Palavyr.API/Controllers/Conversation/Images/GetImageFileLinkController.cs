using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class GetImageFileLinkController : PalavyrBaseController
    {
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;
        private readonly IS3KeyResolver s3KeyResolver;

        public GetImageFileLinkController(
            IConfiguration configuration,
            ILinkCreator linkCreator,
            IS3KeyResolver s3KeyResolver
        )
        {
            this.configuration = configuration;
            this.linkCreator = linkCreator;
            this.s3KeyResolver = s3KeyResolver;
        }

        [HttpPost("images/link")]
        public string Get(
            [FromHeader] string accountId,
            [FromBody] ImageS3LinkRequest request)
        {
            var previewBucket = configuration.GetUserDataBucket();
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(request.S3Key, previewBucket);
            return preSignedUrl;
        }
    }

    public class ImageS3LinkRequest
    {
        public string S3Key { get; set; }
    }
}