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

        public GetImageFileLinkController(
            IConfiguration configuration,
            ILinkCreator linkCreator
        )
        {
            this.configuration = configuration;
            this.linkCreator = linkCreator;
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