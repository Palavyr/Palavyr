using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetAttachmentLinksController : AttachmentsBase
    {
        private DashContext dashContext;
        private ILogger<GetAttachmentLinksController> logger;
        private IAmazonS3 s3Client;

        public GetAttachmentLinksController(
            DashContext dashContext,
            ILogger<GetAttachmentLinksController> logger,
            IAmazonS3 s3Client
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
            this.s3Client = s3Client;
        }
        
        [HttpGet("attachments/{areaId}")]
        public async Task<IActionResult> Get([FromHeader] string accountId, string areaId)
        {
            var fileLinks = await GetFileLinks(accountId, areaId, dashContext, logger, s3Client);
            return Ok(fileLinks.ToArray());
        }
    }
}