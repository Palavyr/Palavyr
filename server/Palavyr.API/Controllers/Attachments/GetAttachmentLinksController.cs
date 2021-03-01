using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.GlobalConstants;
using Palavyr.Data;
using Palavyr.Domain.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    [Route("api")]
    [ApiController]
    public class GetAttachmentLinksController : AttachmentsBase
    {
        private readonly IConfiguration configuration;
        private DashContext dashContext;
        private ILogger<GetAttachmentLinksController> logger;
        private IAmazonS3 s3Client;

        public GetAttachmentLinksController(
            IConfiguration configuration,
            DashContext dashContext,
            ILogger<GetAttachmentLinksController> logger,
            IAmazonS3 s3Client
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.logger = logger;
            this.s3Client = s3Client;
        }
        
        [HttpGet("attachments/{areaId}")]
        public async Task<FileLink[]> Get([FromHeader] string accountId, string areaId)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;
            var fileLinks = await GetFileLinks(accountId, areaId, dashContext, logger, s3Client, previewBucket);
            return fileLinks.ToArray();
        }
    }
}