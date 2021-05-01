using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.Enquiries
{
    public class RetrieveFileLinkController : PalavyrBaseController
    {
        private readonly ILogger<RetrieveFileLinkController> logger;
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;

        private string PreviewBucket => configuration.GetSection(ConfigSections.PreviewSection).Value;

        public RetrieveFileLinkController(
            ILogger<RetrieveFileLinkController> logger,
            IConfiguration configuration,
            ILinkCreator linkCreator
        )
        {
            this.logger = logger;
            this.configuration = configuration;
            this.linkCreator = linkCreator;
        }

        [HttpGet("enquiries/link/{fileId}")]
        public async Task<string> Get(
            [FromHeader] string accountId,
            [FromRoute] string fileId)
        {
            var preSignedUrl = await linkCreator.CreatePreSignedUrlResponseLink(accountId, fileId, PreviewBucket);
            return preSignedUrl;
        }
    }
}